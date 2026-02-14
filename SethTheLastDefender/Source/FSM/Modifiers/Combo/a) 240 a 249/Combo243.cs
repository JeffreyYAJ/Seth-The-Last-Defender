using HutongGames.PlayMaker;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo243 : StateModifierBase
    {
        public override string BindState => "Combo 243";

        public Combo243(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 244",
                        ToFsmState = fsm.Fsm.GetState("Combo 244")
                    }
                }
            };
        
            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState("Tele Out");
            if (source != null)
                SethFsmController.CloneActions(source, state);
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}