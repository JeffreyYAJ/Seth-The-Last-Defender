using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;
using UnityEngine;


namespace SethPrime
{
    public class RecoverCombo1 : StateModifierBase
    {

        public override string BindState => "Recover Combo 1";

        public RecoverCombo1(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            CreateBindState();
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }

        private void CreateBindState()
        {
            if (fsm.Fsm.GetState(BindState) != null) return;

            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Actions = new FsmStateAction[]
                {
                    new SetIsInComboAction(wrapper, false),
                },

                Transitions = new[]
                {
                    new FsmTransition()
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Tele Out",
                        ToFsmState = fsm.Fsm.GetState("Tele Out")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();
            SethPrimeMain.Log.LogInfo("[RecoverCombo1] State créé et ajouté au Control FSM : " + BindState);
        }
    }
}
