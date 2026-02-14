using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Phase4Boom1 : StateModifierBase
    {
        public override string BindState => "Phase 4 Boom 1";

        public Phase4Boom1(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper) : base(fsm, stunFsm, wrapper)
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
                        ToState = "Phase 4 Boom 2",
                        ToFsmState = fsm.Fsm.GetState("Phase 4 Boom 2")
                    }
                }
            };

            // 🔁 CLONE DES ACTIONS DE "Tele In"
            var sourceState = fsm.Fsm.GetState("Tele Out");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }


            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();



            SethPrimeMain.Log.LogInfo("Etat Phase 4 Boom 1 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}