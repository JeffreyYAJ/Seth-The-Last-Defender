using HutongGames.PlayMaker;
using UnityEngine;

namespace SethPrime
{
    public class SetGravity2dScaleAction : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;

        [RequiredField]
        public FsmFloat gravityScale;

        private Rigidbody2D rb;

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go != null)
            {
                rb = go.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = gravityScale.Value;
                }
            }

            Finish();
        }
    }
}