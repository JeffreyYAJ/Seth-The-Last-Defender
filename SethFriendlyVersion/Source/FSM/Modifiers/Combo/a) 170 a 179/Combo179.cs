using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo179 : StateModifierBase
    {
        public override string BindState => "Combo 179";

        public Combo179(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            if (fsm.Fsm.GetState(BindState) != null)
                return;
            var comboState = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Combo 180",
                        ToFsmState = fsm.Fsm.GetState("Combo 180")
                    }
                }
            };
         
            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            var sourceState = fsm.Fsm.GetState("Throw Antic");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }
            ForceColliderOn(comboState);
            SethPrimeMain.Log.LogInfo("Etat Combo 179 Fin du Combo Partie 2 créé");
        }

        // -------------------------
        // PHASE LOGIC
        // -------------------------

        public override void SetupPhase1Modifiers()
        {
            RedirectFinishedToStun();
        }

        public override void SetupPhase2Modifiers()
        {
            RedirectFinishedToStun();
        }

        public override void SetupPhase3Modifiers()
        {
            RedirectFinishedToStun();
        }

        public override void SetupPhase4Modifiers()
        {
            RedirectFinishedToCombo180();
        }
        public override void SetupPhase5Modifiers()
        {
            RedirectFinishedToCombo180();
        }
        // -------------------------
        // HELPERS
        // -------------------------

        private void RedirectFinishedToStun()
        {
            RedirectFinishedTo("Stun Combo 0");
        }

        private void RedirectFinishedToCombo180()
        {
            RedirectFinishedTo("Combo 180");
        }

        private void RedirectFinishedTo(string targetStateName)
        {
            var state = fsm.Fsm.GetState(BindState);
            var target = fsm.Fsm.GetState(targetStateName);

            if (state == null || target == null)
            {
                SethPrimeMain.Log.LogWarning(
                    $"[Combo179] Redirect FAILED → {targetStateName}"
                );
                return;
            }

            state.Transitions = new[]
            {
                new FsmTransition
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = targetStateName,
                    ToFsmState = target
                }
            };

            SethPrimeMain.Log.LogInfo(
                $"[Combo179] FINISHED → {targetStateName}"
            );
        }
    }
}