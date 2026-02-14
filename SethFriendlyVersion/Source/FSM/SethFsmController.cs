using HutongGames.PlayMaker;
using SethPrime.FSM.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SethPrime
{
    public class SethFsmController
    {
        private GameObject boss;
        private List<StateModifierBase> activeMods = new List<StateModifierBase>();
        private Dictionary<PlayMakerFSM, bool> appliedToFsm = new Dictionary<PlayMakerFSM, bool>();
        private bool fsmInitialized = false;

        private PlayMakerFSM controlFsm;
        private PlayMakerFSM stunFsm;
        private PlayMakerFSM shieldFsm;

        public PlayMakerFSM ControlFsm => controlFsm;
        public PlayMakerFSM StunFsm => stunFsm;
        public PlayMakerFSM ShieldFsm => shieldFsm;

        public SethFsmController(GameObject boss)
        {
            this.boss = boss;
        }

        public void SetupFSM()
        {
            if (fsmInitialized) return; // empêche double setup
            fsmInitialized = true;

            controlFsm = FsmHelper.GetFSM(boss, "Control");
            if (controlFsm == null)
            {
                SethPrimeMain.Log.LogError("[SETH FSM] Control FSM introuvable !");
                return;
            }

            if (appliedToFsm.TryGetValue(controlFsm, out var already) && already)
                return;

            stunFsm = FsmHelper.GetFSM(boss, "Stun");
            shieldFsm = FsmHelper.GetFSM(boss, "Shield");

            ForceAllCombos();
            ForceDebutCombos();

            var wrapper = boss.GetComponent<SethWrapper>();

            // Création des modifiers
            activeMods = new List<StateModifierBase>()
            {
                new IdleModifier(controlFsm, stunFsm, wrapper),

                // Cas du Jump Chelou

                new EvSlashCheckModifier(controlFsm, stunFsm, wrapper),


                // Cas du cyclone

                new CycloneModifier(controlFsm, stunFsm, wrapper),
                new CycloneModifierP2(controlFsm, stunFsm, wrapper),
                new CycloneModifierP3(controlFsm, stunFsm, wrapper),
                new CycloneModifierP4(controlFsm, stunFsm, wrapper),

                new CycloneFeint(controlFsm, stunFsm, wrapper),

                // Cas du Dive

                new DiveFollowUpModifierP1(controlFsm, stunFsm, wrapper),
                new DiveFollowUpModifierP2(controlFsm, stunFsm, wrapper),
                new DiveFollowUpModifierP3(controlFsm, stunFsm, wrapper),
                new DiveFollowUpModifierP4(controlFsm, stunFsm, wrapper),

                // Cas du lancé de bouclier
                new GShieldThrowFollowUpModifier(controlFsm, stunFsm, wrapper),
                new GShieldThrowFollowUpModifierP2(controlFsm, stunFsm, wrapper),
                new GShieldThrowFollowUpModifierP3(controlFsm, stunFsm, wrapper),
                new GShieldThrowFollowUpModifierP4(controlFsm, stunFsm, wrapper),

                new AShieldThrowFollowUpModifier(controlFsm, stunFsm, wrapper),
                new AShieldThrowFollowUpModifierP2(controlFsm, stunFsm, wrapper),
                new AShieldThrowFollowUpModifierP3(controlFsm, stunFsm, wrapper),
                new AShieldThrowFollowUpModifierP4(controlFsm, stunFsm, wrapper),

                new ShieldThrowFeint(controlFsm, stunFsm, wrapper),

                // Cas du Stab
                new StabModifier(controlFsm, stunFsm, wrapper),
                new StabModifierP2(controlFsm, stunFsm, wrapper),
                new StabModifierP3(controlFsm, stunFsm, wrapper),
                new StabModifierP4(controlFsm, stunFsm, wrapper),

                new StabFeint(controlFsm, stunFsm, wrapper), 

                // Cas du SlashCombo chose a regler ici
                new SlashComboEvadeP1(controlFsm, stunFsm, wrapper),
                new SlashComboEvadeP2(controlFsm, stunFsm, wrapper),
                new SlashComboEvadeP3(controlFsm, stunFsm, wrapper),
                new SlashComboEvadeP4(controlFsm, stunFsm, wrapper),

                new SlashComboFeint(controlFsm, stunFsm, wrapper),

                // Cas du follow up de la TP

                new TeleMoveModifier(controlFsm, stunFsm, wrapper),

                // Cas des Combo
                new Combo105(controlFsm, stunFsm, wrapper),
                new Combo179(controlFsm, stunFsm, wrapper),
                new Combo283(controlFsm, stunFsm, wrapper),
                new Combo348(controlFsm, stunFsm, wrapper),
            };

            foreach (var m in activeMods)
                m.OnCreateModifier();

            foreach (var m in activeMods)
                m.SetupPhase1Modifiers();

            //FIn des transitions
            appliedToFsm[controlFsm] = true;
            SethPrimeMain.Log.LogInfo("[SETH FSM] Initialisation terminée.");
        }

        private void ForceAllCombos()
        {
            var wrapper = boss.GetComponent<SethWrapper>();

            for (int i = 348; i >= 0; i--) // ordre inversé OBLIGATOIRE
            {
                string stateName = $"Combo {i}";
                if (controlFsm.Fsm.GetState(stateName) != null)
                    continue;

                var type = Type.GetType($"SethPrime.Combo{i}");
                if (type == null)
                {
                    SethPrimeMain.Log.LogWarning($"[SETH FSM] Combo{i} introuvable");
                    continue;
                }

                var combo = (StateModifierBase)Activator.CreateInstance(
                    type,
                    controlFsm,
                    stunFsm,
                    wrapper
                );

                combo.OnCreateModifier();
            }
        }

        public void ForceDebutCombos()
        {
            SethPrimeMain.Log.LogInfo($"[SETH FSM] Forcage des combos Debut");
            var wrapper = boss.GetComponent<SethWrapper>();

            // Cas de la Transition P3
            if (controlFsm.Fsm.GetState("Phase 3 Recovering Music") == null)
                new Phase3RecoveringMusic(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 3 Recovering State") == null)
                new Phase3RecoveringState(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 3 Knocked") == null)
                new Phase3KnockedState(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 3 Knocked 01") == null)
                new Phase3KnockedState01(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 3 Knocked 0") == null)
                new Phase3KnockedState0(controlFsm, stunFsm, wrapper).OnCreateModifier();

            // Cas de la Transition P4
            if (controlFsm.Fsm.GetState("Phase 4 Boom 10") == null)
                new Phase4Boom10(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 9") == null)
                new Phase4Boom9(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 8") == null)
                new Phase4Boom8(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 7") == null)
                new Phase4Boom7(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 6") == null)
                new Phase4Boom6(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 5") == null)
                new Phase4Boom5(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 4") == null)
                new Phase4Boom4(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 3") == null)
                new Phase4Boom3(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 2") == null)
                new Phase4Boom2(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Boom 1") == null)
                new Phase4Boom1(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Recovering State") == null)
                new Phase4RecoveringState(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Knocked") == null)
                new Phase4KnockedState(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Knocked 01") == null)
                new Phase4KnockedState01(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Phase 4 Knocked 0") == null)
                new Phase4KnockedState0(controlFsm, stunFsm, wrapper).OnCreateModifier();

            // Cas du Cyclone TP
            if (controlFsm.Fsm.GetState("Cyclone TP 4") == null)
                new CycloneTP4(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Cyclone TP 3") == null)
                new CycloneTP3(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Cyclone TP 2") == null)
                new CycloneTP2(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Cyclone TP 1") == null)
                new CycloneTP1(controlFsm, stunFsm, wrapper).OnCreateModifier();

            // Cas de Stun Combo
            if (controlFsm.Fsm.GetState("Recover Combo 1") == null)
                new RecoverCombo1(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Recover Combo") == null)
                new RecoverCombo(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Stun Combo") == null)
                new StunCombo(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Stun Combo 0") == null)
                new StunCombo0(controlFsm, stunFsm, wrapper).OnCreateModifier();

            // Cas de Debut Combo
            if (controlFsm.Fsm.GetState("Debut Combo 4") == null)
                new DebutCombo4(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Debut Combo 3") == null)
                new DebutCombo3(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Debut Combo 2") == null)
                new DebutCombo2(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Debut Combo 1") == null)
                new DebutCombo1(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Debut Combo 0") == null)
                new DebutCombo0(controlFsm, stunFsm, wrapper).OnCreateModifier();

            // Cas de Stab TP
            if (controlFsm.Fsm.GetState("Stab TP 4") == null)
                new StabTP4(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Stab TP 3") == null)
                new StabTP3(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Stab TP 2") == null)
                new StabTP2(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("Stab TP 1") == null)
                new StabTP1(controlFsm, stunFsm, wrapper).OnCreateModifier();


            // Cas du GShield TP
            if (controlFsm.Fsm.GetState("GShield TP 14") == null)
                new GShieldTP14(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 13") == null)
                new GShieldTP13(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 12") == null)
                new GShieldTP12(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 11") == null)
                new GShieldTP11(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 10") == null)
                new GShieldTP10(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 9") == null)
                new GShieldTP9(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 8") == null)
                new GShieldTP8(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 7") == null)
                new GShieldTP7(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 6") == null)
                new GShieldTP6(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 5") == null)
                new GShieldTP5(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 4") == null)
                new GShieldTP4(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 3") == null)
                new GShieldTP3(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 2") == null)
                new GShieldTP2(controlFsm, stunFsm, wrapper).OnCreateModifier();
            if (controlFsm.Fsm.GetState("GShield TP 1") == null)
                new GShieldTP1(controlFsm, stunFsm, wrapper).OnCreateModifier();
        }

        public void ApplyPhase(int phase)
        {
            if (activeMods == null) return;

            SethPrimeMain.Log.LogInfo($"[SETH FSM] Application des modifiers → Phase {phase}");

            foreach (var m in activeMods)
            {
                switch (phase)
                {
                    case 1: m.SetupPhase1Modifiers(); break;
                    case 2: m.SetupPhase2Modifiers(); break;
                    case 3: m.SetupPhase3Modifiers(); break;
                    case 4: m.SetupPhase4Modifiers(); break;
                    case 5: m.SetupPhase5Modifiers(); break;
                }
            }
        }

        public float GetStateStartTime()
        {
            if (controlFsm == null || activeMods == null) return 0f;

            var currentStateName = controlFsm.Fsm.ActiveStateName;
            var mod = activeMods.FirstOrDefault(m => m.BindState == currentStateName);
            return mod != null ? mod.AnimationStartTime : 0f;
        }

        public static void CloneActions(FsmState source, FsmState target)
        {
            if (source == null || target == null) return;

            var originalActions = source.Actions ?? Array.Empty<FsmStateAction>();
            target.Actions = new FsmStateAction[originalActions.Length];

            for (int i = 0; i < originalActions.Length; i++)
            {
                var action = originalActions[i];

                if (action == null)
                {
                    continue;
                }

                try
                {
                    target.Actions[i] = CloneAction(action);
                }
                catch (Exception e)
                {
                    SethPrimeMain.Log.LogWarning(
                        $"[CloneActions] CLONE FAILED {action.GetType().Name} → fallback\n{e}"
                    );
                    target.Actions[i] = action;
                }
            }
        }

        public static FsmStateAction CloneAction(FsmStateAction originalAction)
        {
            if (originalAction == null) return null;
            var actionsType = originalAction.GetType();
            var actionCopy = (FsmStateAction)Activator.CreateInstance(actionsType);

            var actionFields = actionsType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in actionFields)
            {
                try
                {
                    field.SetValue(actionCopy, field.GetValue(originalAction));
                }
                catch { }
            }

            return actionCopy;
        }
    }
}