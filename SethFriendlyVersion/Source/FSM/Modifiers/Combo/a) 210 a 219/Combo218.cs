using HutongGames.PlayMaker;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo218 : StateModifierBase
    {
        public override string BindState => "Combo 218";

        public Combo218(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Combo 219",
                        ToFsmState = fsm.Fsm.GetState("Combo 219")
                    }
                }
            };

            state.Actions = new FsmStateAction[]
            {
                new EnableAlignYAction { offsetY = 1f, duration = 0.01f }
            };
  
            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            SethPrimeMain.Log.LogInfo("Etat Combo 218 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}