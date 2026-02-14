using HutongGames.PlayMaker;
using SethPrime.CustomActions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo273 : StateModifierBase
    {
        public override string BindState => "Combo 273";

        private const string SlashL = "Combo 273 Slash L";
        private const string SlashR = "Combo 273 Slash R";

        public Combo273(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            CreateSlashState(SlashL, "Slash L");
            CreateSlashState(SlashR, "Slash R");

            var decision = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Actions = new FsmStateAction[]
                {
                    new WeightedRandomEventAction
                    {
                        events = new[]
                        {
                            FsmEvent.GetFsmEvent("SLASH_L"),
                            FsmEvent.GetFsmEvent("SLASH_R")
                        },
                        weights = new[] { 0.5f, 0.5f }
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("SLASH_L"),
                        ToState = SlashL,
                        ToFsmState = fsm.Fsm.GetState(SlashL)
                    },
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("SLASH_R"),
                        ToState = SlashR,
                        ToFsmState = fsm.Fsm.GetState(SlashR)
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(decision).ToArray();
        }

        private void CreateSlashState(string name, string sourceName)
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = name,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Combo 274",
                        ToFsmState = fsm.Fsm.GetState("Combo 274")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState(sourceName);
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