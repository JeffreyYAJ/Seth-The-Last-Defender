using HarmonyLib;
using UnityEngine;

namespace SethPrime.Patches
{
    [HarmonyPatch(typeof(AudioSource))]
    internal static class AudioSource_PlayOneShot_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch("PlayOneShot", new[] { typeof(AudioClip) })]
        private static void Prefix1(AudioSource __instance, AudioClip clip) => Handle(__instance, clip);

        [HarmonyPrefix]
        [HarmonyPatch("PlayOneShot", new[] { typeof(AudioClip), typeof(float) })]
        private static void Prefix2(AudioSource __instance, AudioClip clip, float volumeScale) => Handle(__instance, clip);

        private static void Handle(AudioSource src, AudioClip clip)
        {
            if (!clip || clip.name != "song_gate_hit")
                return;


            SethPrimeMain.Log.LogInfo($"[AUDIO PATCH OK] Clip={clip.name} Src={src.gameObject.name}");

            // 1️ On tente de récupérer SethWrapper depuis la hiérarchie
            var wrapper = src.GetComponentInParent<SethWrapper>();

            // 2️ Si pas trouvé, utiliser le contexte global (SethContext.Current)
            if (!wrapper && SethContext.Current != null)
                wrapper = SethContext.Current;

            // 3️ En dernier recours, chercher le premier SethWrapper actif dans la scène
            if (!wrapper)
                wrapper = Object.FindFirstObjectByType<SethWrapper>();

            if (!wrapper)
            {
                SethPrimeMain.Log.LogWarning("[GATE_HIT] No SethWrapper found anywhere!");
                return;
            }

            // 4️ Récupérer le FSM actif
            string fsm = wrapper.MainControlFsm?.Fsm?.ActiveStateName ?? "null";

            // 4.5 Ignorer Dive Land
            if (fsm.Contains("Dive Land"))
                return;

            // 5️ Déterminer la position de l'explosion
            Vector3 explosionPos;

            if (fsm.Contains("Block") || fsm.Contains("Fade Slash"))
            {
                // Explosion attachée à Seth, mais hauteur forcée
                explosionPos = wrapper.transform.position;
                explosionPos.y = 10f; // ✅ hauteur fixe pour toutes les explosions de bloc
            }
            else
            {
                // Bouclier lancé
                Transform shieldTransform = wrapper.transform.Find("Shield");
                explosionPos = shieldTransform ? shieldTransform.position : src.transform.position;
                explosionPos.y = 10f; // ✅ hauteur fixe pour explosion du bouclier
            }

            // 6️ Déterminer le type d'impact
            ImpactType impactType = (fsm.Contains("Block") || fsm.Contains("Fade Slash"))
                ? ImpactType.ShieldBlock
                : ImpactType.ShieldGroundBounce;

            // 7️ Jouer l'impact
            
            wrapper.StartCoroutine(DelayedExplosion(wrapper.gameObject, impactType, explosionPos));

            SethPrimeMain.Log.LogInfo(
                $"[GATE_HIT] Impact={impactType} FSM={fsm} ExplosionPos={explosionPos}"
            );
        }
        private static System.Collections.IEnumerator DelayedExplosion(GameObject wrapperGO, ImpactType impactType, Vector3 pos)
        {
            yield return null; // attend la frame suivante
            SethImpactService.PlayAtPosition(wrapperGO, impactType, pos, true, 0.4f);
        }
    }
}
