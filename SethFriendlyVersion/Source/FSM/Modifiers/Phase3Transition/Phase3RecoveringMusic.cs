using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SethPrime.FSM.Modifiers;
using System.Linq;
using UnityEngine;


namespace SethPrime
{
    public class Phase3RecoveringMusic : StateModifierBase
    {

        public override string BindState => "Phase 3 Recovering Music";

        public Phase3RecoveringMusic(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
            : base(fsm, stunFsm, wrapper)
        {
        }

        public override void OnCreateModifier()
        {          
            CreateBindState();
        }

        public override void SetupPhase1Modifiers() { }
        public override void SetupPhase2Modifiers() { }
        public override void SetupPhase3Modifiers() { }
        public override void SetupPhase4Modifiers() { }
        public override void SetupPhase5Modifiers() { }

        private void CreateBindState()
        {
            if (fsm.Fsm.GetState(BindState) != null) return;

            var state = new FsmState(fsm.Fsm)
            {
                Name = BindState,
                Actions = new FsmStateAction[]
                {
                    // 🔹 Définir IsInP3Custom = true juste avant de reprendre la musique
                    new SetIsInP3CustomAction() { Value = true },

                    new ForceCustomMusicAction(),
                    // 🔹 Reprendre la musique, SethAudioHandler remplacera automatiquement Petals v2 par P3 Custom
                    new PauseBossMusicAction() { Pause = false },

                },

                Transitions = new[]
                {
                    new FsmTransition()
                    {                      
                        FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                        ToState = "Rage Roar Antic",
                        ToFsmState = fsm.Fsm.GetState("Rage Roar Antic")
                    }
                }
            };

            fsm.Fsm.States = fsm.Fsm.States.Append(state).ToArray();
            SethPrimeMain.Log.LogInfo("[Phase3RecoveringMusic] State créé et ajouté au Control FSM : " + BindState);
        }
    }

}