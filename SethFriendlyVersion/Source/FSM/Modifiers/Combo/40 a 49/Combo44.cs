using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo44 : StateModifierBase
    {
        public override string BindState => "Combo 44";

        public Combo44(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 45",
                        ToFsmState = fsm.Fsm.GetState("Combo 45")
                    }
                }
            };


            var sourceState = fsm.Fsm.GetState("Throw Shield");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }
            ForceColliderOn(comboState);
            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            SethPrimeMain.Log.LogInfo("Etat Combo 44 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}