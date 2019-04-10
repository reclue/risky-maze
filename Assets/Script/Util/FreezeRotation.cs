using UnityEngine;


namespace ru.lifanoff.Util {

    public class FreezeRotation : MonoBehaviour {

        [SerializeField] private bool freezeX = true;
        [SerializeField] private bool freezeY = true;
        [SerializeField] private bool freezeZ = true;

        private float freezeRotationX = 0f;
        private float freezeRotationY = 0f;
        private float freezeRotationZ = 0f;

        void Start() {
            Vector3 currentRotation = transform.eulerAngles;

            freezeRotationX = currentRotation.x;
            freezeRotationY = currentRotation.y;
            freezeRotationZ = currentRotation.z;
        }

        void Update() {
            if (!freezeX && !freezeY && !freezeZ) return;

            Vector3 newRotation = transform.eulerAngles;

            bool changed = false;
            if (freezeX && newRotation.x != freezeRotationX) {
                newRotation.x = freezeRotationX;
                changed = true;
            }
            if (freezeY && newRotation.y != freezeRotationY) {
                newRotation.y = freezeRotationY;
                changed = true;
            }
            if (freezeZ && newRotation.z != freezeRotationZ) {
                newRotation.z = freezeRotationZ;
                changed = true;
            }

            if (changed) {
                transform.eulerAngles = newRotation;
            }
        }
    }

}
