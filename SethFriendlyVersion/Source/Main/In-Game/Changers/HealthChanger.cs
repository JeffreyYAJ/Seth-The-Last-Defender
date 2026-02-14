using UnityEngine;

namespace SethPrime
{
    public static class HealthChanger
    {
        public static void Initialize(HealthManager hm, int targetHp)
        {
            if (hm == null) return;

            int old = hm.hp;
            hm.hp = targetHp;

            var field = typeof(HealthManager).GetField("initHp");
            if (field != null)
                field.SetValue(hm, targetHp);

            SethPrimeMain.Log.LogInfo($"[SethPrime][Wrapper] HP modifié : {old} → {targetHp}");
        }
    }
}