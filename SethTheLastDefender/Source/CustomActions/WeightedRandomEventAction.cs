using HutongGames.PlayMaker;
using UnityEngine;

namespace SethPrime.CustomActions
{
    public class WeightedRandomEventAction : FsmStateAction
    {
        public FsmEvent[] events;
        public float[] weights;

        // Optionnel : activer/désactiver les logs
        public bool logResult = true;

        public override void OnEnter()
        {
            if (events == null || weights == null || events.Length == 0 || weights.Length != events.Length)
            {
                Finish();
                return;
            }

            float total = 0f;
            for (int i = 0; i < weights.Length; i++)
                total += Mathf.Max(0f, weights[i]);

            if (total <= 0f)
            {
                Finish();
                return;
            }

            float roll = Random.value * total;
            float cumulative = 0f;

            for (int i = 0; i < events.Length; i++)
            {
                cumulative += Mathf.Max(0f, weights[i]);

                if (roll <= cumulative)
                {
                    if (logResult)
                        LogChoice(i, roll, total);

                    Fsm.Event(events[i]);
                    break;
                }
            }

            Finish();
        }

        private void LogChoice(int index, float roll, float total)
        {
            float weight = weights[index];
            float probability = weight / total;

            string stateName = Fsm.ActiveStateName;
            string eventName = events[index]?.Name ?? "NULL";

            //SethPrimeMain.Log.LogInfo($"[SETH RNG] State=\"{stateName}\" | Event=\"{eventName}\" | " + $"weight={weight} | prob={(probability * 100f):0.#}% | roll={(roll / total):0.###}");

        }
    }
}