using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace SethPrime
{
    public static class AudioLogger
    {
        private static bool initialized = false;

        public static void Initialize()
        {
            if (initialized) return;
            initialized = true;

            SethPrimeMain.Log.LogInfo("[AudioLogger] Initialisation...");

            var harmony = new Harmony("com.sethprime.audiologger");

            // --- Patch PlayOneShot(AudioClip) ---
            var playOneShot = typeof(AudioSource).GetMethod(
                nameof(AudioSource.PlayOneShot),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new[] { typeof(AudioClip) },
                null
            );

            if (playOneShot != null)
            {
                harmony.Patch(
                    playOneShot,
                    prefix: new HarmonyMethod(typeof(AudioLogger).GetMethod(nameof(OnPlayOneShot)))
                );
                //SethPrimeMain.Log.LogInfo("[AudioLogger] Patch → PlayOneShot");
            }

            // --- Patch PlayScheduled(double) ---
            var playScheduled = typeof(AudioSource).GetMethod(
                nameof(AudioSource.PlayScheduled),
                BindingFlags.Instance | BindingFlags.Public
            );

            if (playScheduled != null)
            {
                harmony.Patch(
                    playScheduled,
                    prefix: new HarmonyMethod(typeof(AudioLogger).GetMethod(nameof(OnPlayScheduled)))
                );
                //SethPrimeMain.Log.LogInfo("[AudioLogger] Patch → PlayScheduled");
            }

            //SethPrimeMain.Log.LogInfo("[SethPrime] AudioLogger initialisé.");
        }

        // =========================================================
        // PLAY ONE SHOT
        // =========================================================
        public static bool OnPlayOneShot(AudioSource __instance, AudioClip clip)
        {
            if (clip == null)
                return true;

            if (!SoundCooldownManager.CanPlay(clip))
            {
                // 🔇 son bloqué par cooldown
                //SethPrimeMain.Log.LogInfo($"[AUDIO BLOCKED] {clip.name}");
                return false;
            }

            //SethPrimeMain.Log.LogInfo($"[AUDIO OK] {clip.name}");
            return true;
        }

        // =========================================================
        // PLAY SCHEDULED
        // =========================================================
        public static bool OnPlayScheduled(AudioSource __instance, double time)
        {
            if (__instance.clip == null)
                return true;

            if (!SoundCooldownManager.CanPlay(__instance.clip))
            {
                //SethPrimeMain.Log.LogInfo($"[AUDIO BLOCKED] {__instance.clip.name}");
                return false;
            }

            return true;
        }
    }
}