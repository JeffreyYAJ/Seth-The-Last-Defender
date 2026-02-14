using HutongGames.PlayMaker;
using UnityEngine;

namespace SethPrime
{
    /// <summary>
    /// Met en pause uniquement la musique du boss Seth.
    /// </summary>
    public class PauseBossMusicAction : FsmStateAction
    {
        public bool Pause = true;

        public override void OnEnter()
        {
            var src = SethAudioHandler.BossMusicSource;
            if (src == null)
            {
                SethPrimeMain.Log?.LogWarning("[PauseBossMusicAction] BossMusicSource est null !");
                Finish();
                return;
            }

            if (Pause)
            {
                if (src.isPlaying)
                    src.Pause();
            }
            else
            {
                src.UnPause();
            }

            Finish();
        }
    }
}