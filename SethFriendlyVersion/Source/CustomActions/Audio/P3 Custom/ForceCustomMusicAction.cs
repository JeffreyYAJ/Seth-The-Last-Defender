using HutongGames.PlayMaker;
using UnityEngine;

namespace SethPrime
{
    /// <summary>
    /// Action PlayMaker pour forcer la lecture de la musique P3 Custom.
    /// </summary>
    public class ForceCustomMusicAction : FsmStateAction
    {
        public override void OnEnter()
        {
            SethPrimeMain.Log?.LogInfo("[ForceCustomMusicAction] Tentative de forçage musique P3 Custom");
            SethAudioHandler.TryReplaceBossMusic();
            Finish();
        }
    }
}