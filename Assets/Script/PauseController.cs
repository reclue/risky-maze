using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Управление меню "Пауза" в процессе игры
    /// </summary>
    public class PauseController : MonoBehaviour {

        /// <summary>Находится ли игра в режиме паузы</summary>
        [HideInInspector] public bool isPaused { get; private set; }
        /// <summary>Отображаемая панель во время паузы</summary>
        [SerializeField] private RectTransform pausePanel;


        #region Unity events
        void Start() {
            if (pausePanel == null) {
                pausePanel = GetComponentInChildren<RectTransform>();
            }

            isPaused = false;
            pausePanel.gameObject.SetActive(isPaused);
        }

        void Update() {
            if (Input.GetButtonDown(Unchangeable.ESCAPE_INPUT)) {
                PauseSwitcher();
            }
        }
        #endregion

        public void PauseSwitcher() {
            isPaused = !isPaused;

            CursorController.Instance.CursorIsHide = !isPaused;
            PlayerManager.Instance.canMoving = !isPaused;

            pausePanel.gameObject.SetActive(isPaused);

            if (isPaused) {
                Time.timeScale = Unchangeable.PAUSE_TIMESCALE;
            } else {
                Time.timeScale = Unchangeable.DEFAULT_TIMESCALE;
            }

        }

        #region реакция на нажатия кнопок в меню паузы
        /// <summary>Вернуться в игру из меню паузы</summary>
        public void OnResumeClick() {
            PauseSwitcher();
        }

        /// <summary>Выйти в главное меню игры</summary>
        public void OnMainMenuClick() {
            PauseSwitcher();
            GameController.Instance.GoToNextScene(Unchangeable.MAIN_MENU_SCENE_NAME);
        }

        /// <summary>Выти из игры</summary>
        public void OnExitGameClick() {
            Application.Quit();
        }
        #endregion 

    }//class
}//namespace
