using HutongGames.PlayMaker;
using SethPrime.CustomActions;
using System.Linq;

namespace SethPrime.FSM.Modifiers
{
    public class SlashComboEvadeP4 : StateModifierBase  // Gere le cas du follow up de l'evade
    {
        public override string BindState => "Cross Up Evade";

        private const string DecisionStateName = "P4 Slash Combo Cross Up Decision";

        // Les deux états qui peuvent maintenant déclencher le follow-up
        private readonly string[] entryStates = new[]
        {
            "Cross Up Evade",
            "Recover"

        };

        public SlashComboEvadeP4(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            // Si déjà présent, skip
            if (fsm.Fsm.GetState(DecisionStateName) != null)
                return;

            var dec = new FsmState(fsm.Fsm)
            {
                Name = DecisionStateName,
                Actions = new FsmStateAction[]
                {
                    new WeightedRandomEventAction()
                    {
                        events = new[]
                        {
                            FsmEvent.GetFsmEvent("Stab"),
                            FsmEvent.GetFsmEvent("CYCLONE"),
                            FsmEvent.GetFsmEvent("Combo"),
                            FsmEvent.GetFsmEvent("TP"),
                            FsmEvent.GetFsmEvent("GTHROW"),

                        },
                        weights = new float[] { 0.16f, 0.16f, 0.02f, 0.5f, 0.16f }
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("Stab"), ToState = "Stab Antic", ToFsmState = fsm.Fsm.GetState("Stab Antic") },
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("TP"),  ToState = "Tele Out", ToFsmState = fsm.Fsm.GetState("Tele Out") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("Combo"), ToState = "Debut Combo 1", ToFsmState = fsm.Fsm.GetState("Debut Combo 1") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("CYCLONE"), ToState = "Fade Slash Range", ToFsmState = fsm.Fsm.GetState("Fade Slash Range") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("GTHROW"), ToState = "GThrow Range", ToFsmState = fsm.Fsm.GetState("GThrow Range") },
                }
            };

            var states = fsm.Fsm.States.ToList();
            states.Add(dec);
            fsm.Fsm.States = states.ToArray();
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
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