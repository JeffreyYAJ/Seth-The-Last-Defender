using System.Collections;
using UnityEngine;

namespace SethPrime
{
    public class SethImpactRuntime : MonoBehaviour
    {
        public void FreezeAndReattach(
            Transform t,
            Transform parent,
            Vector3 localPos,
            Quaternion localRot,
            Vector3 localScale,
            float delay
        )
        {
            StartCoroutine(ReattachRoutine(t, parent, localPos, localRot, localScale, delay));
        }

        private IEnumerator ReattachRoutine(
            Transform t,
            Transform parent,
            Vector3 localPos,
            Quaternion localRot,
            Vector3 localScale,
            float delay
        )
        {
            yield return new WaitForSeconds(delay);

            if (!t) yield break;

            t.SetParent(parent, false);
            t.localPosition = localPos;
            t.localRotation = localRot;
            t.localScale = localScale;
        }
    }
}