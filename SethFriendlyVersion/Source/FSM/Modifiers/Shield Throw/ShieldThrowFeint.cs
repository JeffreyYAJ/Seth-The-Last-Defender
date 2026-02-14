using System.Linq;
using HutongGames.PlayMaker;
using SethPrime.CustomActions;

namespace SethPrime.FSM.Modifiers
{
    public class ShieldThrowFeint : StateModifierBase
    {
        public override string BindState => "LOL";

        private const string DecisionStateName = "Shield Throw Feint Decision";

        // États qui déclenchent le follow-up
        private readonly string[] entryStates = new[] { "Throw Antic" };

        public ShieldThrowFeint(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            // Si déjà présent, skip
            if (fsm.Fsm.GetState(DecisionStateName) != null)
                return;

            // ----- 1) Création de l'état décisionnel -----
            var dec = new FsmState(fsm.Fsm)
            {
                Name = DecisionStateName,
                Actions = new FsmStateAction[]
                {
                    new WeightedRandomEventAction()
                    {
                        events = new[]
                        {
                            FsmEvent.GetFsmEvent("NOTHING"),
                            FsmEvent.GetFsmEvent("TP"),
                        },
                        weights = new float[] { 0.85f, 0.15f }
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("NOTHING"), ToState="Throw Shield", ToFsmState=fsm.Fsm.GetState("Throw Shield") },
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("TP"), ToState="Tele Out", ToFsmState=fsm.Fsm.GetState("Tele Out") },
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(dec).ToArray();
        }


        public override void SetupPhase1Modifiers()  { }
        public override void SetupPhase2Modifiers()  { }
        public override void SetupPhase3Modifiers() => ApplyEntryStateRedirections();
        public override void SetupPhase4Modifiers() => ApplyEntryStateRedirections();
        public override void SetupPhase5Modifiers() => ApplyEntryStateRedirections();

        private void ApplyEntryStateRedirections()
        {
            var decision = fsm.Fsm.GetState(DecisionStateName);
            if (decision == null)
            {
                OnCreateModifier();
                decision = fsm.Fsm.GetState(DecisionStateName);
                if (decision == null)
                    return;
            }

            foreach (var stateName in entryStates)
            {
                var st = fsm.Fsm.GetState(stateName);
                if (st == null)
                    continue;

                st.Transitions = new[]
                {
                    new FsmTransition()
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = DecisionStateName,
                        ToFsmState = decision
                    }
                };
            }
        }
    }
}













