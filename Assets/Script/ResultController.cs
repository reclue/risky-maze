using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Скрипт, контролирующий меню на сцене Result
    /// </summary>
    public class ResultController : MonoBehaviour {

        #region Unity Events
        void Start() {
            CursorController.Instance.CursorIsHide = false;
        }
        #endregion

        /// <summary>Выйти в главное меню игры</summary>
        public void OnMainMenuClick() {
            SoundController.Instance.PlayClickButton();
            GameController.Instance.GoToNextScene(Unchangeable.MAIN_MENU_SCENE_NAME);
        }

    }//class
}//namespace
