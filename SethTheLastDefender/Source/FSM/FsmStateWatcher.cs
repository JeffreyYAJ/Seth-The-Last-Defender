using UnityEngine;
using HutongGames.PlayMaker;

namespace SethPrime.Debug
{
    public class FsmStateWatcher : MonoBehaviour
    {
        private PlayMakerFSM phaseControl;
        private PlayMakerFSM control;

        private bool waitingForStun2 = false;
        private bool hasForcedStun = false;
        private bool transitionDone = false; // 🔹 nouvelle variable pour ne détecter P3 qu'une fois

        private float p3DetectedTime = -999f;

        private const float ForceDelay = 0.01f;      // délai après Set P3 pour forcer Stun Start 2


        private void Start()
        {
            var boss = GameObject.Find("Seth");
            if (boss == null) return;

            var fsms = boss.GetComponents<PlayMakerFSM>();
            foreach (var f in fsms)
            {
                if (f.FsmName == "Phase Control") phaseControl = f;
                if (f.FsmName == "Control") control = f;
            }
        }

        private void Update()
        {
            if (phaseControl == null || control == null) return;
            if (transitionDone) return; // 🔹 stop tout si transition déjà validée ou forcée

            string phaseState = phaseControl.ActiveStateName;
            string controlState = control.ActiveStateName;

            // Détection P3 (Set P3 / P3)
            if (!waitingForStun2 && (phaseState == "Set P3" || phaseState == "P3"))
            {
                waitingForStun2 = true;
                hasForcedStun = false;
                p3DetectedTime = Time.time;

                SethPrimeMain.Log?.LogInfo(
                    $"[FSM WATCHDOG] P3 detected! PhaseControlState={phaseState} | ControlState={controlState}"
                );
            }

            if (!waitingForStun2) return;

            // ✅ Run OK : Stun Start 2 atteint
            if (controlState == "Stun Start 2")
            {
                SethPrimeMain.Log?.LogInfo("[FSM WATCHDOG] Transition Phase 2 OK");
                waitingForStun2 = false;
                transitionDone = true; // 🔹 plus de détection future
                return;
            }

            // 🔹 Forçage rapide après ForceDelay si pas de Stun Start 2
            if (!hasForcedStun && Time.time - p3DetectedTime > ForceDelay)
            {
                SethPrimeMain.Log?.LogWarning("[FSM WATCHDOG] Transition Phase 2 BUGÉE -> Forçage");

                ForceStunStart2();

                hasForcedStun = true;
                transitionDone = true;   // 🔹 stop toute nouvelle détection
                waitingForStun2 = false;
            }

            
        }

        private void ForceStunStart2()
        {
            if (control == null) return;

            try
            {
                var target = control.Fsm.GetState("Stun Start 2");
                if (target != null)
                {
                    control.Fsm.SetState(target.Name); // 🔹 utiliser le nom du state
                }
                else
                {
                    // fallback : envoyer un event
                    control.SendEvent("Stun Start 2");
                }
            }
            catch
            {
                // silencieux
            }
        }
    }
}