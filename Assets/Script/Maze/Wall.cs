using System.Collections;
using System.Collections.Generic;

namespace ru.lifanoff.Maze {

    /// <summary>
    /// Класс, описывающий объект стены
    /// </summary>
    public class Wall {

        /// <summary>Если true, то значит, что эту стену нельзя убрать</summary>
        public bool isStatic { get; private set; }

        /// <summary>Сторона расположения стены</summary>
        public Side side { get; private set; }

        /// <summary>Расположен ли в стене выход</summary>
        public bool isExit = false;


        /// <param name="side">Сторона расположения стены</param>
        /// <param name="isStatic">Можно ли убрать стену</param>
        public Wall(Side side, bool isStatic = false) {
            this.side = side;
            this.isStatic = isStatic;
        }

    }//class

}//namespace