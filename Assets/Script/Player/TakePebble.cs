using UnityEngine;

using ru.lifanoff.Intarface;

namespace ru.lifanoff.Player {
    public class TakePebble : MonoBehaviour, IUsable {

        private DropPebble dropPebble = null;

        public int countPebbles = 1;

        #region Unity event
        void Start() {
            dropPebble = SecondaryFunctions.GetPlayer().GetComponent<DropPebble>();
        }
        #endregion

        public void Use() {
            if (PauseController.isPaused) return;
            if (dropPebble == null) return;

            dropPebble.CountPebbles += countPebbles;

            Destroy(gameObject);
        }
    }
}
