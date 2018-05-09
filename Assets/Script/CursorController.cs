using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Класс-синглтон для управления поведением курсора
    /// </summary>
    public class CursorController {

        private static CursorController instance = null;
        public static CursorController Instance {
            get { return instance; }
        }

        static CursorController() {
            if (instance == null) {
                instance = new CursorController();
                instance.CursorIsHide = true;
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