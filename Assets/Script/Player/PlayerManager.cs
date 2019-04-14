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
        public bool canMoving = true;
        /// <summary>Есть ли у игрока ключ от выхода</summary>
        public bool hasExitKey = false;

        /// <summary>Находится ли игрок на поверхности</summary>
        public bool isGrounded = false;
        /// <summary>Бежит ли игрок</summary>
        public bool isRunning = false;
        /// <summary>В прыжке ли игрок</summary>
        public bool isJumping = false;
        /// <summary>Передвигается ли игрок медленно</summary>
        public bool isSlowdown = false;

        /// <summary>Скорость шага игрока при замедлении</summary>
        public float speedSlowdown = 1f;
        /// <summary>Скорость обычного шага игрока</summary>
        public float speedWalking = 3f;
        /// <summary>Скорость бега игрока</summary>
        public float speedRunning = 7f;

        /// <summary>Высота прыжка</summary>
        public float jumpHeight = 1.15f;
        /// <summary>Значение гравитации</summary>
        public float gravity = 0f;

        /// <summary>Текущая скорость игрока</summary>
        public float currentSpeed = 0f;
        /// <summary>Текущий вектор движения игрока<summary>
        public Vector3 currentMovement;

        /// <summary>Единственный экземпляр класса <seealso cref="PlayerManager"/></summary>
        public static PlayerManager Instance { get; private set; }

        static PlayerManager() {
            if (Instance == null) {
                Instance = new PlayerManager();
            }
        }

        private PlayerManager() {
            gravity = Physics.gravity.y;
            currentMovement = Vector3.zero;

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
