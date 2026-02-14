using HarmonyLib;
using UnityEngine;

namespace SethPrime.Patches
{
    [HarmonyPatch(typeof(DamageHero), "OnTriggerEnter2D")]
    public static class SethDamageOverride
    {
        [HarmonyPrefix]
        public static void Prefix(DamageHero __instance, Collider2D collision)
        {
            if (__instance == null)
                return;

            string src = __instance.name;
            if (string.IsNullOrEmpty(src))
                return;

            // Corps
            if (src.Contains("Body Damager"))
            {
                __instance.damageDealt = DamageChanger.BodyDamage.Value;
            }
            // Projectile bouclier
            else if (src.Contains("Shield Projectile"))
            {
                __instance.damageDealt = DamageChanger.ShieldProjectileDamage.Value;
            }
            // Explosion
            else if (src == "damager" || src.Contains("damager"))
            {
                __instance.damageDealt = DamageChanger.ExplosionDamage.Value;
            }
            // Dive
            else if (src.Contains("Dive Damager"))
            {
                if (DamageChanger.DiveDamage != null)
                {
                    __instance.damageDealt = DamageChanger.DiveDamage.Value;
                }
            }
            // Shield Collider (Dive / Rush)
            else if (src.Contains("Shield Collider"))
            {
                __instance.damageDealt = DamageChanger.ShieldColliderDamage.Value;
            }
        }
    }
}