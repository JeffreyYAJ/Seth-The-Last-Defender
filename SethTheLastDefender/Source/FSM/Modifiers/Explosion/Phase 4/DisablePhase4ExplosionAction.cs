using HutongGames.PlayMaker;

namespace SethPrime.FSM.Actions
{
    public class DisablePhase4ExplosionsAction : FsmStateAction
    {
        public override void OnEnter()
        {
            PlayerImpactService.DisablePhase4Explosions();
            Finish();
        }
    }
}