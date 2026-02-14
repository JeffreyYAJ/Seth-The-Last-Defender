using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;
using UnityEngine;


namespace SethPrime
{
    public class Phase4RecoveringState : StateModifierBase
    {

        public override string BindState => "Phase 4 Recovering State";

        public Phase4RecoveringState(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            CreateBindState();
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }

        private void CreateBindState()
        {
            if (fsm.Fsm.GetState(BindState) != null) return;

            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Actions = new FsmStateAction[]
                {
                    // Attendre que la vélocité verticale soit quasi nulle (touche le sol)
                    new FadeVelocityAction()
                    {
                        Rb = wrapper.rb,
                        Duration = 0.2f
                    },

                    new AnimationPlayerAction()
                    {
                        animator = wrapper.animator,
                        ClipName = "Stun Land"
                    },
                    new SetVelocity()
                    {
                        vector = Vector3.zero,
                        x = 0f,
                        y = 0f
                    },

                    // Pause la durée du stun
                    new Wait() { time = 2f, finishEvent = FsmEvent.GetFsmEvent("FINISHED") },

                },

                Transitions = new[]
                {
                    new FsmTransition()
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Phase 4 Boom 1",
                        ToFsmState = fsm.Fsm.GetState("Phase 4 Boom 1")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();
            SethPrimeMain.Log.LogInfo("[Phase4RecoveringState] State créé et ajouté au Control FSM : " + BindState);
        }
    }
}
