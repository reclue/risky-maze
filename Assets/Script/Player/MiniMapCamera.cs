using UnityEngine;

namespace ru.lifanoff.Player {

    /// <summary>
    /// Скрипт управляющий камерой мини-карты
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class MiniMapCamera : MonoBehaviour {
        /// <summary>Текущая позиция камеры</summary>
        private Vector3 currentCameraPosition = Vector3.zero;
        /// <summary>Модуль расстояния от камеры до игрока по оси Y</summary>
        private float posY = 5f;

        /// <summary>Текущая позиция камеры</summary>
        private Vector3 currentCameraRotation = Vector3.zero;
        /// <summary>Постоянный поворот камеры по оси X</summary>
        private float rotX = 90f;

        /// <summary>Объект игрока</summary>
        private GameObject currentPlayer;

        /// <summary>Камера мини-карты</summary>
        private Camera miniMapCamera;
        /// <summary>Объем просмотра ортогональной камеры по-умолчанию</summary>
        private float defaultOrthographicSize;


        #region Unity events
        void Start() {
            InitDefaultOrthographicSize();
            InitCamera();
            currentPlayer = SecondaryFunctions.GetPlayer();
            currentCameraRotation.x = rotX;
        }

        void Update() {
            UpdateCameraPosition();
            UpdateCameraRotation();
            UpdateOrthographicSize();
        }
        #endregion


        /// <summary>
        /// Настройка <seealso cref="defaultOrthographicSize"/> 
        /// в зависимости от уровня сложности игры
        /// </summary>
        private void InitDefaultOrthographicSize() {
            switch (GameController.Instance.difficulMode) {
                case DifficultMode.EASY:
                    defaultOrthographicSize = 10f;
                    break;
                case DifficultMode.MEDIUM:
                    defaultOrthographicSize = 15f;
                    break;
                case DifficultMode.HARD:
                    defaultOrthographicSize = 20f;
                    break;
            }
        }

        /// <summary>Настройка начальных значений камеры для мини-карты</summary>
        private void InitCamera() {
            miniMapCamera = GetComponent<Camera>();
            miniMapCamera.orthographic = true;
            miniMapCamera.orthographicSize = defaultOrthographicSize;
            miniMapCamera.farClipPlane = 20f;
        }

        /// <summary>Обновить позицию камеры</summary>
        private void UpdateCameraPosition() {
            currentCameraPosition.x = currentPlayer.transform.position.x;
            currentCameraPosition.y = currentPlayer.transform.position.y + posY;
            currentCameraPosition.z = currentPlayer.transform.position.z;
            transform.position = currentCameraPosition;
        }

        /// <summary>Обновить угол поворрота камеры</summary>
        private void UpdateCameraRotation() {
            currentCameraRotation.y = currentPlayer.transform.eulerAngles.y;
            currentCameraRotation.z = currentPlayer.transform.eulerAngles.z;
            transform.eulerAngles = currentCameraRotation;
        }

        /// <summary>
        /// Обновить объем просмотра ортогональной камеры
        /// в зависимости от позоции игрока по оси Y
        /// </summary>
        private void UpdateOrthographicSize() {
            float additional = currentPlayer.transform.position.y * 2f;
            miniMapCamera.orthographicSize = defaultOrthographicSize + additional;
        }
    }//class
}//namespace
