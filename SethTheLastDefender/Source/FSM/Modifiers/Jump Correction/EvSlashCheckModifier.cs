using HutongGames.PlayMaker;
using SethPrime.CustomActions;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.ExpressionEvaluator;

namespace SethPrime.FSM.Modifiers
{
    public class EvSlashCheckModifier : StateModifierBase
    {
        public override string BindState => "LOL";

        private const string DecisionStateName = "Ev Correction";

        // États qui déclenchent le follow-up
        private readonly string[] entryStates = new[] { "Ev Slash Check", "Ev Cross Up", "Hop Slash Check" };

        public EvSlashCheckModifier(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {

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
                            FsmEvent.GetFsmEvent("Nothing"),
                        },
                        weights = new float[] { 1f }
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("Nothing"), ToState = "Tele Out", ToFsmState = fsm.Fsm.GetState("Tele Out") },
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(dec).ToArray();
        }


        public override void SetupPhase1Modifiers() => ApplyEntryStateRedirections();
        public override void SetupPhase2Modifiers() => ApplyEntryStateRedirections();


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
