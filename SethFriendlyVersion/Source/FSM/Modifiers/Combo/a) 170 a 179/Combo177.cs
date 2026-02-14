using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo177 : StateModifierBase
    {
        public override string BindState => "Combo 177";

        public Combo177(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 178",
                        ToFsmState = fsm.Fsm.GetState("Combo 178")
                    }
                }
            };

            comboState.Actions = new FsmStateAction[]
            {
                new EnableAlignYAction { offsetY = 0f, duration = 0.01f }
            };
            ForceColliderOn(comboState);
            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            SethPrimeMain.Log.LogInfo("Etat Combo 177 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}