using HutongGames.PlayMaker;
using UnityEngine;

namespace SethPrime
{
    /// <summary>
    /// Action PlayMaker pour définir le flag global IsInP3Custom.
    /// </summary>
    public class SetIsInP3CustomAction : FsmStateAction
    {
        public bool Value;

        public override void OnEnter()
        {
            SethMusicState.IsInP3Custom = Value;
            SethPrimeMain.Log?.LogInfo($"[SetIsInP3CustomAction] IsInP3Custom = {Value}");
            Finish();
        }
    }
}