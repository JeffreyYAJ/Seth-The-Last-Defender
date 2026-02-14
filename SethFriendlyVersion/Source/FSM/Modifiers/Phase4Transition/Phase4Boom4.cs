using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Phase4Boom4 : StateModifierBase
    {
        public override string BindState => "Phase 4 Boom 4";

        public Phase4Boom4(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Phase 4 Boom 5",
                        ToFsmState = fsm.Fsm.GetState("Phase 4 Boom 5")
                    }
                }
            };

            comboState.Actions = new FsmStateAction[]
            {
                new ExplodeOnPlayerAction(0.2f),
                new Wait() { time = 1.5f, finishEvent = FsmEvent.GetFsmEvent("FINISHED") },
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();

            SethPrimeMain.Log.LogInfo("Etat Phase 4 Boom 4 créé (Explosion 2)");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}
