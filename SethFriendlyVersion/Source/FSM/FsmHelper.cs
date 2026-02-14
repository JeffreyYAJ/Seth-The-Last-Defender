using HutongGames.PlayMaker;
using UnityEngine;

namespace SethPrime
{
    public static class FsmHelper
    {
        public static PlayMakerFSM GetFSM(GameObject obj, string fsmName)
        {
            if (obj == null)
            {
                
                return null;
            }

            foreach (var fsm in obj.GetComponents<PlayMakerFSM>())
            {
                if (fsm.FsmName == fsmName)
                    return fsm;
            }

            
            return null;
        }
    }
}