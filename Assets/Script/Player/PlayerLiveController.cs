using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using ru.lifanoff.Intarface;

namespace ru.lifanoff.Player {

    [RequireComponent(typeof(DropPebble), typeof(UseController))]
    public class PlayerLiveController : MonoBehaviour, ILives {

        [Tooltip("UI отображение количества жизней")]
        [SerializeField] private Text livesText = null;

        /// <summary>Количество жизней</summary>
        private int countLives = Unchangeable.DEFAULT_COUNT_LIVES;
        /// <summary>Количество жизней</summary>
        public int CountLives {
            get {
                return countLives;
            }
            set {
                if (countLives == value) return;

                countLives = Mathf.Clamp(value, 0, int.MaxValue);
                UpdatePebbleText();

                if (countLives < 1) GoToGameOver();
            }
        }

        #region Unity events
        void Start() {
            UpdatePebbleText();
        }
        #endregion

        private void UpdatePebbleText() {
            if (livesText == null) return;
            livesText.text = $"x{countLives}";
        }

        private void GoToGameOver() {
            PlayerManager.Instance.SendMessageToPlayer("You died :(");

            PlayerManager.Instance.canMoving = false;
            GetComponent<DropPebble>().canDrop = false;
            GetComponent<UseController>().canUsing = false;

            StartCoroutine(GoToGameOverScene());
        }

        private IEnumerator GoToGameOverScene() {
            yield return new WaitForSeconds(2.0f);
            GameController.Instance.GoToNextScene(Unchangeable.GAME_OVER_SCENE_NAME);
        }

        public void IncreaseLive() {
            CountLives++;
        }

        public void DecreaseLive() {
            CountLives--;
        }
    }

}
