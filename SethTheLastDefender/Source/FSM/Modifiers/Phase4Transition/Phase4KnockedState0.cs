using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Phase4KnockedState0 : StateModifierBase
    {
        public override string BindState => "Phase 4 Knocked 0";

        public Phase4KnockedState0(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Phase 4 Knocked 01",
                        ToFsmState = fsm.Fsm.GetState("Phase 4 Knocked 01")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState("Stab Recover");
            if (source != null)
                SethFsmController.CloneActions(source, state);

            state.Actions = state.Actions
            .Concat(new FsmStateAction[]
            {
                new SetIsInComboAction(wrapper, false),
                new Wait() { time = 0.001f, finishEvent = FsmEvent.GetFsmEvent("FINISHED") },
            })
            .ToArray();

            ForceColliderOn(state);
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}
