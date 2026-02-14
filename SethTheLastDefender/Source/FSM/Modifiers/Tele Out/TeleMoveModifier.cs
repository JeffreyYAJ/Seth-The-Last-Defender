using HutongGames.PlayMaker;
using SethPrime.CustomActions;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SethPrime.FSM.Modifiers
{
    public class TeleMoveModifier : StateModifierBase
    {
        public override string BindState => "LOL";

        private const string DecisionStateName = "Tele Move Decision";

        // États qui déclenchent le follow-up
        private readonly string[] entryStates = new[] { "Tele Move" };

        public TeleMoveModifier(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                            FsmEvent.GetFsmEvent("Dive"),
                            FsmEvent.GetFsmEvent("Slash"),
                            FsmEvent.GetFsmEvent("ShieldA"),
                            FsmEvent.GetFsmEvent("ShieldG"),
                            FsmEvent.GetFsmEvent("Stab"),
                            FsmEvent.GetFsmEvent("Cyclone"),
                           
                        },
                        weights = new float[] { 0.25f, 0.15f, 0.15f, 0.15f , 0.15f , 0.15f }
                    }
                },
                Transitions = new[]
                {
                    new FsmTransition(){ FsmEvent = FsmEvent.GetFsmEvent("Dive"), ToState = "Tele Dive Pos", ToFsmState = fsm.Fsm.GetState("Tele Dive Pos") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("Slash"), ToState = "Slash Combo Pos", ToFsmState = fsm.Fsm.GetState("Slash Combo Pos") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("ShieldA"), ToState = "Tele Throw Pos", ToFsmState = fsm.Fsm.GetState("Tele Throw Pos") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("ShieldG"), ToState = "GShield TP 1", ToFsmState = fsm.Fsm.GetState("GShield TP 1") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("Stab"), ToState = "Stab TP 1", ToFsmState = fsm.Fsm.GetState("Stab TP 1") },
                    new FsmTransition() { FsmEvent = FsmEvent.GetFsmEvent("Cyclone"), ToState = "Cyclone TP 1", ToFsmState = fsm.Fsm.GetState("Cyclone TP 1") },
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