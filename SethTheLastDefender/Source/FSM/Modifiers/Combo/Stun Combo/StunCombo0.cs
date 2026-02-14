using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;
using UnityEngine;

namespace SethPrime
{
    public class StunCombo0 : StateModifierBase
    {

        public override string BindState => "Stun Combo 0";

        // ctor : utilise la signature standard du StateModifierBase du projet
        public StunCombo0(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                    new SetCollider()
                    {
                        gameObject = new FsmOwnerDefault
                        {
                            OwnerOption = OwnerDefaultOption.UseOwner
                        },
                        active = true
                    },

                    // 🔴 FORCE RÉINTÉGRATION PHYSIQUE
                    new SetGravity2dScale()
                    {
                        gameObject = new FsmOwnerDefault
                        {
                            OwnerOption = OwnerDefaultOption.UseOwner
                        },
                        gravityScale = 0.01f // PAS 0
                    },

                    // 🔴 TICK PHYSIQUE GARANTI
                    new Wait()
                    {
                        time = 0f,
                        realTime = false
                    },

                    // RESET FINAL
                    new SetVelocity2d()
                    {
                        gameObject = new FsmOwnerDefault
                        {
                            OwnerOption = OwnerDefaultOption.UseOwner
                        },
                        vector = Vector2.zero
                    },

                    new SendEvent()
                    {
                        sendEvent = FsmEvent.GetFsmEvent("FINISHED")
                    }
                },

                Transitions = new[]
                {
                    new FsmTransition()
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Stun Combo",
                        ToFsmState = fsm.Fsm.GetState("Stun Combo")
                    }
                }
            };

            // ajoute l'état au FSM (préserve les états existants)
            fsm.Fsm.States = fsm.FsmStates.Append(state).ToArray();
            SethPrimeMain.Log.LogInfo("[StunCombo0] State créé et ajouté au Control FSM : " + BindState);
        }

        // phases vides — on crée l'état dans OnCreateModifier
        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}