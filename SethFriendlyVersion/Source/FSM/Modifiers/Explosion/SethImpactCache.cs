using UnityEngine;
using System.Linq;

namespace SethPrime
{
    public static class SethImpactCache
    {
        public static bool Ready { get; private set; }

        public static GameObject StompBlastGO;
        public static CameraShakeProfile EnemyKillProfile;


        public static void Init(GameObject sethRoot)
        {
            if (sethRoot == null) return;

            // Toujours chercher StompBlastGO même si Ready
            var t = sethRoot.transform.Find("Stomp Blast");
            if (t != null) StompBlastGO = t.gameObject;

            EnemyKillProfile = Resources.FindObjectsOfTypeAll<CameraShakeProfile>()
                .FirstOrDefault(p => p && p.name == "Enemy Kill");

            Ready = StompBlastGO != null;

            SethPrimeMain.Log.LogInfo(
                $"[ImpactCache] Ready={Ready} " +
                $"Stomp={(StompBlastGO ? "OK" : "NULL")} " +
                $"Shake={(EnemyKillProfile ? "OK" : "NULL")}"
            );
        }
    }
}