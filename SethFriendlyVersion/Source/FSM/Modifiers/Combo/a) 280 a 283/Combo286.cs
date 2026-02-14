using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.CustomActions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo286 : StateModifierBase
    {
        public override string BindState => "Combo 286";

        private const string ThrowLState = "Combo 286 Throw L";
        private const string ThrowRState = "Combo 286 Throw R";

        public Combo286(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            CreateThrowState(ThrowLState, "Throw L");
            CreateThrowState(ThrowRState, "Throw R");

            CreateDecisionState();

            SethPrimeMain.Log.LogInfo("[Combo47] Etats décision + Throw créés");
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
                        ToState = "Combo 287",
                        ToFsmState = fsm.Fsm.GetState("Combo 287")
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