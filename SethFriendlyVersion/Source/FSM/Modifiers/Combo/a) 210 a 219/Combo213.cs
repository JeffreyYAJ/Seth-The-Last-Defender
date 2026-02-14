using HutongGames.PlayMaker;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo213 : StateModifierBase
    {
        public override string BindState => "Combo 213";

        public Combo213(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 214",
                        ToFsmState = fsm.Fsm.GetState("Combo 214")
                    }
                }
            };
      
            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState("Slash Combo 8");
            if (source != null)
                SethFsmController.CloneActions(source, state);

            ForceColliderOn(state);
            state.Actions = state.Actions.Concat(new FsmStateAction[]
            {
                 new SetGravity2dScaleAction()
                    {
                        gameObject = new FsmOwnerDefault
                        {
                            OwnerOption = OwnerDefaultOption.UseOwner
                        },
                        gravityScale = 0f
                    },
            }).ToArray();
            SethPrimeMain.Log.LogInfo("Etat Combo 213 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}