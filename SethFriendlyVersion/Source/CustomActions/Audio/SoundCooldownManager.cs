using UnityEngine;
using System.Collections.Generic;

namespace SethPrime
{
    public static class SoundCooldownManager
    {
        private static float lastPlayTime = -999f;
        private const float Cooldown = 2f;

        private static readonly string[] filteredPrefixes =
        {
            "SB_attack_",
            "SB_attack_large_",
            "SB_evade_"
        };

        public static bool CanPlay(AudioClip clip)
        {
            if (clip == null) return true;

            string name = clip.name;

            bool isFiltered = false;
            foreach (var p in filteredPrefixes)
            {
                if (name.StartsWith(p))
                {
                    isFiltered = true;
                    break;
                }
            }

            if (!isFiltered)
                return true;

            if (Time.time - lastPlayTime < Cooldown)
                return false;

            lastPlayTime = Time.time;
            return true;
        }
    }
}