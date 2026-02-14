using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo27 : StateModifierBase
    {
        public override string BindState => "Combo 27";

        public Combo27(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 28",
                        ToFsmState = fsm.Fsm.GetState("Combo 28")
                    }
                }
            };

            comboState.Actions = new FsmStateAction[]
            {               
                new Wait { time = 0.15f },
            };
            ForceColliderOn(comboState);
            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            SethPrimeMain.Log.LogInfo("Etat Combo 27 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}