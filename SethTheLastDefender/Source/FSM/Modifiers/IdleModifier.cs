using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace SethPrime.FSM.Modifiers
{
    public class IdleModifier : StateModifierBase
    {
        private FsmState idleState;
        private Wait idleWaitAction;

        public override string BindState => "Idle";

        public IdleModifier(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            // Récupérer l'état "Idle" depuis l'instance FSM (champ 'fsm' hérité)
            idleState = fsm.Fsm.GetState("Idle");



            // Chercher l'action Wait dans l'état Idle
            foreach (var action in idleState.Actions)
            {
                if (action is Wait wait)
                {
                    idleWaitAction = wait;

                    break;
                }
            }

        }

        public override void SetupPhase1Modifiers()
        {
            if (idleWaitAction == null) return;
            idleWaitAction.time.Value = 0.20f;

        }

        public override void SetupPhase2Modifiers()
        {
            if (idleWaitAction == null) return;
            idleWaitAction.time.Value = 0.20f;

        }

        public override void SetupPhase3Modifiers()
        {
            if (idleWaitAction == null) return;
            idleWaitAction.time.Value = 0.2f;

        }

        public override void SetupPhase4Modifiers()
        {
            if (idleWaitAction == null) return;
            idleWaitAction.time.Value = 0.10f;

        }
        public override void SetupPhase5Modifiers()
        {
            if (idleWaitAction == null) return;
            idleWaitAction.time.Value = 0.10f;

        }
    }
}