using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class DebutCombo3 : StateModifierBase
    {
        public override string BindState => "Debut Combo 3";

        public DebutCombo3(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            var comboState = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Debut Combo 4",
                        ToFsmState = fsm.Fsm.GetState("Debut Combo 4")
                    }
                }
            };

            comboState.Actions = new FsmStateAction[]
            {

                // 1️⃣ Roar immédiat
                new PlayClipAction{Clip = wrapper.SethRoarAudio, Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value},

                new Wait{time = 2f, finishEvent = FsmEvent.GetFsmEvent("FINISHED")},

            };

            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            SethPrimeMain.Log.LogInfo("Etat Debut Combo 3 créé");
        }
     

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}