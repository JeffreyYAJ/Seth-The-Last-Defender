using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;
using UnityEngine;

namespace SethPrime
{
    public class StunCombo : StateModifierBase
    {

        public override string BindState => "Stun Combo";

        // ctor : utilise la signature standard du StateModifierBase du projet
        public StunCombo(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {

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

                    // Gravité renforcée
                    new SetGravity2dScaleAction()
                    {
                        gameObject = new FsmOwnerDefault
                        {
                            OwnerOption = OwnerDefaultOption.UseOwner
                        },
                        gravityScale = 2f
                    },

                    // Animation
                    new AnimationPlayerAction()
                    {
                        animator = wrapper.animator,
                        ClipName = "Stun Air"
                    },

                    // IMPULSION UNIQUE (PAS EN UPDATE)
                    new SetVelocityToPlayer()
                    {
                        Rb = wrapper.rb,
                        velocity = -10f,
                        velocityY = 23f,
                        ResetOnUpdate = true // 🔴 CRITIQUE
                    },

                    // Décélération naturelle
                    new DecelerateXY()
                    {
                        decelerationX = 0.9f,
                        decelerationY = 0.9f,
                    },

                    // SON
                    new PlayClipAction
                    {
                        Clip = wrapper.SethStunAudio,
                        Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                    },

                    new PlayClipAction
                    {
                        Clip = wrapper.BossStunAudio,
                        Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                    },

                    // ✅ CHECK À LA FIN
                    new CheckYVelocityAction()
                    {
                        Rb = wrapper.rb,
                        Velocity = 0.01f,
                        OnVelocityMatch = FsmEvent.GetFsmEvent("FINISHED")
                    }

                },

                Transitions = new[]
                {
                    new FsmTransition()
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Recover Combo",
                        ToFsmState = fsm.Fsm.GetState("Recover Combo")
                    }
                }
            };

            // ajoute l'état au FSM (préserve les états existants)
            fsm.Fsm.States = fsm.FsmStates.Append(state).ToArray();
            SethPrimeMain.Log.LogInfo("[StunCombo] State créé et ajouté au Control FSM : " + BindState);
        }

        // phases vides — on crée l'état dans OnCreateModifier
        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}