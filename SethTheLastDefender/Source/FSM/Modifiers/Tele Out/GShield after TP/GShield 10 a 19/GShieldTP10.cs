using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class GShieldTP10 : StateModifierBase
    {
        public override string BindState => "GShield TP 10";

        public GShieldTP10(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

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
                        ToState = "GShield TP 11",
                        ToFsmState = fsm.Fsm.GetState("GShield TP 11")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();


            var sourceState = fsm.Fsm.GetState("Away");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }

            SethPrimeMain.Log.LogInfo("Etat GShield TP 10 créé");

        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}