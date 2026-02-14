using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace SethPrime
{
    /// <summary>
    /// État global simple pour savoir si on est en P3 Custom
    /// Utilisé par SethAudioHandler
    /// </summary>
    public static class SethMusicState
    {
        public static bool IsInP3Custom = false;
        public static bool IsFightActive = false;
    }
}