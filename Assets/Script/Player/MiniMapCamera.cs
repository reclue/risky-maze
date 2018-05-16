using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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


        #region Unity events
        void Start() {
            currentPlayer = SecondaryFunctions.GetPlayer();
            currentCameraRotation.x = rotX;
        }

        void Update() {
            UpdateCameraPosition();
            UpdateCameraRotation();
        }
        #endregion

        /// <summary>Обновить позицию камеры</summary>
        private void UpdateCameraPosition() {
            currentCameraPosition.x = currentPlayer.transform.position.x;
            currentCameraPosition.y = currentPlayer.transform.position.x + posY;
            currentCameraPosition.z = currentPlayer.transform.position.z;
            transform.position = currentCameraPosition;
        }

        /// <summary>Обновить угол поворрота камеры</summary>
        private void UpdateCameraRotation() {
            currentCameraRotation.y = currentPlayer.transform.eulerAngles.y;
            currentCameraRotation.z = currentPlayer.transform.eulerAngles.z;
            transform.eulerAngles = currentCameraRotation;
        }
    }//class
}//namespace
