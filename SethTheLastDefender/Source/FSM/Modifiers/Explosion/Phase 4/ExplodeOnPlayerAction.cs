using HutongGames.PlayMaker;

namespace SethPrime.FSM.Actions
{
    public class ExplodeOnPlayerAction : FsmStateAction
    {
        private float _delay;

        public ExplodeOnPlayerAction(float delay = 0.15f)
        {
            _delay = delay;
        }

        public override void OnEnter()
        {
            SethPrimeMain.Log.LogInfo(
                $"[ExplodeOnPlayerAction] Trigger explosion joueur (delay={_delay})"
            );

            PlayerImpactService.ExplodeOnPlayer(_delay, 1f);
            Finish();
        }
    }
}