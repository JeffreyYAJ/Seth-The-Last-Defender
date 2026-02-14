using HutongGames.PlayMaker;

namespace SethPrime.FSM.Actions
{
    public class SetIsInComboAction : FsmStateAction
    {
        private readonly SethWrapper wrapper;
        private readonly bool value;

        public SetIsInComboAction(SethWrapper wrapper, bool value)
        {
            this.wrapper = wrapper;
            this.value = value;
        }

        public override void OnEnter()
        {
            if (wrapper == null)
            {
                SethPrimeMain.Log.LogWarning("[SethPrime][Action] SetIsInComboAction → wrapper est NULL !");
                Finish();
                return;
            }

            wrapper.IsInCombo = value;

            SethPrimeMain.Log.LogInfo(
                $"[SethPrime][Action] 🔥 IsInCombo mis à {(value ? "TRUE" : "FALSE")} (State={State?.Name})"
            );

            Finish();
        }
    }
}