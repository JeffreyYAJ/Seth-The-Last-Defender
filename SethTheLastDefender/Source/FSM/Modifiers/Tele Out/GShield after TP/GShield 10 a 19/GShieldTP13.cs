using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class GShieldTP13 : StateModifierBase
    {
        public override string BindState => "GShield TP 13";

        public GShieldTP13(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "GShield TP 14",
                        ToFsmState = fsm.Fsm.GetState("GShield TP 14")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();


            var sourceState = fsm.Fsm.GetState("Throw Tele In");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }

            SethPrimeMain.Log.LogInfo("Etat GShield TP 14 créé");

        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}