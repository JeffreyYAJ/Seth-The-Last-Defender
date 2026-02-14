using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class DebutCombo1 : StateModifierBase
    {
        public override string BindState => "Debut Combo 1";

        public DebutCombo1(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Debut Combo 2",
                        ToFsmState = fsm.Fsm.GetState("Debut Combo 2")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();
            
            var sourceState = fsm.Fsm.GetState("Tele Out");
            if (sourceState != null)
            {
                SethFsmController.CloneActions(sourceState, comboState);
            }

            comboState.Actions = comboState.Actions
            .Concat(new FsmStateAction[]
            {
                new DisablePhase4ExplosionsAction(),
                new SetIsInComboAction(wrapper, true),
            })
            .ToArray();

            
            SethPrimeMain.Log.LogInfo("Etat Debut Combo 1 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}