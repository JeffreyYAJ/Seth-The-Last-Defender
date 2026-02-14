using HutongGames.PlayMaker;

namespace SethPrime.FSM.Actions
{
    public class EnablePhase4ExplosionsAction : FsmStateAction
    {
        public override void OnEnter()
        {
            PlayerImpactService.EnablePhase4Explosions();
            Finish();
        }
    }
}