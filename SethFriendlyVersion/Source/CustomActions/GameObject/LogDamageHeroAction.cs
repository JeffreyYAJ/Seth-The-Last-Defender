using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Linq;

namespace SethPrime.FSM.Actions
{
    public class LogDamageHeroAction : FsmStateAction
    {
        public GameObject wrapper;

        public override void OnEnter()
        {
            var damagers = wrapper.GetComponentsInChildren<DamageHero>(true);
            foreach (var d in damagers)
                SethPrimeMain.Log.LogInfo($"[Damager] name={d.name}, active={d.gameObject.activeSelf}");

            Finish();
        }
    }
}
