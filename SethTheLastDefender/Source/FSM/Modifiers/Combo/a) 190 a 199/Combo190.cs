using HutongGames.PlayMaker;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo190 : StateModifierBase
    {
        public override string BindState => "Combo 190";

        public Combo190(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper) { }

        public override void OnCreateModifier()
        {
            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Actions = new FsmStateAction[]
                {
                    new EnableAlignYAction { offsetY = 0f, duration = 0.01f }
                },
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Combo 191",
                        ToFsmState = fsm.Fsm.GetState("Combo 191")
                    }
                }
            };
            ForceColliderOn(state);
            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();
            SethPrimeMain.Log.LogInfo("Etat Combo 190 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}