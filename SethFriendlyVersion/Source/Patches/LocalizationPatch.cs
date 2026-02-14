using System;
using System.Collections.Generic;
using TeamCherry.Localization;

namespace SethPrime.Patches
{
    public static class LocalizationPatch
    {
        private static readonly Dictionary<string, string> SethTitles = new Dictionary<string, string>
        {
            { "SETH_BC_SUPER_SUPER_MAIN", Constants.SethDisplayName },       // titre du milieu (DisplayName)
            { "SETH_BC_SUPER_SUPER_SUB", Constants.SethBossTitleSub },      // titre du bas
            { "SETH_BC_SUPER_SUPER_SUPER", Constants.SethBossTitleSuper }   // titre du haut
        };

        public static void Postfix(string key, string sheetTitle, ref string __result)
        {
            try
            {
                //SethPrimeMain.Log.LogInfo($"[LocalizationPatch] key={key}, sheet={sheetTitle}, result={__result}");

                if (SethTitles.TryGetValue(key, out string value))
                {
                    __result = value;
                }
            }
            catch
            {          
                //SethPrimeMain.Log.LogError($"[LocalizationPatch] Exception : {ex}");
            }
        }
    }
}