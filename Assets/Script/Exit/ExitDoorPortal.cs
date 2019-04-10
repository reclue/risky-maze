using UnityEngine;

using ru.lifanoff.Intarface;

namespace ru.lifanoff.Exit {

    /// <summary>
    /// Скрипт для двери-выхода
    /// </summary>
    public class ExitDoorPortal : MonoBehaviour, IUsable {
        /// <summary>Сообщение для игрока, если он пытается открыть дверь, не имея ключа</summary>
        private const string NEED_KEY_MESSAGE = "You need the key to open this door.";

        /// <summary>
        /// Реакция на попытку игрока открыть дверь
        /// <para>Реализация метода интерфейса <seealso cref="IUsable"/></para>
        /// </summary>
        public void Use() {
            if (PlayerManager.Instance.hasExitKey) {
                GameController.Instance.GoToNextScene(Unchangeable.RESULT_SCENE_NAME);
            } else {
                PlayerManager.Instance.SendMessageToPlayer(NEED_KEY_MESSAGE);
            }
        }

    }//class
}//namespace
