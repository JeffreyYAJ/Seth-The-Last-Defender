using HutongGames.PlayMaker;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo193 : StateModifierBase
    {
        public override string BindState => "Combo 193";

        public Combo193(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
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
                        ToState = "Combo 194",
                        ToFsmState = fsm.Fsm.GetState("Combo 194")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();

            var source = fsm.Fsm.GetState("Slash Dash");
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

            SethPrimeMain.Log.LogInfo("Etat Combo 193 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}