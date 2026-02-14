using System.Linq;
using HutongGames.PlayMaker;
using SethPrime.CustomActions;

namespace SethPrime.FSM.Modifiers
{
    public class GShieldThrowFollowUpModifierP3 : StateModifierBase
    {
        public override string BindState => "LOL";

        private const string DecisionStateName = "P3 GShield Throw Follow Up Decision";

        // États qui déclenchent le follow-up
        private readonly string[] entryStates = new[] { "Catch Ground" };

        public GShieldThrowFollowUpModifierP3(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                            FsmEvent.GetFsmEvent("Combo"),
                        },
                        weights = new float[] { 0.48f, 0.5f, 0.02f }
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("NOTHING"), ToState="Land", ToFsmState=fsm.Fsm.GetState("Land") },
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("TP"), ToState="Tele Out", ToFsmState=fsm.Fsm.GetState("Tele Out") },
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("Combo"), ToState="Debut Combo 1", ToFsmState=fsm.Fsm.GetState("Debut Combo 1") },
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(dec).ToArray();
        }


        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }


        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() => ApplyEntryStateRedirections();
        public override void SetupPhase5Modifiers() { }

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













