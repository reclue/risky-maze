using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ru.lifanoff {

    /// <summary>
    /// Класс-синглтон, который хранит и передает данные между сценами.
    /// Этот класс прикреплен к специальному объекту на сцене, который не удаляется при смене сцены.
    /// </summary>
    public class GameController : MonoBehaviour {

        /// <summary>Единственный экземпляр класса <seealso cref="GameController"/></summary>
        public static GameController Instance { get; private set; }

        static GameController() {
            InitDifficultMode();
        }

        private GameController() { }

        /// <summary>Настройки отображения курсора</summary>
        private CursorController cursorController;


        /// <summary>Режим сложности игры</summary>
        public DifficultMode difficulMode = DifficultMode.EASY;
        /// <summary>Размеры лабиринта для различных уровней сложности игры</summary>
        private static Dictionary<DifficultMode, Dictionary<MinMax, int>> sizeMazeDifficult;
        /// <summary>Получить минимальный возможный размер лабиринта</summary>
        public int getMinSizeMaze {
            get { return sizeMazeDifficult[difficulMode][MinMax.MIN]; }
        }
        /// <summary>Получить максимальный возможный размер лабиринта</summary>
        public int getMaxSizeMaze {
            get { return sizeMazeDifficult[difficulMode][MinMax.MAX]; }
        }

        /// <summary>Инициализация словаря <seealso cref="sizeMazeDifficult"/></summary>
        private static void InitDifficultMode() {
            sizeMazeDifficult = new Dictionary<DifficultMode, Dictionary<MinMax, int>>();

            sizeMazeDifficult.Add(DifficultMode.EASY, new Dictionary<MinMax, int>());
            sizeMazeDifficult.Add(DifficultMode.MEDIUM, new Dictionary<MinMax, int>());
            sizeMazeDifficult.Add(DifficultMode.HARD, new Dictionary<MinMax, int>());

            sizeMazeDifficult[DifficultMode.EASY].Add(MinMax.MIN, 7);
            sizeMazeDifficult[DifficultMode.EASY].Add(MinMax.MAX, 12);

            sizeMazeDifficult[DifficultMode.MEDIUM].Add(MinMax.MIN, 15);
            sizeMazeDifficult[DifficultMode.MEDIUM].Add(MinMax.MAX, 25);

            sizeMazeDifficult[DifficultMode.HARD].Add(MinMax.MIN, 28);
            sizeMazeDifficult[DifficultMode.HARD].Add(MinMax.MAX, 40);
        }


        #region Названия предыдущей и следующей сцены для сцены Loader
        /// <summary>Сцена PreLoader</summary>
        private const string PRELOADER_SCENE_NAME = Unchangeable.PRELOADER_SCENE_NAME;

        /// <summary>Название текущей сцены. Также, это будет предыдущая сцена относительно сцены <seealso cref="PRELOADER_SCENE_NAME"/></summary>
        [HideInInspector] public string currentSceneName;
        /// <summary>Название следующей сцены относительно сцены <seealso cref="PRELOADER_SCENE_NAME"/></summary>
        [HideInInspector] public string nextSceneName;
        #endregion


        #region Unity Events
        void Awake() {
            if (Instance == null) {
                cursorController = CursorController.Instance;
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            Instance.Initialize();
        }
        #endregion

        /// <summary>Инициализация специфических настроек</summary>
        private void Initialize() {
            currentSceneName = SceneInformation.GetCurrentSceneName();
            
            // Показать или скрыть курсор на определенных сценах
            if (currentSceneName == Unchangeable.PRELOADER_SCENE_NAME || currentSceneName == Unchangeable.MAIN_MENU_SCENE_NAME) {
                Instance.cursorController.CursorIsHide = false;
            } else {
                Instance.cursorController.CursorIsHide = true;
            }
        }

        /// <summary>Переключение на другую сцену</summary>
        /// <param name="nextScene">Сцена, на которую переключаемся</param>
        public void GoToNextScene(string nextScene) {
            currentSceneName = SceneInformation.GetCurrentSceneName();
            nextSceneName = nextScene;
            SceneManager.LoadScene(PRELOADER_SCENE_NAME);
        }
    }//class
}//namespace