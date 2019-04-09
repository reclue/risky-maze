using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Класс-синглтон для управления поведением курсора
    /// </summary>
    public class CursorController {

        public static CursorController Instance { get; private set; }

        static CursorController() {
            if (Instance == null) {
                Instance = new CursorController();
                Instance.CursorIsHide = true;
            }
        }

        private CursorController() { }


        /// <summary>Скрыть\показать курсор</summary>
        private bool cursorIsHide = true;
        /// <summary>Скрыть\показать курсор</summary>
        public bool CursorIsHide {
            get { return cursorIsHide; }
            set {
                cursorIsHide = value;

                if (cursorIsHide) {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    
                } else {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }



    }

}