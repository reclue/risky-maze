using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ru.lifanoff {

    /// <summary>
    /// Класс-синглтон, который хранит и передает данные между сценами.
    /// Этот класс прикреплен к специальному объекту на сцене, который никогда не удаляется.
    /// Специальный объект, в свою очередь, входит в состав префаба, который размещается на каждой сцене.
    /// </summary>
    public class GameController : MonoBehaviour {

        /// <summary>Единственный экземпляр класса <seealso cref="GameController"/></summary>
        private static GameController instance;
        /// <summary>Единственный экземпляр класса <seealso cref="GameController"/></summary>
        public static GameController Instance {
            get { return instance; }
        }

        private GameController() { }

        /// <summary>Настройки отображения курсора</summary>
        private CursorController cursorController;


        #region Названия предыдущей и следующей сцены для сцены Loader
        /// <summary>Название предыдущей сцены относительно сцены <seealso cref="PRELOADER_SCENE_NAME"/></summary>
        private string _prevSceneName;
        /// <summary>Название предыдущей сцены относительно сцены <seealso cref="PRELOADER_SCENE_NAME"/></summary>
        public string prevSceneName {
            get { return _prevSceneName; }
            set { _prevSceneName = value; }
        }

        /// <summary>Название следующей сцены относительно сцены <seealso cref="PRELOADER_SCENE_NAME"/></summary>
        private string _nextSceneName;
        /// <summary>Название следующей сцены относительно сцены <seealso cref="PRELOADER_SCENE_NAME"/></summary>
        public string nextSceneName {
            get { return _nextSceneName; }
            set { _nextSceneName = value; }
        }

        /// <summary>Сцена PreLoader</summary>
        private const string PRELOADER_SCENE_NAME = Unchangeable.PRELOADER_SCENE_NAME;
        #endregion


        #region Unity Events
        void Awake() {
            if (instance == null) {
                cursorController = CursorController.Instance;
                instance = this;
            } else if (instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            instance.Initialize();
        }
        #endregion

        /// <summary>Инициализация специфических настроек</summary>
        private void Initialize() {
            _prevSceneName = SceneInformation.GetCurrentSceneName();

            // Показать или скрыть курсор на определенных сценах
            if (_prevSceneName == Unchangeable.PRELOADER_SCENE_NAME || _prevSceneName == Unchangeable.MAIN_MENU_SCENE_NAME) {
                instance.cursorController.CursorIsHide = false;
            } else {
                instance.cursorController.CursorIsHide = true;
            }
        }

        /// <summary>Переключение на другую сцену</summary>
        /// <param name="nextScene">Сцена, на которую переключаемся</param>
        public void GoToNextScene(string nextScene) {
            _prevSceneName = SceneInformation.GetCurrentSceneName();
            _nextSceneName = nextScene;
            SceneManager.LoadScene(PRELOADER_SCENE_NAME);
        }
    }//class
}//namespace