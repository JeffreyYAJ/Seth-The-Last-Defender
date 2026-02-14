using UnityEngine;
using System.Collections;

namespace SethPrime
{
    public static class PlayerImpactService
    {
        private static AudioSource _audioSrc;
        private static bool Phase4Active = false;
        private static float lastPhase4ExplosionTime = 0f;
        private const float Phase4Cooldown = 10f; // délai entre explosion

        public static void ExplodeOnPlayer(float delay = 0.15f, float volume = 1f)
        {
            var hero = HeroController.instance;
            if (!hero)
            {
                SethPrimeMain.Log.LogWarning("[PlayerImpactService] HeroController NULL");
                return;
            }

            EnsureAudio(hero.gameObject);

            // 🎯 Capture IMMÉDIATE de la position
            Vector3 explosionPos = hero.transform.position;
            explosionPos.y = 10f;

            // 🔊 Son joué maintenant
            var clip = SethPrimeMain.Instance?.ThxKarmelitaPrimeClip;
            if (clip && _audioSrc)
            {
                SethPrimeMain.Log.LogInfo(
                    $"[PlayerImpactService] Play ThxKarmelitaPrime @ {explosionPos}"
                );
                _audioSrc.pitch = 1f;
                _audioSrc.PlayOneShot(clip, volume);
            }
            else
            {
                SethPrimeMain.Log.LogWarning("[PlayerImpactService] Clip ou AudioSource NULL");
            }

            // ⏱️ Explosion différée
            hero.StartCoroutine(
                DelayedExplosion(explosionPos, delay)
            );
        }

        private static IEnumerator DelayedExplosion(Vector3 explosionPos, float delay)
        {
            yield return new WaitForSeconds(delay);

            var seth = GameObject.Find("Seth");
            if (!seth)
            {
                SethPrimeMain.Log.LogWarning("[PlayerImpactService] Seth introuvable");
                yield break;
            }

            SethImpactService.PlayAtPosition(
                seth,
                ImpactType.PlayerExplosion,
                explosionPos,
                freezeInWorld: true,
                freezeDuration: 0.4f
            );

            SethPrimeMain.Log.LogInfo(
                $"[PlayerImpactService] PlayerExplosion triggered @ {explosionPos}"
            );
        }

        private static void EnsureAudio(GameObject host)
        {
            if (_audioSrc) return;

            _audioSrc = host.AddComponent<AudioSource>();
            _audioSrc.spatialBlend = 0f; // 2D
            _audioSrc.playOnAwake = false;
            _audioSrc.volume = 1f;

            SethPrimeMain.Log.LogInfo("[PlayerImpactService] AudioSource créée sur le joueur");
        }

        public static void EnablePhase4Explosions()
        {
            Phase4Active = true;
            lastPhase4ExplosionTime = Time.time;

            SethPrimeMain.Log.LogInfo("[PlayerImpactService] Phase 4 explosions ENABLED");
        }

        public static void DisablePhase4Explosions()
        {
            Phase4Active = false;

            SethPrimeMain.Log.LogInfo("[PlayerImpactService] Phase 4 explosions DISABLED");
        }

        public static void UpdatePhase4Explosions()
        {
            if (!Phase4Active)
                return;

            if (Time.time - lastPhase4ExplosionTime < Phase4Cooldown)
                return;

            var hero = HeroController.instance;
            if (hero == null)
                return;

            // ❗ NOUVELLE CONDITION
            if (!hero.cState.onGround)
            {
                // timer prêt MAIS joueur en l'air → on attend
                return;
            }

            // ✅ TIMER OK + JOUEUR AU SOL
            SethPrimeMain.Log.LogInfo("[PlayerImpactService] Phase 4 grounded explosion");

            ExplodeOnPlayer(0.4f, 1f); // Délai explosion phase 4 libre

            lastPhase4ExplosionTime = Time.time;
        }
    }
}