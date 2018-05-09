using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Класс-контроллер для сцены Intro
    /// </summary>
    public class IntroController : MonoBehaviour {

        #region Unity events
        void Start() {
            GameController.Instance.GoToNextScene(Unchangeable.MAIN_MENU_SCENE_NAME);
        }
        #endregion

    }//class
}//namespace
