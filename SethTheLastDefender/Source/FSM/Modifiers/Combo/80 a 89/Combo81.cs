using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo81 : StateModifierBase
    {
        public override string BindState => "Combo 81";

        public Combo81(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 82",
                        ToFsmState = fsm.Fsm.GetState("Combo 82")
                    }
                }
            };
            
            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            var sourceState = fsm.Fsm.GetState("Slash Combo 5");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }
            ForceColliderOn(comboState);
            SethPrimeMain.Log.LogInfo("Etat Combo 81 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}