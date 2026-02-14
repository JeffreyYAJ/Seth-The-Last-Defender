using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SethPrime.Patches
{
    [HarmonyPatch]
    public static class AnimationPatch
    {
        private static SethWrapper wrapper;

        private static SethWrapper Wrapper
        {
            get
            {
                if (wrapper != null) return wrapper;

                var seth = GameObject.Find("Seth");
                if (seth == null) return null;

                wrapper = seth.GetComponent<SethWrapper>();
                return wrapper;
            }
        }

        // ===============================
        // PATCH Play()
        // ===============================
        [HarmonyPrefix]
        [HarmonyPatch(typeof(tk2dSpriteAnimator), nameof(tk2dSpriteAnimator.Play),
            new[] { typeof(tk2dSpriteAnimationClip), typeof(float), typeof(float) })]
        private static void OverrideAnimationStatsPatch(
            tk2dSpriteAnimator __instance,
            ref tk2dSpriteAnimationClip clip,
            ref float clipStartTime,
            ref float overrideFps)
        {
            if (__instance == null || clip == null) return;
            if (__instance.gameObject.name != "Seth") return;

            var w = Wrapper;
            if (w == null) return;

            // 🔥 IMPORTANT
            clipStartTime = w.GetAnimationStartTime();
            overrideFps = clip.fps * w.GetAnimationSpeedModifier(clip.name);
        }

        // ===============================
        // PATCH IsPlaying()
        // ===============================
        [HarmonyPostfix]
        [HarmonyPatch(typeof(tk2dSpriteAnimator), nameof(tk2dSpriteAnimator.IsPlaying),
            new[] { typeof(tk2dSpriteAnimationClip) })]
        private static void AllowSameAnimationPlayPatch(
            tk2dSpriteAnimator __instance,
            ref bool __result)
        {
            if (__instance == null) return;
            if (__instance.gameObject.name != "Seth") return;

            // 🔥 AUTORISE LE REPLAY
            __result = false;
        }
    }
}