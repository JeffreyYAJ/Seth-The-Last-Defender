using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.CustomActions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo115 : StateModifierBase
    {
        public override string BindState => "Combo 115";

        private const string SlashLState = "Combo 115 Slash L";
        private const string SlashRState = "Combo 115 Slash R";

        public Combo115(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            CreateSlashState(SlashLState, "Slash L");
            CreateSlashState(SlashRState, "Slash R");

            CreateDecisionState();

            SethPrimeMain.Log.LogInfo("[Combo115] Etats décision + Slash créés");
        }

        private void CreateDecisionState()
        {
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
                        ToState = SlashLState,
                        ToFsmState = fsm.Fsm.GetState(SlashLState)
                    },
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("SLASH_R"),
                        ToState = SlashRState,
                        ToFsmState = fsm.Fsm.GetState(SlashRState)
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(decision).ToArray();
        }

        private void CreateSlashState(string stateName, string sourceStateName)
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = stateName,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Combo 116",
                        ToFsmState = fsm.Fsm.GetState("Combo 116")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState(sourceStateName);
            if (source != null)
            {
                SethFsmController.CloneActions(source, state);
            }
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}