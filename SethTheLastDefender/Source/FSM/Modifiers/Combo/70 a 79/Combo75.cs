using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.CustomActions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo75 : StateModifierBase
    {
        public override string BindState => "Combo 75";

        private const string SlashLState = "Combo 75 Slash L";
        private const string SlashRState = "Combo 75 Slash R";

        public Combo75(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            CreateSlashState(SlashLState, "Slash L");
            CreateSlashState(SlashRState, "Slash R");

            CreateDecisionState();

            SethPrimeMain.Log.LogInfo("[Combo47] Etats décision + Slash créés");
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
                        ToState = "Combo 76",
                        ToFsmState = fsm.Fsm.GetState("Combo 76")
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