using System.Collections;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SethPrime.Patches
{
    [HarmonyPatch]
    public class BossTitlePatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(DisplayBossTitle), nameof(DisplayBossTitle.OnEnter))]
        private static void MakeBossTitleBigPatch(ref DisplayBossTitle __instance)
        {
            // On ne déclenche que dans la scène de Seth
            if (SceneManager.GetActiveScene().name != Constants.SethSceneName)
                return;

            // Lance la coroutine via le main mod
            SethPrimeMain.Instance.StartCoroutine(BossTitleRoutine(__instance));
        }

        private static IEnumerator BossTitleRoutine(DisplayBossTitle __instance)
        {
            // Récupération de l'AreaTitle (texte de zone, gros, centré)
            GameObject areaTitleObject = ManagerSingleton<AreaTitle>.Instance.gameObject;
            PlayMakerFSM areaTitleFsm = ActionHelpers.GetGameObjectFsm(areaTitleObject, "Area Title Control");

            // On redirige DisplayBossTitle vers l'AreaTitle
            __instance.areaTitleObject.Value = areaTitleObject;

            // Reset propre pour forcer l'affichage
            areaTitleObject.SetActive(false);

            areaTitleFsm.FsmVariables.FindFsmBool("Visited").Value = false;
            areaTitleFsm.FsmVariables.FindFsmBool("Display Right").Value =
                __instance.displayRight.Value;

            // TEXTE : pour l’instant on laisse le bossTitle vanilla
            // (on remplacera par Constants.SethBossTitleSuper ensuite)
            areaTitleFsm.FsmVariables.FindFsmString("Area Event").Value =
                Constants.SethBossTitleSuperKey;

            areaTitleFsm.FsmVariables.FindFsmBool("NPC Title").Value = false;

            areaTitleObject.SetActive(true);

            // Petit boost de vitesse d'anim (comme Karmelita)
            var mainAnimator = areaTitleObject.GetComponentInChildren<Animator>();
            if (mainAnimator != null)
            {
                mainAnimator.speed = 1.15f;
                var childAnimator = mainAnimator.GetComponentInChildren<Animator>();
                if (childAnimator != null)
                    childAnimator.speed = 1.15f;
            }

            // Temps d'affichage
            yield return new WaitForSeconds(2.5f);

            // Forcer la fin propre de l'AreaTitle
            var fsm = areaTitleObject.GetComponent<PlayMakerFSM>();
            fsm.SendEvent("FINISHED");
        }
    }
}