using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System.Linq;
using UnityEngine;

namespace SethPrime.FSM.Modifiers
{
    public abstract class StateModifierBase
    {
        public abstract string BindState { get; }
        public virtual float AnimationStartTime => 0f;

        protected PlayMakerFSM fsm;
        protected PlayMakerFSM stunFsm;
        protected SethWrapper wrapper;

        protected StateModifierBase(PlayMakerFSM fsm, PlayMakerFSM stunFsm, SethWrapper wrapper)
        {
            this.fsm = fsm;
            this.stunFsm = stunFsm;
            this.wrapper = wrapper;
        }

        // 🔹 Force Collider et Body Damager sur true à l'entrée du state
        protected void ForceColliderOn(FsmState state)
        {
            // 1️⃣ SetCollider comme avant
            var setCollider = new SetCollider
            {
                gameObject = new FsmOwnerDefault { OwnerOption = OwnerDefaultOption.UseOwner },
                active = true
            };

            // 2️⃣ Active Body Damager via action PlayMaker
            var activateBodyDamager = new ActivateBodyDamagerAction
            {
                wrapperGameObject = wrapper.gameObject
            };

            // On injecte les deux actions au début de la liste
            state.Actions = state.Actions == null
                ? new FsmStateAction[] { setCollider, activateBodyDamager }
                : state.Actions.Prepend(setCollider).Prepend(activateBodyDamager).ToArray();

            // ⚡ Réinitialisation des SetCollider clonés (utile pour combos)
            foreach (var action in state.Actions.OfType<SetCollider>())
            {
                action.gameObject = new FsmOwnerDefault { OwnerOption = OwnerDefaultOption.UseOwner };
                action.active = true;
            }
        }

        public abstract void OnCreateModifier();
        public abstract void SetupPhase1Modifiers();
        public abstract void SetupPhase2Modifiers();
        public abstract void SetupPhase3Modifiers();
        public abstract void SetupPhase4Modifiers();
        public abstract void SetupPhase5Modifiers();
    }

    // 🔹 Action PlayMaker pour activer Body Damager à l'entrée du state
    public class ActivateBodyDamagerAction : FsmStateAction
    {
        public GameObject wrapperGameObject;

        public override void OnEnter()
        {
            if (wrapperGameObject != null)
            {
                var bodyDamagers = wrapperGameObject.GetComponentsInChildren<DamageHero>(true)
                    .Where(d => d.name.Contains("Body Damager"));

                foreach (var d in bodyDamagers)
                    d.gameObject.SetActive(true);
            }

            Finish();
        }
    }
}