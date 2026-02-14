using BepInEx.Configuration;

namespace SethPrime
{
    public static class DamageChanger
    {
        public static ConfigEntry<int> BodyDamage;
        public static ConfigEntry<int> ShieldProjectileDamage;
        public static ConfigEntry<int> ExplosionDamage;
        public static ConfigEntry<int> DiveDamage;
        public static ConfigEntry<int> ShieldColliderDamage;

        public static void Init(BepInEx.BaseUnityPlugin plugin)
        {
            BodyDamage = plugin.Config.Bind(
                "Damage",
                "BodyDamage",
                2,
                "Dégâts infligés par le corps du boss"
            );

            ShieldProjectileDamage = plugin.Config.Bind(
                "Damage",
                "ShieldProjectileDamage",
                2,
                "Dégâts du projectile de bouclier"
            );

            ExplosionDamage = plugin.Config.Bind(
                "Damage",
                "ExplosionDamage",
                3,
                "Dégâts de l'explosion 'damager'"
            );

            DiveDamage = plugin.Config.Bind(
                "Damage",
                "DiveDamage",
                2,
                "Dégâts du Dive Damager (plonge)"
            );
            ShieldColliderDamage = plugin.Config.Bind(
                "Damage",
                "ShieldCollider",
                2,
                "Dégâts du contact avec bouclier"
            );
        }
    }
}