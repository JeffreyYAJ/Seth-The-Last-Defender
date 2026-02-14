using HutongGames.PlayMaker;
using SethPrime.Debugging;
using UnityEngine;

namespace SethPrime.FSM.Actions
{
    [ActionCategory("SethPrime/Debug")]

    public class DumpDiveLandAction : FsmStateAction
    {
        // ⚠️ OBLIGATOIRE : déclaré explicitement
        [RequiredField]
        public FsmOwnerDefault target;

        public override void Reset()
        {
            target = new FsmOwnerDefault
            {
                OwnerOption = OwnerDefaultOption.UseOwner
            };
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(target);
            if (go != null && Fsm != null && Fsm.FsmComponent != null)
            {
                FsmImpactDebug.DumpDiveLand(Fsm.FsmComponent, go);
            }

            Finish();
        }
    }
}