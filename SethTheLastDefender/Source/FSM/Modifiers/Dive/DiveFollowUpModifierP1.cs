using System.Linq;
using HutongGames.PlayMaker;
using SethPrime.CustomActions;

namespace SethPrime.FSM.Modifiers
{
    public class DiveFollowUpModifierP1 : StateModifierBase
    {
        public override string BindState => "LOL";

        private const string DecisionStateName = "P1 Dive Follow Up Decision";

        // États qui déclenchent le follow-up
        private readonly string[] entryStates = new[] { "Dive Land" };

        public DiveFollowUpModifierP1(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                            FsmEvent.GetFsmEvent("FOLLOW_STAB"),
                            FsmEvent.GetFsmEvent("FOLLOW_SLASH"),
                            FsmEvent.GetFsmEvent("Combo"),
                            FsmEvent.GetFsmEvent("FOLLOW_GTHROW"),
                            FsmEvent.GetFsmEvent("TP"),

                        },
                        weights = new float[] { 0.25f, 0.26f, 0.03f , 0.26f, 0.2f}

                    }
                },
                Transitions = new[]
                {
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("FOLLOW_STAB"), ToState = "Stab Antic", ToFsmState = fsm.Fsm.GetState("Stab Antic") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("FOLLOW_SLASH"), ToState = "Slash Range", ToFsmState = fsm.Fsm.GetState("Slash Range") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("Combo"), ToState = "Debut Combo 1", ToFsmState = fsm.Fsm.GetState("Debut Combo 1") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("FOLLOW_GTHROW"), ToState = "GThrow Range", ToFsmState = fsm.Fsm.GetState("GThrow Range") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("TP"), ToState = "Tele Out", ToFsmState = fsm.Fsm.GetState("Tele Out") },

                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(dec).ToArray();
        }


        public override void SetupPhase1Modifiers() => ApplyEntryStateRedirections();
        public override void SetupPhase2Modifiers() => ApplyEntryStateRedirections();


        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
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