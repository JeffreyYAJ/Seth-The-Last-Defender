using HutongGames.PlayMaker;
using UnityEngine;

namespace SethPrime.FSM.Actions
{
    [ActionCategory("Seth Prime")]
    public class EnableAlignYAction : FsmStateAction
    {
        public FsmFloat offsetY;
        public FsmFloat duration;

        public override void OnEnter()
        {
            var controller = Owner.GetComponent<AlignYController>();
            if (controller == null)
                controller = Owner.AddComponent<AlignYController>();

            controller.Enable(offsetY.Value, duration.Value);
            Finish(); // 🔥 IMPORTANT
        }
    }
}