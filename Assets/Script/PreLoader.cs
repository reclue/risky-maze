using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ru.lifanoff {

    /// <summary>
    /// Прездагрузка сцен
    /// </summary>
    public class PreLoader : MonoBehaviour {

        // Загружаемая сцена
        private string sceneName;
        /// <summary>Текстовое поле отображающее сколько осталось процентов до конца загрузки сцены"</summary>
        private Text progressText;

        // Контроллер игры, который содержит информацию о предыдущей и следующем уровнях
        private GameController gameController;


        #region Unity Events
        void Start() {
            gameController = GameController.Instance;
            sceneName = gameController.nextSceneName;

            // Если переменная sceneName имеет пустое содержимое или указанной сцены нет в списке активных сцен, 
            // то присвоить sceneName название предыдущей сцены, с которой попали в сцену PreLoader.
            if (string.IsNullOrWhiteSpace(sceneName)) {
                if (!SceneInformation.ActiveNames.Contains(sceneName)) {
                    sceneName = gameController.prevSceneName;
                }
            }

            progressText = GetComponentInChildren<Text>(true);

            StartCoroutine(AsyncSceneLoader());
        }

        #endregion


        /// <summary>
        /// Корутина, которая асинхронно загружает следующую сцену, указанную в sceneName и показывает прогресс загрузки в процентах.
        /// </summary>
        private IEnumerator AsyncSceneLoader() {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!operation.isDone) {
                float progress = operation.progress / .9f; // Делим на .9f чтобы прогресс шел от 0 до 1, а не от 0 до 0.9
                progressText.text = string.Format("{0:P}", progress); // Выводим в виде процентов
                yield return null;
            }
        }
    }//class
}//namespace