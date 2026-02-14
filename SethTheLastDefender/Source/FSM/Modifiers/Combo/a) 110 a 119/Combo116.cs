using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo116 : StateModifierBase
    {
        public override string BindState => "Combo 116";

        public Combo116(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 117",
                        ToFsmState = fsm.Fsm.GetState("Combo 117")
                    }
                }
            };
          
            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            var sourceState = fsm.Fsm.GetState("Tele In");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }
            ForceColliderOn(comboState);
            SethPrimeMain.Log.LogInfo("Etat Combo 116 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}