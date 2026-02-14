using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo283 : StateModifierBase
    {
        public override string BindState => "Combo 283";

        public Combo283(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 284",
                        ToFsmState = fsm.Fsm.GetState("Combo 284")
                    }
                }
            };
         
            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            var sourceState = fsm.Fsm.GetState("Catch Air");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }
            ForceColliderOn(comboState);
            SethPrimeMain.Log.LogInfo("Etat Combo 283 créé FIN DE LA PARTIE 3 DU COMBO");
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
            RedirectFinishedToStun();
        }
        public override void SetupPhase5Modifiers()
        {
            RedirectFinishedToCombo284();
        }

        // -------------------------
        // HELPERS
        // -------------------------

        private void RedirectFinishedToStun()
        {
            RedirectFinishedTo("Stun Combo 0");
        }
        private void RedirectFinishedToCombo284()
        {
            RedirectFinishedTo("Combo 284");
        }

        private void RedirectFinishedTo(string targetStateName)
        {
            var state = fsm.Fsm.GetState(BindState);
            var target = fsm.Fsm.GetState(targetStateName);

            if (state == null || target == null)
            {
                SethPrimeMain.Log.LogWarning(
                    $"[Combo283] Redirect FAILED → {targetStateName}"
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
                $"[Combo283] FINISHED → {targetStateName}"
            );
        }
    }
}