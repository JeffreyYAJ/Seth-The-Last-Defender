using UnityEngine;

namespace SethPrime
{
    public class AlignYController : MonoBehaviour
    {
        private Transform hero;
        private float offsetY;
        private float timer;
        private float duration;
        private bool active;

        void Awake()
        {
            hero = HeroController.instance?.transform;
        }

        public void Enable(float offset, float time)
        {
            offsetY = offset;
            duration = time;
            timer = 0f;
            active = true;
        }

        public void Disable()
        {
            active = false;
        }

        void Update()
        {
            if (!active || hero == null) return;

            transform.position = new Vector3(
                transform.position.x,
                hero.position.y + offsetY,
                transform.position.z
            );

            if (duration > 0f)
            {
                timer += Time.deltaTime;
                if (timer >= duration)
                    active = false;
            }
        }
    }
}