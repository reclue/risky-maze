using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ru.lifanoff.Intarface;

namespace ru.lifanoff {

    /// <summary>
    /// Управление отображением перекрестья в центре экрана
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class CrosshairController : MonoBehaviour {

        [Header("Спрайты курсоров")]

        [Tooltip("Курсор по-умолчанию")]
        [SerializeField] private Sprite defaultCrosshair;

        [Tooltip("Курсор, отображаемый при наведении на что-либо, что можно использовать")]
        [SerializeField] private Sprite activeCrosshair;


        // Интервал запуска проверки для отображения нужного курсора
        private float secondsInterval = .12f;

        /// <summary>Центр экрана</summary>
        private Vector2 screenCenter;
        /// <summary>Камера игрока</summary>
        private Camera cameraPlayer;

        /// <summary>Растояние, на котором можно использовать предмет</summary>
        private float distanceRaycast = Unchangeable.DISTANCE_PLAYER_RAYCAST;
        /// <summary>Информация о текущем пересечении<summary>
        private RaycastHit currentHit = new RaycastHit();

        /// <summary>Объект, в который надо разместить <seealso cref="defaultCrosshair"/></summary>
        private Image crosshairImage;


        #region Unity events
        void Start() {
            crosshairImage = GetComponent<Image>();
            crosshairImage.sprite = defaultCrosshair;
            screenCenter = SecondaryFunctions.GetScreenCenter();
            cameraPlayer = SecondaryFunctions.GetCameraPlayer();

            StartCoroutine(CheckCrosshair());
        }
        #endregion


        private IEnumerator CheckCrosshair() {
            while (true) {
                Sprite currentSprite = defaultCrosshair;

                // Получаем луч, который идет из центра экрана
                Ray ray = cameraPlayer.ScreenPointToRay(screenCenter);

                // Бросаем луч; данные о пересечении записываются в переменную currentHit
                Physics.Raycast(ray, out currentHit, distanceRaycast);

                if (currentHit.transform != null) {  // Проверяем, не нажимаем ли в пустоту
                    foreach (MonoBehaviour script in currentHit.transform.GetComponents<MonoBehaviour>()) {
                        if (script != null) {
                            if (script is IUsable) { // Ищем у объекта компоненты реализующие интерфейс IUsable
                                currentSprite = activeCrosshair;
                                break;
                            }//fi
                        }//fi
                    }//hcaerof
                }//fi

                if (crosshairImage.sprite != currentSprite) {
                    crosshairImage.sprite = currentSprite;
                }//fi

                yield return new WaitForSeconds(secondsInterval); // сделать следующую проверку через secondsInterval секунд
            }//fi
        }

    }//class
}//namespace
