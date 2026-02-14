using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Linq;

namespace SethPrime.FSM.Actions
{
    [ActionCategory("SethPrime/Debug")]
    public class PhysicsHitboxLogAction : FsmStateAction
    {
        public FsmOwnerDefault Target;
        public string Tag;

        public override void OnEnter()
        {
            var owner = Fsm.GetOwnerDefaultTarget(Target);
            if (owner == null)
            {
                Finish();
                return;
            }

            var rb = owner.GetComponent<Rigidbody2D>();
            var box = owner.GetComponent<BoxCollider2D>();

            string log = $"[PhysicsHitboxLog - {Tag}]\n" +
                         $"-- TRANSFORM --\n" +
                         $"pos={owner.transform.position}\n" +
                         $"rot={owner.transform.rotation.eulerAngles}\n" +
                         $"scale={owner.transform.localScale}\n" +
                         $"-- RIGIDBODY2D --\n" +
                         $"linearVelocity={(rb != null ? rb.linearVelocity : Vector2.zero)}\n" +
                         $"angularVelocity={(rb != null ? rb.angularVelocity : 0f)}\n" +
                         $"gravityScale={(rb != null ? rb.gravityScale : 0f)}\n" +
                         $"bodyType={(rb != null ? rb.bodyType.ToString() : "None")}\n" +
                         $"simulated={(rb != null ? rb.simulated : false)}\n" +
                         $"sleeping={(rb != null ? rb.IsSleeping() : false)}\n" +
                         $"constraints={(rb != null ? rb.constraints.ToString() : "None")}\n" +
                         $"-- BOX COLLIDER --\n" +
                         $"enabled={(box != null ? box.enabled : false)}\n" +
                         $"isTrigger={(box != null ? box.isTrigger : false)}";

            // 🔹 Log des SetCollider dans le state
            foreach (var action in Fsm.ActiveState.Actions)
            {
                if (action is SetCollider sc)
                {
                    log += $"\n[SetCollider] active={sc.active} gameObject={sc.gameObject?.OwnerOption}";
                }
            }

            SethPrimeMain.Log.LogInfo(log);
            Finish();
        }
    }
}