using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SethPrime.Patches;
using System;
using System.IO;
using System.Reflection;
using TeamCherry.Localization;
using UnityEngine;
using UnityEngine.Audio; // ✅ IMPORTANT
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


namespace SethPrime
{
    [BepInPlugin("com.sethprime.mod", "Seth Prime", "1.0.0")]
    public class SethPrimeMain : BaseUnityPlugin
    {
        public static SethPrimeMain Instance; // ✅ accès global
        public static ManualLogSource Log;
        private Harmony _harmony;

        public AudioClip Phase3CustomMusic;
        public AudioClip ThxKarmelitaPrimeClip;

        public bool IsInitialized = false; // 🔹 flag pour indiquer que Awake est fini



        private void Awake()
        {
            Log = Logger;
            Instance = this; // ✅ assignation de l'instance
            Log.LogInfo("[SethPrime] Plugin chargé !");

            _harmony = new Harmony("com.sethprime.mod");
            _harmony.PatchAll();
            Log.LogInfo("[SethPrime] Harmony PatchAll exécuté.");

            Log.LogInfo("[SethPrime] Harmony Localization Patch exécuté.");

            AudioLogger.Initialize();
            Log.LogInfo("[SethPrime] AudioLogger initialisé.");

            DamageChanger.Init(this);
            LoadEmbeddedResources(); // 🔹 charge p3musique

            IsInitialized = true; // 🔹 on peut maintenant remplacer la musique


            Log.LogInfo("[SethPrime] Fin du Awake de SethPrimeMain.");

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }


        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ResetGlobalMusicState($"SceneLoaded: {scene.name}");
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            ResetGlobalMusicState($"SceneLoaded: {scene.name}");
        }
        private static void ResetGlobalMusicState(string source)
        {
            SethMusicState.IsInP3Custom = false;

            // 🔹 reset clip boss pour éviter le pop de P3 Custom
            if (SethAudioHandler.BossMusicSource != null)
            {
                SethAudioHandler.BossMusicSource.Stop();
                SethAudioHandler.BossMusicSource.clip = null;
            }

            Log?.LogInfo($"[SethPrime][MusicState] Reset global ({source})");
        }
        private void LoadEmbeddedResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                Log.LogInfo("[SethPrime] Ressource trouvée : " + resourceName);

                if (resourceName.ToLower().Contains("p3musique"))
                {
                    Log.LogInfo("[SethPrime] Ressource p3musique trouvée, chargement...");
                    Phase3CustomMusic = GetEmbeddedClip(resourceName, AudioType.MPEG);
                    if (Phase3CustomMusic != null)
                        Log.LogInfo("[SethPrime] Clip P3 chargé avec succès !");
                    else
                        Log.LogError("[SethPrime] Impossible de charger le clip P3 !");
                }
                if (resourceName.ToLower().Contains("thxkarmelitaprime"))
                {
                    Log.LogInfo("[SethPrime] Ressource ThxKarmelitaPrime trouvée, chargement...");
                    ThxKarmelitaPrimeClip = GetEmbeddedClip(resourceName, AudioType.WAV);

                    if (ThxKarmelitaPrimeClip != null)
                        Log.LogInfo("[SethPrime] Clip ThxKarmelitaPrime chargé avec succès !");
                    else
                        Log.LogError("[SethPrime] Impossible de charger ThxKarmelitaPrime !");
                }
            }
        }

        private AudioClip GetEmbeddedClip(string resourceName, AudioType audioType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) return null;

            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                buffer = ms.ToArray();
            }

            string tempPath = Path.Combine(Application.temporaryCachePath, $"temp_p3music_{Guid.NewGuid()}.mp3");
            File.WriteAllBytes(tempPath, buffer);
            string url = $"file:///{tempPath}";

            AudioClip clip = null;
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                uwr.SendWebRequest();
                while (!uwr.isDone) { }
                if (uwr.result == UnityWebRequest.Result.Success)
                    clip = DownloadHandlerAudioClip.GetContent(uwr);
            }

            File.Delete(tempPath);
            return clip;
        }

        private void OnDestroy()
        {
            PlayerImpactService.DisablePhase4Explosions();
            _harmony?.UnpatchSelf();
        }

        private void Update()
        {
            var boss = GameObject.Find("Seth");
            if (boss == null)
                return;

            // Wrapper
            if (!boss.TryGetComponent<SethWrapper>(out var wrapper))
            {
                Log.LogInfo("[SethPrime] Wrapper ajouté.");
                wrapper = boss.AddComponent<SethWrapper>();
            }

            // FSM Controller (NON STATIC)
            if (!boss.TryGetComponent<SethWrapper>(out _))
            {
                Log.LogInfo("[SethPrime] Wrapper ajouté.");
                boss.AddComponent<SethWrapper>();
            }

            // Crée l'instance non-static de SethFsmController
            var fsmController = new SethFsmController(boss);

            if (!boss.TryGetComponent<SethPrime.Debug.FsmStateWatcher>(out _))
            {
                SethPrimeMain.Log.LogInfo("[SethPrime] FsmStateWatcher ajouté.");
                boss.AddComponent<SethPrime.Debug.FsmStateWatcher>();
            }
        }
    }
}