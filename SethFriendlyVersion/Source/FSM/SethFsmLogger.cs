using UnityEngine;
using HutongGames.PlayMaker;
using System;

namespace SethPrime.Debugging
{
    public class SethFsmLogger : MonoBehaviour
    {
        private PlayMakerFSM[] fsms;
        private bool initialized = false;
        private SethWrapper wrapper;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (initialized) return;
            initialized = true;

            wrapper = GetComponent<SethWrapper>();
            if (!wrapper)
            {
                SethPrimeMain.Log.LogError("[SETH FSM LOGGER] No SethWrapper found on this GameObject");
                return;
            }

            fsms = GetComponents<PlayMakerFSM>();
            SethPrimeMain.Log.LogInfo("[SETH FSM LOGGER] Initialisation…");

            foreach (var fsm in fsms)
                HookFSM(fsm);
        }

        private void HookFSM(PlayMakerFSM fsm)
        {
            foreach (var state in fsm.FsmStates)
                InsertEnterLog(state, fsm.FsmName);

            SethPrimeMain.Log.LogInfo($"[SETH FSM LOGGER] Hooked FSM: {fsm.FsmName}");
        }

        private void InsertEnterLog(FsmState state, string fsmName)
        {
            foreach (var action in state.Actions)
                if (action is LogStateEnterAction) return;

            var old = state.Actions;
            var newActions = new FsmStateAction[old.Length + 1];

            newActions[0] = new LogStateEnterAction(fsmName, state.Name, wrapper);
            Array.Copy(old, 0, newActions, 1, old.Length);

            state.Actions = newActions;
        }

        private class LogStateEnterAction : FsmStateAction
        {
            private readonly string fsmName;
            private readonly string stateName;
            private readonly SethWrapper wrapper; // ✅ clé

            public LogStateEnterAction(string fsm, string state, SethWrapper wrapper)
            {
                fsmName = fsm;
                stateName = state;
                this.wrapper = wrapper;
            }

            public override void OnEnter()
            {
                SethContext.Current = wrapper;

                SethPrimeMain.Log.LogInfo(
                    $"[SETH FSM] Enter: {fsmName} → {stateName} (Context updated)"
                );

                Finish();
            }
        }
    }
}