using UnityEngine;

using ru.lifanoff.Intarface;

namespace ru.lifanoff.Player {

    /// <summary>
    /// Скрипт для реализации взаимодействия любого вида, котрое возникает 
    /// при нажатии на клавишу, отвечающую за взаимодействие (В Input-меню - это "Use")
    /// </summary>
    public class UseController : MonoBehaviour {

        #region Данные для камеры
        private Camera cameraPlayer; // Камера игрока
        private Vector2 screenCenter;
        #endregion

        // Растояние, на котором можно использовать предмет
        private float distance = Unchangeable.DISTANCE_PLAYER_RAYCAST;
        // Информация о текущем пересечении
        private RaycastHit currentHit = new RaycastHit();

        #region Unity events
        void Start() {
            cameraPlayer = SecondaryFunctions.GetCameraPlayer();
            screenCenter = SecondaryFunctions.GetScreenCenter();
        }

        void Update() => PressUse();
        #endregion


        /// <summary>Реакция на нажатие клавиши "Use"</summary>
        private void PressUse() {
            if (Input.GetButtonDown(Unchangeable.USE_INPUT)) { // Нажимаем клавишу Use (В данном случае - левая кнопка мыши)

                // Получаем луч, который идет из центра экрана
                Ray ray = cameraPlayer.ScreenPointToRay(screenCenter);

                // Бросаем луч; данные о пересечении записываются в переменную currentHit
                Physics.Raycast(ray, out currentHit, distance);

                if (currentHit.transform != null) {  // Проверяем, не нажимаем ли в пустоту

                    foreach (MonoBehaviour script in currentHit.transform.GetComponents<MonoBehaviour>()) {
                        if (script != null) {
                            if (script is IUsable) { // Ищем у объекта компоненты реализующие интерфейс IUsable
                                (script as IUsable).Use();
                            }
                        }//fi
                    }//hcaerof
                }//fi
            }//fi
        }

    }//class

}//namespace