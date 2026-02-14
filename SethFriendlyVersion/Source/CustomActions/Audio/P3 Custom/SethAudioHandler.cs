using HarmonyLib;
using UnityEngine;

namespace SethPrime
{
    [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.clip), MethodType.Setter)]
    public static class SethAudioHandler
    {
        private static AudioSource bossMusicSource;
        public static AudioSource BossMusicSource => bossMusicSource;


        [HarmonyPostfix]
        public static void OnClipSet(AudioSource __instance, AudioClip value)
        {
            if (__instance == null || value == null)
                return;

            // 🎵 Détection UNIQUE de la musique de boss
            if (value.name == "Petals v2")
            {
                bossMusicSource = __instance;
                SethPrimeMain.Log?.LogInfo(
                    "[SethAudioHandler] Source musique boss détectée (Petals v2)"
                );
            }

            // Si on n'est pas en P3 custom → rien
            if (!SethMusicState.IsInP3Custom)
                return;

            TryReplaceBossMusic();
        }

        public static void TryReplaceBossMusic()
        {
            if (!SethMusicState.IsInP3Custom)
                return;

            if (bossMusicSource == null)
            {
                SethPrimeMain.Log?.LogWarning(
                    "[SethAudioHandler] bossMusicSource NULL"
                );
                return;
            }

            var main = SethPrimeMain.Instance;
            if (main?.Phase3CustomMusic == null)
                return;

            if (bossMusicSource.clip == main.Phase3CustomMusic)
                return;

            SethPrimeMain.Log?.LogInfo(
                "[SethAudioHandler] Remplacement musique boss → P3 Custom"
            );

            bossMusicSource.clip = main.Phase3CustomMusic;
            bossMusicSource.time = 0f;

            bossMusicSource.volume = 1f; // Volume de base a ajusté en fonction de la musique
            // 🔹 Réglage du pitch
            bossMusicSource.pitch = 1f; // 1f = normal
            bossMusicSource.Play();
        }
    }
}