using HutongGames.PlayMaker;
using SethPrime;

public class SetFightActiveAction : FsmStateAction
{
    public bool Value;

    public override void OnEnter()
    {
        SethMusicState.IsFightActive = Value;
        SethPrimeMain.Log?.LogInfo(
            $"[SetFightActiveAction] IsFightActive = {Value}"
        );
        Finish();
    }
}