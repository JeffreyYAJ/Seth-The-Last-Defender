using System.Collections.Generic;
using TeamCherry.Localization;

namespace SethPrime
{
    public static class Constants
    {
        #region Seth Localization

        public static string SethSceneName => "Shellwood_22";

        // 🔑 KEYS (obligatoires)
        public static string SethBossTitleMainKey => "SETH_BC_SUPER_MAIN";
        public static string SethBossTitleSuperKey => "SETH_BC_SUPER_SUPER";
        public static string SethBossTitleSubKey => "SETH_BC_SUPER_SUB";
        public static string SethBossTitleMain => SethDisplayName;

        // 🔤 VALEURS LOCALISÉES
        public static string SethDisplayName
        {
            get
            {
                return Language.CurrentLanguage() switch
                {
                    LanguageCode.FR => "Seth",
                    LanguageCode.EN => "Seth",
                    LanguageCode.JA => "Seth",
                    LanguageCode.ES => "Seth",
                    LanguageCode.IT => "Seth",
                    LanguageCode.DE => "Seth",
                    _ => "Seth"
                };
            }
        }

        public static string SethBossTitleSuper 
        {
            get
            {
                return Language.CurrentLanguage() switch
                {
                    LanguageCode.FR => "Gardien de la Sauce Blanche Éternelle",
                    LanguageCode.EN => "Guardian of the Shrine of Skill Issues",
                    LanguageCode.JA => "スキル不足の神殿の守護者",
                    LanguageCode.ES => "Guardián del Santuario de la Falta de Habilidad",
                    LanguageCode.IT => "Guardiano del Santuario dei Problemi di Skill",
                    LanguageCode.DE => "Wächter des Schreins der Skillprobleme",
                    _ => "Guardian of the Shrine of Skill Issues"
                };
            }
        }

        public static string SethBossTitleSub
        {
            get
            {
                return Language.CurrentLanguage() switch
                {
                    LanguageCode.FR => "La Fraude",
                    LanguageCode.EN => "The Free Win",
                    LanguageCode.JA => "最後の守護者",
                    LanguageCode.ES => "La Victoria Gratis",
                    LanguageCode.IT => "La Vittoria Gratis",
                    LanguageCode.DE => "Der Gratis-Sieg",
                    _ => "The Free Win"
                };
            }
        }

        #endregion

        #region HP et seuils
        // ============================================================
        //                 HP & PHASE THRESHOLDS
        // ============================================================
        public const int SethMaxHp = 2500;

        public const int SethPhase15HpThreshold = 2200; // P1.5

        public const int SethPhase2HpThreshold = 1900;  // P2 réelle

        public const int SethPhase3HpThreshold = 1300;  // P3 custom


        public const int SethPhase4HpThreshold = 700;  // P4 custom

        #endregion

        #region Vitesses Animations par Phases
        // ============================================================
        //                 PHASED ANIMATION SPEED TABLES
        // ============================================================

        // ---------------------------
        // PHASE 1 (P1)
        // ---------------------------
        public static readonly Dictionary<string, float> AnimationSpeedP1 =
            new Dictionary<string, float>()
            {
                // SLASH COMBO
                {"Slash Antic G", 1.5f},
                {"Slash Combo 1", 1.4f},
                {"Slash Dash", 1.3f},
                {"Slash Combo 2", 1.2f},
                {"Slash Combo 3", 1.1f},
                {"Slash Combo 4", 1.1f},
                {"Slash Combo 5", 1.1f},
                {"Slash Combo 6", 1.1f},
                {"Slash Combo 7", 1.1f},
                {"Slash Combo 8", 1.1f},
                {"Slash Combo 9", 1.1f},
                {"Slash Combo 10", 1.1f},
                {"Slash Combo 11", 1.1f},

                // DIVE ATTACK
                {"Shield Dive", 1.8f},
                {"Shield Dive Land", 1f},

                // CYCLONE
                {"FadeSlash Antic", 1.1f},
                {"FadeSlash", 1.1f},
                {"FadeSlash D Strik", 1.1f},
                {"FadeSlash Cyclone", 1.1f},

                // SHIELD THROW
                {"Shield Throw Antic", 1.2f},
                {"Shield Throw A", 1.2f},
                {"Shield Throw G", 1.2f},
                {"Tele Out Straight", 1.2f},
                {"Tele In NoShield", 1.2f},
                {"Catch", 1.2f},

                // STAB
                {"Stab Antic", 1f},
                {"Stab Attack", 1f},
                {"Stab Recover", 1f},

                // BLOCK
                {"Block Stance", 1.2f},
                {"Block U", 1.2f},
                {"Block Hit U", 1.2f},
                {"Block Hit F", 1.2f},

                // MOVEMENT
                {"Jump Antic", 1.4f},
                {"Land", 1.4f},
                {"Jump", 1.4f},
                {"Fall", 1.4f},
                {"Evade", 1.4f},
                {"Hop", 1.4f},
                {"Tele Out", 1.4f},
                {"Tele In", 1.4f},
                {"Recover", 1.4f}
            };


        // ---------------------------
        // PHASE 2 (P1.5)
        // Identique à la P1
        // ---------------------------
        public static readonly Dictionary<string, float> AnimationSpeedP2 =
            AnimationSpeedP1;


        // ---------------------------
        // PHASE 3 (P2 réelle)
        // Copie indépendante modifiable
        // ---------------------------
        public static readonly Dictionary<string, float> AnimationSpeedP3 =
            new Dictionary<string, float>()
            {
                // SLASH COMBO
                {"Slash Antic G", 1.6f},
                {"Slash Combo 1", 1.5f},
                {"Slash Dash", 1.4f},
                {"Slash Combo 2", 1.3f},
                {"Slash Combo 3", 1.2f},
                {"Slash Combo 4", 1.2f},
                {"Slash Combo 5", 1.2f},
                {"Slash Combo 6", 1.2f},
                {"Slash Combo 7", 1.2f},
                {"Slash Combo 8", 1.2f},
                {"Slash Combo 9", 1.2f},
                {"Slash Combo 10", 1.2f},
                {"Slash Combo 11", 1.2f},

                // DIVE ATTACK
                {"Shield Dive", 1.9f},
                {"Shield Dive Land", 1f},

                // CYCLONE
                {"FadeSlash Antic", 1.2f},
                {"FadeSlash", 1.2f},
                {"FadeSlash D Strik", 1.2f},
                {"FadeSlash Cyclone", 1.2f},

                // SHIELD THROW
                {"Shield Throw Antic", 1.3f},
                {"Shield Throw A", 1.3f},
                {"Shield Throw G", 1.3f},
                {"Tele Out Straight", 1.3f},
                {"Tele In NoShield", 1.3f},
                {"Catch", 1.3f},

                // STAB
                {"Stab Antic", 1.1f},
                {"Stab Attack", 1.1f},
                {"Stab Recover", 1.1f},

                // BLOCK
                {"Block Stance", 1.2f},
                {"Block U", 1.2f},
                {"Block Hit U", 1.2f},
                {"Block Hit F", 1.2f},

                // MOVEMENT
                {"Jump Antic", 1.5f},
                {"Land", 1.5f},
                {"Jump", 1.5f},
                {"Fall", 1.5f},
                {"Evade", 1.5f},
                {"Hop", 1.5f},
                {"Tele Out", 1.5f},
                {"Tele In", 1.5f},
                {"Recover", 1.5f}
            };


        // ---------------------------
        // PHASE 4 (P3 custom)
        // Copie indépendante modifiable
        // ---------------------------
        public static readonly Dictionary<string, float> AnimationSpeedP4 =
            new Dictionary<string, float>()
            {
                // SLASH COMBO
                {"Slash Antic G", 1.7f},
                {"Slash Combo 1", 1.6f},
                {"Slash Dash", 1.5f},
                {"Slash Combo 2", 1.4f},
                {"Slash Combo 3", 1.3f},
                {"Slash Combo 4", 1.3f},
                {"Slash Combo 5", 1.3f},
                {"Slash Combo 6", 1.3f},
                {"Slash Combo 7", 1.3f},
                {"Slash Combo 8", 1.3f},
                {"Slash Combo 9", 1.3f},
                {"Slash Combo 10", 1.3f},
                {"Slash Combo 11", 1.3f},

                // DIVE ATTACK
                {"Shield Dive", 2f},
                {"Shield Dive Land", 1f},

                // CYCLONE
                {"FadeSlash Antic", 1.3f},
                {"FadeSlash", 1.3f},
                {"FadeSlash D Strik", 1.3f},
                {"FadeSlash Cyclone", 1.3f},

                // SHIELD THROW
                {"Shield Throw Antic", 1.4f},
                {"Shield Throw A", 1.4f},
                {"Shield Throw G", 1.4f},
                {"Tele Out Straight", 1.4f},
                {"Tele In NoShield", 1.4f},
                {"Catch", 1.4f},

                // STAB
                {"Stab Antic", 1.2f},
                {"Stab Attack", 1.2f},
                {"Stab Recover", 1.2f},

                // BLOCK
                {"Block Stance", 1.2f},
                {"Block U", 1.2f},
                {"Block Hit U", 1.2f},
                {"Block Hit F", 1.2f},

                // MOVEMENT
                {"Jump Antic", 1.6f},
                {"Land", 1.6f},
                {"Jump", 1.6f},
                {"Fall", 1.6f},
                {"Evade", 1.6f},
                {"Hop", 1.6f},
                {"Tele Out", 1.6f},
                {"Tele In", 1.6f},
                {"Recover", 1.6f}
            };

        // ---------------------------
        // PHASE 5 (P4 custom)
        // Copie indépendante modifiable
        // ---------------------------
        public static readonly Dictionary<string, float> AnimationSpeedP5 =
            new Dictionary<string, float>()
            {
                // SLASH COMBO
                {"Slash Antic G", 1.8f},
                {"Slash Combo 1", 1.7f},
                {"Slash Dash", 1.6f},
                {"Slash Combo 2", 1.5f},
                {"Slash Combo 3", 1.4f},
                {"Slash Combo 4", 1.4f},
                {"Slash Combo 5", 1.4f},
                {"Slash Combo 6", 1.4f},
                {"Slash Combo 7", 1.4f},
                {"Slash Combo 8", 1.4f},
                {"Slash Combo 9", 1.4f},
                {"Slash Combo 10", 1.4f},
                {"Slash Combo 11", 1.4f},

                // DIVE ATTACK
                {"Shield Dive", 2.1f},
                {"Shield Dive Land", 1f},

                // CYCLONE
                {"FadeSlash Antic", 1.4f},
                {"FadeSlash", 1.4f},
                {"FadeSlash D Strik", 1.4f},
                {"FadeSlash Cyclone", 1.4f},

                // SHIELD THROW
                {"Shield Throw Antic", 1.5f},
                {"Shield Throw A", 1.5f},
                {"Shield Throw G", 1.5f},
                {"Tele Out Straight", 1.5f},
                {"Tele In NoShield", 1.5f},
                {"Catch", 1.5f},

                // STAB
                {"Stab Antic", 1.3f},
                {"Stab Attack", 1.3f},
                {"Stab Recover", 1.3f},

                // BLOCK
                {"Block Stance", 1.2f},
                {"Block U", 1.2f},
                {"Block Hit U", 1.2f},
                {"Block Hit F", 1.2f},

                // MOVEMENT
                {"Jump Antic", 1.7f},
                {"Land", 1.7f},
                {"Jump", 1.7f},
                {"Fall", 1.7f},
                {"Evade", 1.7f},
                {"Hop", 1.7f},
                {"Tele Out", 1.7f},
                {"Tele In", 1.7f},
                {"Recover", 1.7f}
            };
        #endregion
    }

}