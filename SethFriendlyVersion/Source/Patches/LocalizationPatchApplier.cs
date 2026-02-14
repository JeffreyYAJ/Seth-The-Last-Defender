using HarmonyLib;
using System.Reflection;
using TeamCherry.Localization;

namespace SethPrime.Patches
{
    public static class LocalizationPatchApplier
    {
        public static void Apply()
        {
            try
            {
                var harmony = new Harmony("com.sethprime.localization");

                // Récupère la méthode Language.Get(string, string)
                MethodInfo target = typeof(Language).GetMethod(
                    "Get",
                    new[] { typeof(string), typeof(string) }
                );

                // Récupère ta méthode postfix
                MethodInfo postfix = typeof(LocalizationPatch).GetMethod(
                    "Postfix",
                    BindingFlags.Public | BindingFlags.Static
                );

                // Patch uniquement cette méthode
                harmony.Patch(target, postfix: new HarmonyMethod(postfix));

                SethPrimeMain.Log.LogInfo("[SethWrapper] LocalizationPatch appliqué !");
            }
            catch (System.Exception ex)
            {
                SethPrimeMain.Log.LogError($"[SethWrapper] Erreur lors du patch de la localisation : {ex}");
            }
        }
    }
}