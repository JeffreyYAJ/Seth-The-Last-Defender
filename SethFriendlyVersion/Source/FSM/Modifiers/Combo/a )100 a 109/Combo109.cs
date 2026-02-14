using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo109 : StateModifierBase
    {
        public override string BindState => "Combo 109";

        public Combo109(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 110",
                        ToFsmState = fsm.Fsm.GetState("Combo 110")
                    }
                }
            };

            comboState.Actions = new FsmStateAction[]
            {
                new Wait { time = 0.15f },

            };

            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            SethPrimeMain.Log.LogInfo("Etat Combo 109 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}