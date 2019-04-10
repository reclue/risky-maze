using UnityEngine;
using UnityEngine.UI;

namespace ru.lifanoff.Player {

    [RequireComponent(typeof(PlayerMovement))]
    public class DropPebble : MonoBehaviour {

        [Tooltip("GameObject камня")]
        [SerializeField] private GameObject pebble = null;

        [Tooltip("UI отображение количества камней")]
        [SerializeField] private Text pebbleText = null;

        private Camera cameraPlayer = null;

        public bool canDrop = true;

        /// <summary>Количество камней</summary>
        private int countPebbles = Unchangeable.DEFAULT_COUNT_PEBBLES;
        /// <summary>Количество камней</summary>
        public int CountPebbles {
            get {
                return countPebbles;
            }
            set {
                if (countPebbles == value) return;
                countPebbles = Mathf.Clamp(value, 0, int.MaxValue);
                UpdatePebbleText();
            }
        }

        #region Unity event
        void Start() {
            cameraPlayer = SecondaryFunctions.GetCameraPlayer();
            UpdatePebbleText();
        }

        void Update() => PressDropPebble();
        #endregion

        private void UpdatePebbleText() {
            if (pebbleText == null) return;
            pebbleText.text = $"x{countPebbles}";
        }

        private void PressDropPebble() {
            if (!canDrop || PauseController.isPaused) return;
            if (countPebbles < 1) return;

            if (Input.GetButtonDown(Unchangeable.DROP_PEBBLE_INPUT)) {
                CountPebbles--;
                Drop();
            }
        }

        private void Drop() {
            GameObject newPebble = Instantiate(pebble);
            newPebble.transform.parent = null;

            Rigidbody pebbleRB = newPebble.GetComponent<Rigidbody>();

            if (pebbleRB == null) return;

            Vector3 newPosition = cameraPlayer.transform.position;
            newPosition += cameraPlayer.transform.right / 3f;
            newPosition.y -= .1f;

            newPebble.transform.position = newPosition;

            Vector3 shotObjectCoord = cameraPlayer.transform.forward * Unchangeable.SHOT_FORCE;

            if (Input.GetButton(Unchangeable.RUN_INPUT) && Input.GetAxis(Unchangeable.VERTICAL_INPUT) > 0) {
                shotObjectCoord *= 7f;
            } else {
                shotObjectCoord *= 3f;
            }

            pebbleRB.AddForceAtPosition(shotObjectCoord, transform.position);

            Physics.IgnoreCollision(newPebble.transform.GetComponent<Collider>(), transform.GetComponent<Collider>());
        }

    }
}