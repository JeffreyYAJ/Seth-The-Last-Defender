using System.Text;
using HutongGames.PlayMaker;
using UnityEngine;

namespace SethPrime
{
    public static class SethFsmDumper
    {
        public static void DumpAllFsms(GameObject boss)
        {
            if (boss == null)
            {
                SethPrimeMain.Log.LogError("[SethPrime][Dumper] Boss null.");
                return;
            }

            var fsms = boss.GetComponents<PlayMakerFSM>();
            if (fsms == null || fsms.Length == 0)
            {
                SethPrimeMain.Log.LogWarning("[SethPrime][Dumper] Aucun FSM.");
                return;
            }

            SethPrimeMain.Log.LogInfo($"[SethPrime][Dumper] ===== FSM de {boss.name} =====");

            foreach (var fsm in fsms)
                DumpFsm(fsm);

            SethPrimeMain.Log.LogInfo("[SethPrime][Dumper] ===== FIN =====");
        }

        private static void DumpFsm(PlayMakerFSM fsm)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"[FSM] Nom : {fsm.FsmName}");
            sb.AppendLine($" - États : {fsm.FsmStates.Length}");

            sb.AppendLine("--- VARIABLES INT ---");
            foreach (var iv in fsm.FsmVariables.IntVariables)
                sb.AppendLine($"  • {iv.Name} = {iv.Value}");

            sb.AppendLine("--- VARIABLES FLOAT ---");
            foreach (var fv in fsm.FsmVariables.FloatVariables)
                sb.AppendLine($"  • {fv.Name} = {fv.Value}");

            sb.AppendLine("--- ÉTATS ---");
            foreach (var state in fsm.FsmStates)
            {
                sb.AppendLine($"  ► State: {state.Name}");

                foreach (var action in state.Actions)
                    sb.AppendLine($"     - Action: {action.GetType().Name}");

                foreach (var t in state.Transitions)
                    sb.AppendLine($"        → '{state.Name}' --({t.EventName})-> '{t.ToState}'");
            }

            SethPrimeMain.Log.LogInfo(sb.ToString());
        }
    }
}