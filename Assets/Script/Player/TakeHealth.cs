using UnityEngine;

using ru.lifanoff.Intarface;

namespace ru.lifanoff.Player {
    public class TakeHealth : MonoBehaviour, IUsable {

        private PlayerLiveController playerLive = null;

        public int countLives = 1;

        #region Unity event
        void Start() {
            playerLive = SecondaryFunctions.GetPlayer().GetComponent<PlayerLiveController>();
        }
        #endregion

        public void Use() {
            if (PauseController.isPaused) return;
            if (playerLive == null) return;

            playerLive.IncreaseLive(countLives);

            Destroy(gameObject);
        }
    }
}
