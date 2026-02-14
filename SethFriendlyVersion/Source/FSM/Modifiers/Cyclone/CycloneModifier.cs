using HutongGames.PlayMaker;
using SethPrime.CustomActions;
using System.Linq;

namespace SethPrime.FSM.Modifiers
{
    public class CycloneModifier : StateModifierBase  // Gere le cas du follow up au sommet du Cyclone
    {
        public override string BindState => "Fade Slash 1";

        private const string DecisionStateName = "P1 Cyclone Top Decision";

        // Les deux états qui peuvent maintenant déclencher le follow-up
        private readonly string[] entryStates = new[]
        {
                "Fade Slash Combo",   // normal case
                "Cyclone D Strike"    // hit case
        };

        public CycloneModifier(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        events = new []
                        {
                            FsmEvent.GetFsmEvent("TP"),
                            FsmEvent.GetFsmEvent("ATHROW"),
                            FsmEvent.GetFsmEvent("DIVE"),
                        },
                        weights = new [] { 0.2f, 0.4f, 0.4f }
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("DIVE"), ToState = "Jump Dive Dir",ToFsmState = fsm.Fsm.GetState("Jump Dive Dir") },
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("ATHROW"), ToState = "Throw Dir", ToFsmState = fsm.Fsm.GetState("Throw Dir") },
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("TP"), ToState = "Tele Out", ToFsmState = fsm.Fsm.GetState("Tele Out") }
                }
            };

            var states = fsm.Fsm.States.ToList();
            states.Add(dec);
            fsm.Fsm.States = states.ToArray();
        }

        public override void SetupPhase1Modifiers()
        {
            ApplyEntryStateRedirections();
        }
        public override void SetupPhase2Modifiers()
        {
            ApplyEntryStateRedirections();
        }

        public override void SetupPhase3Modifiers() { }


        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }


        /// <summary>
        /// Redirige les états d’entrée vers l’état décisionnel
        /// pour les phases 3 & 4.
        /// </summary>
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

