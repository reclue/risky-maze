using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Контроллер для сцены Intro, в которой предварительно проигрываются видеовставки
    /// </summary>
    public class IntroController : MonoBehaviour {

        #region Unity events
        void Start() {
            GameController.Instance.GoToNextScene(Unchangeable.MAIN_MENU_SCENE_NAME);
        }
        #endregion

    }//class
}//namespace
