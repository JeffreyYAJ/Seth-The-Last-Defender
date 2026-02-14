using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.Debugging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SethPrime.Constants;
using SethPrime.Patches;

namespace SethPrime
{
    public class SethWrapper : MonoBehaviour
    {
        private HealthManager health;
        public HealthManager Health => health;

        private PlayMakerFSM phaseFsm;
        private PlayMakerFSM controlFsm;

        public PlayMakerFSM MainControlFsm { get; private set; }
        public PlayMakerFSM StunFsm { get; private set; }
        public PlayMakerFSM ShieldFsm { get; private set; }
        public PlayMakerFSM ShieldColliderFsm { get; private set; }

        private bool hasForcedCombo = false;


        public AudioClip BossGenericDeathAudio;
        public AudioClip EnemyDamageOldAudio;
        public AudioClip EnemyDeathSwordOldAudio;
        public AudioClip SethDeath2Audio;
        public AudioClip SethStunAudio;
        public AudioClip SethTalkAudio;
        public AudioClip SethHitAudio;
        public AudioClip SethRoarAudio;
        public AudioClip BossStunAudio;

        public Rigidbody2D rb;

        public tk2dSpriteAnimator animator;
        private SethFsmController fsmController;

        private bool HpPhase2Check = true;
        private bool HpPhase15Check = true;

        private int lastPhase = 1;
        public int CurrentPhase => lastPhase;

        private bool p3CustomRequested = false;
        private bool p3CustomApplied = false;

        private bool p4CustomRequested = false;
        private bool p4CustomApplied = false;

        public bool IsInCombo = false;
        // ------------------------------------------
        // RESSOURCES AUDIO
        // ------------------------------------------
        private void GetResources()
        {
            foreach (var clip in Resources.FindObjectsOfTypeAll<AudioClip>())
            {
                if (clip.name.Contains("boss_death_pt_1"))
                    BossGenericDeathAudio = clip;
                if (clip.name.Contains("enemy_damage_old"))
                    EnemyDamageOldAudio = clip;
                if (clip.name.Contains("enemy_death_sword_old"))
                    EnemyDeathSwordOldAudio = clip;
                if (clip.name.Contains("SB_death_hit_02"))
                    SethDeath2Audio = clip;
                if (clip.name.Contains("SB_stun_01"))
                    SethStunAudio = clip;
                if (clip.name.Contains("SB_talk_01"))
                    SethTalkAudio = clip;
                if (clip.name == "song_gate_hit")
                    SethHitAudio = clip;
                if (clip.name.Contains("SB_roar_01"))
                    SethRoarAudio = clip;
                if (clip.name == "boss_stun")
                    BossStunAudio = clip;
            }
        }


        // ------------------------------------------
        // AWAKE
        // ------------------------------------------
        private void Awake()
        {
            SethPrimeMain.Log.LogInfo("[SethPrime][Wrapper] === AWAKE ===");

            PlayerImpactService.DisablePhase4Explosions();
            // Récupération du FSM Shield Collider
            ShieldColliderFsm = GetComponents<PlayMakerFSM>()
                .FirstOrDefault(f => f.FsmName == "Shield Collider");

            if (ShieldColliderFsm == null)
            {
                SethPrimeMain.Log.LogWarning("[SethWrapper] Shield Collider FSM introuvable");
                return;
            }

            SethPrimeMain.Log.LogInfo("[SethWrapper] Shield Collider FSM trouvé");

            // 🔹 LOG ÉTATS
            foreach (var s in ShieldColliderFsm.Fsm.States)
            {
                SethPrimeMain.Log.LogInfo($"[ShieldCollider] State = {s.Name}");
            }

            // 🔹 LOG EVENTS
            foreach (var e in ShieldColliderFsm.Fsm.Events)
            {
                SethPrimeMain.Log.LogInfo($"[ShieldCollider] Event = {e.Name}");
            }


            GetResources();

            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<tk2dSpriteAnimator>();
            health = GetComponent<HealthManager>();
            if (health == null)
            {
                SethPrimeMain.Log.LogError("HealthManager introuvable !");
                return;
            }

            // TrySetupControlFsm sera appelé plus bas
            StunFsm = FsmHelper.GetFSM(gameObject, "Stun");
            if (StunFsm != null) StunFsm.enabled = false;

            phaseFsm = GetComponents<PlayMakerFSM>().FirstOrDefault(f => f.FsmName == "Phase Control");
            if (phaseFsm == null)
            {
                SethPrimeMain.Log.LogError("FSM Phase Control introuvable !");
                return;
            }
            phaseFsm.enabled = false;

            HealthChanger.Initialize(health, Constants.SethMaxHp);

            var p2 = phaseFsm.FsmVariables.IntVariables.FirstOrDefault(v => v.Name == "P2 HP");
            if (p2 != null) p2.Value = Constants.SethPhase15HpThreshold;

            var p3 = phaseFsm.FsmVariables.IntVariables.FirstOrDefault(v => v.Name == "P3 HP");
            if (p3 != null) p3.Value = Constants.SethPhase2HpThreshold;

            var init = phaseFsm.Fsm.GetState("Init");
            if (init != null)
            {
                init.Actions = new FsmStateAction[0];
                foreach (var tr in init.Transitions)
                    tr.ToState = "P1";
            }

            phaseFsm.enabled = true;

            if (gameObject.GetComponent<SethPrime.Debugging.SethFsmLogger>() == null)
                gameObject.AddComponent<SethPrime.Debugging.SethFsmLogger>();

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            TrySetupControlFsm();

            LocalizationPatchApplier.Apply();
            SethImpactCache.Init(gameObject);

            IsInCombo = false;
            SethPrimeMain.Log.LogInfo("[SethPrime] IsInCombo = False");

            SethPrimeMain.Log.LogInfo("[SethPrime] Localization Patch appliqué");
            SethPrimeMain.Log.LogInfo("[SethPrime][Wrapper] === AWAKE END ===");
        }


        private void OnSceneLoaded(Scene s, LoadSceneMode _)
        {
            TrySetupControlFsm();
        }


        // ------------------------------------------
        // SETUP FSM
        // ------------------------------------------
        private void TrySetupControlFsm()
        {
            if (fsmController != null) return;

            fsmController = new SethFsmController(gameObject);
            fsmController.SetupFSM();

            controlFsm = fsmController.ControlFsm;
            StunFsm = fsmController.StunFsm;
            ShieldFsm = fsmController.ShieldFsm;
            MainControlFsm = controlFsm;
        }


        // ------------------------------------------
        // UPDATE
        // ------------------------------------------
        private void Update()
        {
            
            // 🔥 FORÇAGE FINAL — FIN DE COMBAT
            ForceComboStartIfHpBelow(180);

            //Phase 1.5 HP Check
            if (HpPhase15Check == true && health.hp <= SethPhase15HpThreshold)
            {
                SethPrimeMain.Log.LogInfo("[SethPrime][Wrapper] Hp Phase 1.5 Passés");
                HpPhase15Check = false;
            }

            //Phase 2 HP Check
            if (HpPhase2Check == true && health.hp <= SethPhase2HpThreshold)
            {
                SethPrimeMain.Log.LogInfo("[SethPrime][Wrapper] Hp Phase 2 Passés");
                HpPhase2Check = false;
            }

            // Phase 3 Trigger
            if (!p3CustomApplied && p3CustomRequested)
            {
                p3CustomRequested = false;
                p3CustomApplied = true;

                rb.linearVelocity = Vector2.zero;
                lastPhase = 4;

                fsmController.ApplyPhase(4);
                controlFsm.Fsm.SetState("Phase 3 Knocked 0");
                
            }

            // Phase 4 Trigger
            if (!p4CustomApplied && p4CustomRequested)
            {
                p4CustomRequested = false;
                p4CustomApplied = true;

                rb.linearVelocity = Vector2.zero;
                lastPhase = 5;

                fsmController.ApplyPhase(5);
                controlFsm.Fsm.SetState("Phase 4 Knocked 0");
                
            }
            if (phaseFsm == null || controlFsm?.Fsm == null) return;

            string active = controlFsm.Fsm.ActiveStateName;
            if (string.IsNullOrEmpty(active)) return;

            if (health.hp <= SethPhase15HpThreshold && lastPhase < 2)
            {
                lastPhase = 2;
                SethPrimeMain.Log.LogInfo("[SethPrime][Wrapper] Phase détectée = P2 (HP threshold)");
                fsmController.ApplyPhase(2);
                return;
            }

            if (active == "Rage Roar Antic" && lastPhase < 3)
            {
                lastPhase = 3;
                SethPrimeMain.Log.LogInfo("[SethPrime][Wrapper] Phase détectée = P3 (vanilla)");
                fsmController.ApplyPhase(3);
                return;
            }

            if (health.hp <= SethPhase3HpThreshold && lastPhase < 4)
            {
                p3CustomRequested = true;
                SethPrimeMain.Log.LogInfo("[SethPrime][Wrapper] Phase détectée = P4 (P3 custom)");
            }

            if (health.hp <= SethPhase4HpThreshold && lastPhase < 5)
            {
                p4CustomRequested = true;
                SethPrimeMain.Log.LogInfo("[SethPrime][Wrapper] Phase détectée = P5 (P4 custom)");
            }

            PlayerImpactService.UpdatePhase4Explosions();

        }

        // ------------------------------------------
        // ANIMATION SPEED
        // ------------------------------------------
        public float GetAnimationStartTime() => fsmController.GetStateStartTime();
        public void ForceComboStartIfHpBelow(int hpThreshold)
        {
            if (hasForcedCombo) return;
            if (health == null || controlFsm?.Fsm == null) return;
            if (health.hp > hpThreshold) return;
            if (IsInCombo == true) return;

            hasForcedCombo = true;

            SethPrimeMain.Log.LogInfo(
                $"[SethPrime][Wrapper] 🔥 ForceComboStart → 'Debut Combo 1' (HP={health.hp}, Threshold={hpThreshold})"
            );

            rb.linearVelocity = Vector2.zero;
            controlFsm.Fsm.SetState("Debut Combo 1");
        }
        public float GetAnimationSpeedModifier(string clip)
        {
            if (string.IsNullOrEmpty(clip)) return 1f;

            return lastPhase switch
            {
                1 => AnimationSpeedP1.GetValueOrDefault(clip, 1f),
                2 => AnimationSpeedP2.GetValueOrDefault(clip, 1f),
                3 => AnimationSpeedP3.GetValueOrDefault(clip, 1f),
                4 => AnimationSpeedP4.GetValueOrDefault(clip, 1f),
                5 => AnimationSpeedP5.GetValueOrDefault(clip, 1f),
                _ => 1f
            };
        }

    }
}