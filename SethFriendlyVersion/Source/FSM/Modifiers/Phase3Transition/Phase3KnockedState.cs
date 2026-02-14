using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;
using UnityEngine;

namespace SethPrime
{
    public class Phase3KnockedState : StateModifierBase
    {
        private GameObject finalHitObject;

        public override string BindState => "Phase 3 Knocked";

        // ctor : utilise la signature standard du StateModifierBase du projet
        public Phase3KnockedState(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            // On cherche l'explosion finale utilisée par les boss vanilla (optionnel)
            foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go == null) continue;
                if (go.name.Contains("Boss Death FinalHit"))
                {
                    finalHitObject = go;
                    break;
                }
            }

            CreateBindState();
        }      

        private void CreateBindState()
        {
            // protège contre double création si l'état existe déjà
            if (fsm.Fsm.GetState(BindState) != null) return;

            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Actions = new FsmStateAction[]
                {
                    new SetCollider()
                    {
                        gameObject = new FsmOwnerDefault
                        {
                            OwnerOption = OwnerDefaultOption.UseOwner
                        },
                        active = true
                    },
                    new PauseBossMusicAction() { Pause = true },
                    new SetGravity2dScaleAction()
                    {
                        gameObject = new FsmOwnerDefault
                        {
                            OwnerOption = OwnerDefaultOption.UseOwner
                        },
                        gravityScale = 2f
                    },
                    //BASE
                    new CheckYVelocityAction()
                    {
                        Rb = wrapper.rb,
                        Velocity = 0.01f,
                        OnVelocityMatch = FsmEvent.GetFsmEvent("FINISHED")
                    },
                    
                    new AnimationPlayerAction()
                    {
                        animator = wrapper.animator,
                        ClipName = "Stun Air"
                    },
                    new SetVelocityToPlayer()
                    {
                        Rb = wrapper.rb,
                        velocity = -10f,
                        velocityY = 23f,
                        ResetOnUpdate = true
                    },
                    new DecelerateXY()
                    {
                        decelerationX = 0.9f,
                        decelerationY = 0.9f,
                    },

                    //Jouer les sons "pseudo-mort" si présents (façon PlayMaker)
                    new PlayClipAction()
                    {
                        Clip = wrapper.SethDeath2Audio,
                        Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                    },

                    new PlayClipAction()
                    {
                        Clip = wrapper.BossGenericDeathAudio,
                        Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                    },

                    // spawn final hit prefab s'il existe
                    new SpawnPrefabAction()
                    {
                        Prefab = finalHitObject
                    },

                },

                Transitions = new[]
                {
                    new FsmTransition()
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Phase 3 Recovering State",
                        ToFsmState = fsm.Fsm.GetState("Phase 3 Recovering State")
                    }
                }
            };

            // ajoute l'état au FSM (préserve les états existants)
            fsm.Fsm.States = fsm.FsmStates.Append(state).ToArray();
            SethPrimeMain.Log.LogInfo("[Phase3KnockedState] State créé et ajouté au Control FSM : " + BindState);
        }

        // phases vides — on crée l'état dans OnCreateModifier
        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}