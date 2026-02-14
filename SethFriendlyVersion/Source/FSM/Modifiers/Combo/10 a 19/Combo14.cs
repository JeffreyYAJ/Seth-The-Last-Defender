using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;

namespace SethPrime
{
    public class Combo14 : StateModifierBase
    {
        public override string BindState => "Combo 14";

        public Combo14(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {
            // 🔹 Création de l'état Combo3
            var comboState = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Transitions = new[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Combo 15",
                        ToFsmState = fsm.Fsm.GetState("Combo 15")
                    }
                }
            };

            comboState.Actions = new FsmStateAction[]
            {
                new EnableAlignYAction { offsetY = 0f, duration = 0.3f }
            };
            
            fsm.Fsm.States = fsm.Fsm.States.Append(comboState).ToArray();
            ForceColliderOn(comboState);
            SethPrimeMain.Log.LogInfo("Etat Combo 14 créé");
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }
    }
}
