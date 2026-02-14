using HarmonyLib;
using UnityEngine;

namespace SethPrime
{
    [HarmonyPatch(typeof(AudioSource), "clip", MethodType.Setter)]
    public static class MusicLoggerPatch
    {
        static void Postfix(AudioSource __instance, AudioClip value)
        {
            if (value == null)
                return;

            if (__instance.loop)
            {
                //SethPrimeMain.Log.LogInfo($"[MUSIC LOGGER] Source={__instance.gameObject.name} Clip={value.name}");
            }
        }
    }
}