using HutongGames.PlayMaker;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;



namespace SethPrime.Debugging
{
    public static class FsmImpactDebug
    {
        public static void DumpDiveLand(PlayMakerFSM fsm, GameObject root)
        {
            SethPrimeMain.Log.LogInfo("===== [Impact DEBUG] DIVE LAND DUMP =====");

            var diveLand = fsm.Fsm.GetState("Dive Land");
            if (diveLand == null)
            {
                SethPrimeMain.Log.LogWarning("[Impact DEBUG] Dive Land state not found");
                return;
            }

            for (int i = 0; i < diveLand.Actions.Length; i++)
            {
                var a = diveLand.Actions[i];
                if (a == null) continue;

                var t = a.GetType();
                SethPrimeMain.Log.LogInfo($"[Impact Action {i}] {t.FullName}");

                DumpActionFields(a);
                DumpTk2dClipEvents(root, "Shield Dive Land");
                SethPrimeMain.Log.LogInfo("----");
            }

            SethPrimeMain.Log.LogInfo("===== [Impact DEBUG] END =====");
        }

        private static void DumpActionFields(object action)
        {
            try
            {
                var t = action.GetType();

                // Champs publics + privés (PlayMaker stocke souvent en private)
                var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var f in fields)
                {
                    object val = null;
                    try
                    {
                        val = f.GetValue(action);
                    }
                    catch (Exception e)
                    {
                        SethPrimeMain.Log.LogWarning($"[Impact DEBUG] Field read failed {f.Name}: {e.Message}");
                        continue;
                    }

                    SethPrimeMain.Log.LogInfo($"  {f.FieldType.Name} {f.Name} = {FormatValue(val)}");
                }
            }
            catch (Exception e)
            {
                SethPrimeMain.Log.LogWarning($"[Impact DEBUG] DumpActionFields failed: {e}");
            }
        }
        private static void DumpTk2dClipEvents(GameObject root, string clipName)
        {
            var anim = root.GetComponent<tk2dSpriteAnimator>();
            if (anim == null || anim.Library == null || anim.Library.clips == null)
            {
                SethPrimeMain.Log.LogWarning("[Impact DEBUG] tk2d animator/library missing");
                return;
            }

            var clip = anim.Library.clips.FirstOrDefault(c => c != null && c.name == clipName);
            if (clip == null)
            {
                SethPrimeMain.Log.LogWarning($"[Impact DEBUG] tk2d clip not found: {clipName}");
                return;
            }

            SethPrimeMain.Log.LogInfo($"[Impact DEBUG] tk2d clip '{clipName}' fields:");

            var clipType = clip.GetType();
            var fields = clipType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var f in fields)
            {
                object val = null;
                try
                {
                    val = f.GetValue(clip);
                }
                catch { }
                SethPrimeMain.Log.LogInfo($"  {f.FieldType.Name} {f.Name} = {FormatValue(val)}");
            }

            // Optionnel : lister aussi les propriétés
            var props = clipType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var p in props)
            {
                object val = null;
                try
                {
                    val = p.GetValue(clip);
                }
                catch { }
                SethPrimeMain.Log.LogInfo($"  {p.PropertyType.Name} {p.Name} = {FormatValue(val)}");
            }
        }
        private static string FormatValue(object val)
        {
            if (val == null) return "null";

            // Unity Object
            if (val is UnityEngine.Object uo)
                return $"{uo.name} ({uo.GetType().Name})";

            // PlayMaker variables (souvent FsmXxx)
            var t = val.GetType();
            var valueProp = t.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
            if (valueProp != null && valueProp.CanRead)
            {
                try
                {
                    var inner = valueProp.GetValue(val, null);
                    return $"{t.Name}.Value={inner ?? "null"}";
                }
                catch { /* ignore */ }
            }

            return val.ToString();
        }
    }
}