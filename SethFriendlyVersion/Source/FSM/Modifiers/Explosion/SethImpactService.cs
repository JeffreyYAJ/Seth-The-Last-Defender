using UnityEngine;
using HutongGames.PlayMaker.Actions;

namespace SethPrime
{
    public static class SethImpactService
    {
        private static float lastImpactTime;
        private const float MinDelay = 0.1f;

        public static void PlayAtPosition(
            GameObject sethRoot,
            ImpactType type,
            Vector3 position,
            bool freezeInWorld = true,
            float freezeDuration = 0.4f
        )
        {
            if (!sethRoot) return;
            if (!PhaseAllowed(sethRoot)) return;

            if (Time.time - lastImpactTime < MinDelay)
                return;

            lastImpactTime = Time.time;

            SethImpactCache.Init(sethRoot);

            switch (type)
            {
                case ImpactType.DiveLandStomp:
                case ImpactType.ShieldBlock:
                case ImpactType.ShieldGroundBounce:
                case ImpactType.PlayerExplosion:
                    TriggerStomp(sethRoot, position, type, freezeInWorld, freezeDuration);
                    break;
            }
        }

        private static bool PhaseAllowed(GameObject sethRoot)
        {
            var wrapper = sethRoot.GetComponent<SethWrapper>();
            if (!wrapper) return false;

            return wrapper.CurrentPhase >= 4;
        }

        private static void TriggerStomp(
            GameObject sethRoot,
            Vector3 worldPos,
            ImpactType type,
            bool freezeInWorld,
            float freezeDuration
        )
        {
            var prefab = SethImpactCache.StompBlastGO;
            if (!prefab)
            {
                SethPrimeMain.Log.LogWarning("[ImpactService] StompBlastGO NULL !");
                return;
            }

            // On instancie une copie pour que ça ne suive pas Seth
            var stomp = Object.Instantiate(prefab, worldPos, Quaternion.identity);
            stomp.SetActive(true);

            SethPrimeMain.Log.LogInfo(
                $"[ImpactService] TriggerStomp Pos={worldPos} StompGO={stomp.name} Type={type}"
            );


            // Camera shake
            if (SethImpactCache.EnemyKillProfile)
            {
                new DoCameraShake
                {
                    Camera = new HutongGames.PlayMaker.FsmObject { Value = Camera.main },
                    Profile = new HutongGames.PlayMaker.FsmObject { Value = SethImpactCache.EnemyKillProfile },
                    DoFreeze = new HutongGames.PlayMaker.FsmBool { Value = false }
                }.OnEnter();
            }
        }

        private static void Play(AudioSource src, AudioClip clip, float pitch, float vol)
        {
            if (!clip) return;
            src.pitch = pitch;
            src.PlayOneShot(clip, vol);
        }
    }
}