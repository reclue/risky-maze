using UnityEngine;


namespace ru.lifanoff.Util {
    /// <summary>
    /// Класс, который поворачивает объект по оси y лицевой стороной к камере
    /// </summary>
    public class FaceToScreen : MonoBehaviour {

        private Vector3 targetRotation = new Vector3();
        private Camera playerCamera = null;

        void Start() {
            playerCamera = SecondaryFunctions.GetCameraPlayer();
            targetRotation = playerCamera.transform.eulerAngles;
        }

        void Update() {
            targetRotation = transform.eulerAngles;
            targetRotation.y = playerCamera.transform.eulerAngles.y + 180f;
            transform.eulerAngles = targetRotation;
        }

    }

}
