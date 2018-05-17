using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using ru.lifanoff.Intarface;

namespace ru.lifanoff.Exit {

    /// <summary>
    /// Скрипт для двери-выхода
    /// </summary>
    public class ExitDoorPortal : MonoBehaviour, IUsable {
        /// <summary>Сообщение для игрока, если он пытается открыть дверь, не имея ключа</summary>
        private const string NEED_KEY_MESSAGE = "I need a key to open the door.";

        /// <summary>Текущая корутина</summary>
        private Coroutine currentCoroutine;

        /// <summary>Текстовое поле для отправки сообщений игроку</summary>
        private Text messageToPlayer;


        #region Unity Event
        void Start() {
            messageToPlayer = SecondaryFunctions.GetMessageToPlayer().GetComponent<Text>();
        }
        #endregion

        /// <summary>
        /// Реакция на попытку игрока открыть дверь
        /// <para>Реализация метода интерфейса <seealso cref="IUsable"/></para>
        /// </summary>
        public void Use() {
            if (PlayerManager.Instance.hasExitKey) {
                GameController.Instance.GoToNextScene(Unchangeable.RESULT_SCENE_NAME);
            } else {
                if (currentCoroutine != null) {
                    StopCoroutine(currentCoroutine);
                }

                currentCoroutine = StartCoroutine(SendMessageToPlayer(NEED_KEY_MESSAGE));
            }
        }


        /// <summary>
        /// Отправить сообщение <paramref name="message"/> игроку, 
        /// котрое будет отображаться <paramref name="waitSeconds"/> секунд
        /// </summary>
        /// <param name="message">Сообщение для игрока</param>
        /// <param name="waitSeconds">Время, в течение которого будет отображаться сообщение</param>
        private IEnumerator SendMessageToPlayer(string message, float waitSeconds = 3f) {
            messageToPlayer.text = message;

            yield return new WaitForSeconds(waitSeconds);

            messageToPlayer.text = string.Empty;
        }

    }//class
}//namespace
