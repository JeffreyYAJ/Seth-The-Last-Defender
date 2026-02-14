using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo348 : StateModifierBase
    {
        public override string BindState => "Combo 348";

        public Combo348(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Stun Combo 0",
                        ToFsmState = fsm.Fsm.GetState("Stun Combo 0")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState("Fade Slash Antic");
            if (source != null)
                SethFsmController.CloneActions(source, state);

            state.Actions = state.Actions
            .Concat(new FsmStateAction[]
            {
                    new PlayClipAction(){Clip = wrapper.SethHitAudio,Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value},
                    new EnablePhase4ExplosionsAction(), // 👈 ICI
                    new Wait { time = 0.5f,finishEvent = FsmEvent.GetFsmEvent("FINISHED")}
            })
            .ToArray();
            ForceColliderOn(state);

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
            RedirectFinishedToStun();
        }
        // -------------------------
        // HELPERS
        // -------------------------

        private void RedirectFinishedToStun()
        {
            RedirectFinishedTo("Stun Combo 0");
        }


        private void RedirectFinishedTo(string targetStateName)
        {
            var state = fsm.Fsm.GetState(BindState);
            var target = fsm.Fsm.GetState(targetStateName);

            if (state == null || target == null)
            {
                SethPrimeMain.Log.LogWarning(
                    $"[Combo348] Redirect FAILED → {targetStateName}"
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
                $"[Combo348] FINISHED → {targetStateName}"
            );
        }
    }
}
