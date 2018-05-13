using System.Collections;
using System.Collections.Generic;

namespace ru.lifanoff.Maze {

    /// <summary>
    /// Класс, описывающий объект стены
    /// </summary>
    public class Wall {

        /// <summary>Сторона расположения стены</summary>
        public Side side { get; private set; }

        /// <summary>Расположен ли в стене выход</summary>
        public bool isExit = false;


        /// <param name="side">Сторона расположения стены</param>
        public Wall(Side side) {
            this.side = side;
        }

    }//class

}//namespace