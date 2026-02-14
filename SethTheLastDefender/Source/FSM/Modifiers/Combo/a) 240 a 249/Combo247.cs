using HutongGames.PlayMaker;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo247 : StateModifierBase
    {
        public override string BindState => "Combo 247";

        public Combo247(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Actions = new FsmStateAction[]
                {
                    new EnableAlignYAction
                    {
                        offsetY = 0f,
                        duration = 0.01f
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.Finished,
                        ToState = "Combo 248",
                        ToFsmState = fsm.Fsm.GetState("Combo 248")
                    }
                }
            };
            ForceColliderOn(state);
            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}
