using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.CustomActions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class GShieldTP2 : StateModifierBase
    {
        public override string BindState => "GShield TP 2";

        private const string ThrowLState = "GShield T 2 Throw L";
        private const string ThrowRState = "GShield TP 2 Throw R";

        public GShieldTP2(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            CreateThrowState(ThrowLState, "Throw L");
            CreateThrowState(ThrowRState, "Throw R");

            CreateDecisionState();

            SethPrimeMain.Log.LogInfo("Etats GShield TP 2 décision + Throw créés");
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
                            FsmEvent.GetFsmEvent("Throw_L"),
                            FsmEvent.GetFsmEvent("Throw_R")
                        },
                        weights = new[] { 0.5f, 0.5f }
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("Throw_L"),
                        ToState = ThrowLState,
                        ToFsmState = fsm.Fsm.GetState(ThrowLState)
                    },
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("Throw_R"),
                        ToState = ThrowRState,
                        ToFsmState = fsm.Fsm.GetState(ThrowRState)
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(decision).ToArray();
        }

        private void CreateThrowState(string stateName, string sourceStateName)
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = stateName,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "GShield TP 3",
                        ToFsmState = fsm.Fsm.GetState("GShield TP 3")
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