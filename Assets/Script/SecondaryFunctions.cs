using System;
using System.Linq;
using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Вспомогательные функции
    /// </summary>
    public class SecondaryFunctions {

        /// <summary>Получить вектор положения центра экрана</summary>
        public static Vector2 GetScreenCenter() {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            return new Vector2(x, y);
        }


        /// <summary>Получить основную камеру игрока</summary>
        public static Camera GetCameraPlayer() {
            return GameObject.FindWithTag(Unchangeable.CAMERA_PLAYER_TAG).GetComponent<Camera>();
        }

        /// <summary>Получить GameObject игрока, расположенного на текущей сцене</summary>
        public static GameObject GetPlayer() {
            return GameObject.FindWithTag(Unchangeable.PLAYER_TAG);
        }

        /// <summary>Получить HUD игрока, расположенного на текущей сцене</summary>
        public static GameObject GetPlayerHUD() {
            return GetPlayerChildGameObject(Unchangeable.PLAYER_HUD_TAG);
        }

        /// <summary>Получить игровой объект с текстовым полем для сообщений игроку</summary>
        public static GameObject GetMessageToPlayer() {
            return GetPlayerChildGameObject(Unchangeable.MESSAGE_TO_PLAYER_TAG);
        }

        /// <summary>Получить дочерний GameObject игрока по тегу goTag</summary>
        private static GameObject GetPlayerChildGameObject(string goTag) {
            if (string.IsNullOrWhiteSpace(goTag)) return null;

            GameObject goPlayer = GetPlayer();

            if (goPlayer == null) return null;

            foreach (Transform goTransform in goPlayer.GetComponentsInChildren<Transform>(true)) {
                if (goTransform.CompareTag(goTag)) {
                    return goTransform.gameObject;
                }
            }

            return null;
        }

        /// <summary>Получить размер перечисления enum</summary>
        /// <param name="enumType">typeof([__any_enum__])</param>
        /// <returns>Размер перечисления</returns>
        public static int GetEnumLenght(Type enumType) {
            return Enum.GetValues(enumType).Length;
        }
    }

}