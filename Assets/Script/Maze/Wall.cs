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
        public bool hasExit = false;

        /// <summary>В каком блоке расположена стена</summary>
        public Chunk chunk { get; private set; }

        /// <param name="side">Сторона расположения стены</param>
        /// <param name="chunk">В каком блоке расположена стена</param>
        public Wall(Side side, Chunk chunk) {
            this.side = side;
            this.chunk = chunk;
        }

    }//class

}//namespace