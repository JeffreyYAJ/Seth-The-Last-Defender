using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo105 : StateModifierBase
    {
        public override string BindState => "Combo 105";

        public Combo105(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                    // transition par défaut (phase 3 / 4)
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Combo 106",
                        ToFsmState = fsm.Fsm.GetState("Combo 106")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            var sourceState = fsm.Fsm.GetState("Throw Antic");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }

            SethPrimeMain.Log.LogInfo("Etat Combo 105 créé FIN DE LA PARTIE 1 DU COMBO");
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
            RedirectFinishedToCombo106();
        }

        public override void SetupPhase4Modifiers()
        {
            RedirectFinishedToCombo106();
        }

        public override void SetupPhase5Modifiers()
        {
            RedirectFinishedToCombo106();
        }
        // -------------------------
        // HELPERS
        // -------------------------

        private void RedirectFinishedToStun()
        {
            RedirectFinishedTo("Stun Combo 0");
        }

        private void RedirectFinishedToCombo106()
        {
            RedirectFinishedTo("Combo 106");
        }

        private void RedirectFinishedTo(string targetStateName)
        {
            var state = fsm.Fsm.GetState(BindState);
            var target = fsm.Fsm.GetState(targetStateName);

            if (state == null || target == null)
            {
                SethPrimeMain.Log.LogWarning(
                    $"[Combo105] Redirect FAILED → {targetStateName}"
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
                $"[Combo105] FINISHED → {targetStateName}"
            );
        }
    }
}