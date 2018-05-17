using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ru.lifanoff {

    /// <summary>
    /// Настройки игрока во время игрового процесса
    /// </summary>
    public class PlayerManager {

        /// <summary>Текущая корутина</summary>
        private Coroutine currentCoroutine;
        /// <summary>Текстовое поле для отправки сообщений игроку</summary>
        private Text messageToPlayer;


        /// <summary>Можети ли игрок передвигаться</summary>
        public bool canMoving;
        /// <summary>Есть ли у игрока ключ от выхода</summary>
        public bool hasExitKey;

        /// <summary>Единственный экземпляр класса <seealso cref="PlayerManager"/></summary>
        private static PlayerManager instance;
        /// <summary>Единственный экземпляр класса <seealso cref="PlayerManager"/></summary>
        public static PlayerManager Instance {
            get { return instance; }
        }

        static PlayerManager() {
            if (instance == null) {
                instance = new PlayerManager();
            }
        }

        private PlayerManager() {
            canMoving = true;
            hasExitKey = false;

            InitMessageToPlayer();
        }

        /// <summary>Инициализация значения переменной <seealso cref="messageToPlayer"/></summary>
        private void InitMessageToPlayer() {
            if (messageToPlayer == null) {
                messageToPlayer = SecondaryFunctions.GetMessageToPlayer()?.GetComponent<Text>();
            }
        }

        /// <summary>
        /// Отправить сообщение <paramref name="message"/> игроку, 
        /// котрое будет отображаться <paramref name="waitSeconds"/> секунд
        /// </summary>
        /// <param name="message">Сообщение для игрока</param>
        /// <param name="waitSeconds">Время, в течение которого будет отображаться сообщение</param>
        public void SendMessageToPlayer(string message, float waitSeconds = 3f) {
            InitMessageToPlayer();

            if (messageToPlayer == null) return;

            if (currentCoroutine != null) {
                messageToPlayer.StopCoroutine(currentCoroutine);
            }

            currentCoroutine = messageToPlayer.StartCoroutine(SendMessageToPlayerCoroutine(message, waitSeconds));
        }


        /// <summary>
        /// Отправить сообщение <paramref name="message"/> игроку, 
        /// котрое будет отображаться <paramref name="waitSeconds"/> секунд
        /// </summary>
        /// <param name="message">Сообщение для игрока</param>
        /// <param name="waitSeconds">Время, в течение которого будет отображаться сообщение</param>
        private IEnumerator SendMessageToPlayerCoroutine(string message, float waitSeconds) {
            messageToPlayer.text = message;

            yield return new WaitForSeconds(waitSeconds);

            messageToPlayer.text = string.Empty;
        }

    }//class
}//namespace
