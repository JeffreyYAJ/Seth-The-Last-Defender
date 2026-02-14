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
                    LanguageCode.FR => "Gardien du Sanctuaire Oublié",
                    LanguageCode.EN => "Guardian of the Forgotten Shrine",
                    LanguageCode.JA => "忘れられた神殿の守護者",
                    LanguageCode.ES => "Guardián del Santuario Olvidado",
                    LanguageCode.IT => "Guardiano del Santuario Dimenticato",
                    LanguageCode.DE => "Wächter des Vergessenen Schreins",
                    _ => "Guardian of the Forgotten Shrine"
                };
            }
        }

        public static string SethBossTitleSub
        {
            get
            {
                return Language.CurrentLanguage() switch
                {
                    LanguageCode.FR => "Le Dernier Protecteur",
                    LanguageCode.EN => "The Last Defender",
                    LanguageCode.JA => "最後の守護者",
                    LanguageCode.ES => "El Último Protector",
                    LanguageCode.IT => "L'Ultimo Protettore",
                    LanguageCode.DE => "Der Letzte Beschützer",
                    _ => "The Last Defender"
                };
            }
        }

        #endregion

        #region HP et seuils
        // ============================================================
        //                 HP & PHASE THRESHOLDS
        // ============================================================
        public const int SethMaxHp = 3300;

        public const int SethPhase15HpThreshold = 2900; // P1.5

        public const int SethPhase2HpThreshold = 2550;  // P2 réelle

        public const int SethPhase3HpThreshold = 1800;  // P3 custom


        public const int SethPhase4HpThreshold = 1000;  // P4 custom

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
                {"Slash Antic G", 1.9f},
                {"Slash Combo 1", 1.8f},
                {"Slash Dash", 1.7f},
                {"Slash Combo 2", 1.6f},
                {"Slash Combo 3", 1.5f},
                {"Slash Combo 4", 1.5f},
                {"Slash Combo 5", 1.5f},
                {"Slash Combo 6", 1.5f},
                {"Slash Combo 7", 1.5f},
                {"Slash Combo 8", 1.5f},
                {"Slash Combo 9", 1.5f},
                {"Slash Combo 10", 1.5f},
                {"Slash Combo 11", 1.5f},

                // DIVE ATTACK
                {"Shield Dive", 2.2f},
                {"Shield Dive Land", 1f},

                // CYCLONE
                {"FadeSlash Antic", 1.5f},
                {"FadeSlash", 1.5f},
                {"FadeSlash D Strik", 1.5f},
                {"FadeSlash Cyclone", 1.5f},

                // SHIELD THROW
                {"Shield Throw Antic", 1.6f},
                {"Shield Throw A", 1.6f},
                {"Shield Throw G", 1.6f},
                {"Tele Out Straight", 1.6f},
                {"Tele In NoShield", 1.6f},
                {"Catch", 1.6f},

                // STAB
                {"Stab Antic", 1.4f},
                {"Stab Attack", 1.4f},
                {"Stab Recover", 1.4f},

                // BLOCK
                {"Block Stance", 1.2f},
                {"Block U", 1.2f},
                {"Block Hit U", 1.2f},
                {"Block Hit F", 1.2f},

                // MOVEMENT
                {"Jump Antic", 1.8f},
                {"Land", 1.8f},
                {"Jump", 1.8f},
                {"Fall", 1.8f},
                {"Evade", 1.8f},
                {"Hop", 1.8f},
                {"Tele Out", 1.8f},
                {"Tele In", 1.8f},
                {"Recover", 1.8f}
            };


        // ---------------------------
        // PHASE 4 (P3 custom)
        // Copie indépendante modifiable
        // ---------------------------
        public static readonly Dictionary<string, float> AnimationSpeedP4 =
            new Dictionary<string, float>()
            {
                // SLASH COMBO
                {"Slash Antic G", 2f},
                {"Slash Combo 1", 1.9f},
                {"Slash Dash", 1.8f},
                {"Slash Combo 2", 1.7f},
                {"Slash Combo 3", 1.6f},
                {"Slash Combo 4", 1.6f},
                {"Slash Combo 5", 1.6f},
                {"Slash Combo 6", 1.6f},
                {"Slash Combo 7", 1.6f},
                {"Slash Combo 8", 1.6f},
                {"Slash Combo 9", 1.6f},
                {"Slash Combo 10", 1.6f},
                {"Slash Combo 11", 1.6f},

                // DIVE ATTACK
                {"Shield Dive", 2.3f},
                {"Shield Dive Land", 1f},

                // CYCLONE
                {"FadeSlash Antic", 1.6f},
                {"FadeSlash", 1.6f},
                {"FadeSlash D Strik", 1.6f},
                {"FadeSlash Cyclone", 1.6f},

                // SHIELD THROW
                {"Shield Throw Antic", 1.7f},
                {"Shield Throw A", 1.7f},
                {"Shield Throw G", 1.7f},
                {"Tele Out Straight", 1.7f},
                {"Tele In NoShield", 1.7f},
                {"Catch", 1.7f},

                // STAB
                {"Stab Antic", 1.5f},
                {"Stab Attack", 1.5f},
                {"Stab Recover", 1.5f},

                // BLOCK
                {"Block Stance", 1.2f},
                {"Block U", 1.2f},
                {"Block Hit U", 1.2f},
                {"Block Hit F", 1.2f},

                // MOVEMENT
                {"Jump Antic", 1.9f},
                {"Land", 1.9f},
                {"Jump", 1.9f},
                {"Fall", 1.9f},
                {"Evade", 1.9f},
                {"Hop", 1.9f},
                {"Tele Out", 1.9f},
                {"Tele In", 1.9f},
                {"Recover", 1.9f}
            };

        // ---------------------------
        // PHASE 5 (P4 custom)
        // Copie indépendante modifiable
        // ---------------------------
        public static readonly Dictionary<string, float> AnimationSpeedP5 =
            new Dictionary<string, float>()
            {
                // SLASH COMBO
                {"Slash Antic G", 2.1f},
                {"Slash Combo 1", 2f},
                {"Slash Dash", 1.9f},
                {"Slash Combo 2", 1.8f},
                {"Slash Combo 3", 1.7f},
                {"Slash Combo 4", 1.7f},
                {"Slash Combo 5", 1.7f},
                {"Slash Combo 6", 1.7f},
                {"Slash Combo 7", 1.7f},
                {"Slash Combo 8", 1.7f},
                {"Slash Combo 9", 1.7f},
                {"Slash Combo 10", 1.7f},
                {"Slash Combo 11", 1.7f},

                // DIVE ATTACK
                {"Shield Dive", 2.4f},
                {"Shield Dive Land", 1f},

                // CYCLONE
                {"FadeSlash Antic", 1.7f},
                {"FadeSlash", 1.7f},
                {"FadeSlash D Strik", 1.7f},
                {"FadeSlash Cyclone", 1.7f},

                // SHIELD THROW
                {"Shield Throw Antic", 1.8f},
                {"Shield Throw A", 1.8f},
                {"Shield Throw G", 1.8f},
                {"Tele Out Straight", 1.8f},
                {"Tele In NoShield", 1.8f},
                {"Catch", 1.8f},

                // STAB
                {"Stab Antic", 1.6f},
                {"Stab Attack", 1.6f},
                {"Stab Recover", 1.6f},

                // BLOCK
                {"Block Stance", 1.2f},
                {"Block U", 1.2f},
                {"Block Hit U", 1.2f},
                {"Block Hit F", 1.2f},

                // MOVEMENT
                {"Jump Antic", 2f},
                {"Land", 2f},
                {"Jump", 2f},
                {"Fall", 2f},
                {"Evade", 2f},
                {"Hop", 2f},
                {"Tele Out", 2f},
                {"Tele In", 2f},
                {"Recover", 2f}

            };
        #endregion
    }

}