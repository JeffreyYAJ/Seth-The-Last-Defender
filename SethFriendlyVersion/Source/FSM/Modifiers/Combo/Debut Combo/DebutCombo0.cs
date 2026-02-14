using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class DebutCombo0 : StateModifierBase
    {
        public override string BindState => "Debut Combo 0";

        public DebutCombo0(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            var comboState = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Debut Combo 1",
                        ToFsmState = fsm.Fsm.GetState("Debut Combo 1")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            var source = fsm.Fsm.GetState("Stab Recover");
            if (source != null)
                SethFsmController.CloneActions(source, comboState);

            comboState.Actions = comboState.Actions
            .Concat(new FsmStateAction[]
            {
                new SetIsInComboAction(wrapper, false),
                new Wait() { time = 0.001f, finishEvent = FsmEvent.GetFsmEvent("FINISHED") },
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