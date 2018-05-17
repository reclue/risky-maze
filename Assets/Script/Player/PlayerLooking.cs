using UnityEngine;

namespace ru.lifanoff.Player {
    /// <summary>
    /// Скрипт для управления поворотомкамеры игрока
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class PlayerLooking : MonoBehaviour {

        /// <summary>Чувствительность поворота мыши по-горизонтали</summary>
        private float horizontalSensitivity = 2f;
        /// <summary>Чувствительность поворота мыши по-вертикали</summary>
        private float verticalSensitivity = 2f;

        /// <summary>Максимальный угол при повороте камеры вверх</summary>
        private float maxAngleUp = -55f;
        /// <summary>Максимальный угол при повороте камеры вниз</summary>
        private float maxAngleDown = 60f;

        /// <summary>Угол, на который надо повернуть камеру по-горизонтали</summary>
        private float horizontalAngle;
        /// <summary>Угол, на который надо повернуть камеру по-вертикали</summary>
        private float verticalAngle;


        #region Unity events
        void Start() {
            horizontalSensitivity = SaveManager.Instance.optionsManager.controlOptions.mouseSensitivityX;
            verticalSensitivity = SaveManager.Instance.optionsManager.controlOptions.mouseSensitivityY;
        }

        void LateUpdate() {
            if (PlayerManager.Instance.canMoving) {
                SimpleMouseLook();
            }
        }
        #endregion


        /// <summary>Простой поворот камеры</summary>
        private void SimpleMouseLook() {
            horizontalAngle = Input.GetAxis(Unchangeable.MOUSE_X_INPUT) * horizontalSensitivity * Time.timeScale;
            verticalAngle -= Input.GetAxis(Unchangeable.MOUSE_Y_INPUT) * verticalSensitivity * Time.timeScale;

            // Ограничиваем verticalAngle диапазоном от maxAngleUp до maxAngleDown
            verticalAngle = Mathf.Clamp(verticalAngle, maxAngleUp, maxAngleDown);

            transform.localRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
        }

    }//class
}//namespace