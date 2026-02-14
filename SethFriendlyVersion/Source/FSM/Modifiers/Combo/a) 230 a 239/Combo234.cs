using HutongGames.PlayMaker;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo234 : StateModifierBase
    {
        public override string BindState => "Combo 234";

        public Combo234(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 235",
                        ToFsmState = fsm.Fsm.GetState("Combo 235")
                    }
                }
            };
       
            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState("Stab Recover");
            if (source != null)
                SethFsmController.CloneActions(source, state);

            ForceColliderOn(state);
        }
        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}