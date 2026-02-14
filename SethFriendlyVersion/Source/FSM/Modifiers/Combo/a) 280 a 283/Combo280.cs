using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo280 : StateModifierBase
    {
        public override string BindState => "Combo 280";

        public Combo280(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Actions = new FsmStateAction[]
                {
                    new Wait { time = 0.1f }
                },
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.Finished,
                        ToState = "Combo 281",
                        ToFsmState = fsm.Fsm.GetState("Combo 281")
                    }
                }
            };
    
            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}