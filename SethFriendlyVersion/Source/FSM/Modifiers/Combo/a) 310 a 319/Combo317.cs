using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo317 : StateModifierBase
    {
        public override string BindState => "Combo 317";

        public Combo317(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 318",
                        ToFsmState = fsm.Fsm.GetState("Combo 318")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState("Tele In");
            if (source != null)
                SethFsmController.CloneActions(source, state);

            state.Actions = state.Actions
            .Concat(new FsmStateAction[]
            {
                new EnableAlignYAction { offsetY = 1f, duration = 0.01f }
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