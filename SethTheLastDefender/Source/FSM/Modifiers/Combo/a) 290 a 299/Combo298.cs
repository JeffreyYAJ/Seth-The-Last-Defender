using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo298 : StateModifierBase
    {
        public override string BindState => "Combo 298";

        public Combo298(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 299",
                        ToFsmState = fsm.Fsm.GetState("Combo 299")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();


            state.Actions = state.Actions
            .Concat(new FsmStateAction[]
            {
                new ExplodeOnPlayerAction(0.2f),
                new Wait { time = 0.3f,finishEvent = FsmEvent.GetFsmEvent("FINISHED")}
            })
            .ToArray();
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}