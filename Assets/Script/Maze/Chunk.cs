using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace ru.lifanoff.Maze {

    /// <summary>
    /// Один структурный блок лабиринта
    /// </summary>
    public class Chunk {
        /// <summary>Размер игрового блока</summary>
        public const float CHUNK_SIZE = Unchangeable.CHUNK_SIZE;

        /// <summary>Позиция блока в списке по X</summary>
        public int x { get; private set; }
        /// <summary>Позиция блока в списке по Y</summary>
        public int y { get; private set; }


        /// <param name="posX">Позиция блока в списке по X</param>
        /// <param name="posY">Позиция блока в списке по Y</param>
        public Chunk(int posX, int posY) {
            x = posX;
            y = posY;
        }


        #region Cоседние блоки
        /// <summary>Соседний блок слева от текущего блока</summary>
        public Chunk leftChunk = null;
        /// <summary>Имеет ли текущий блока соседний блок слева</summary>
        public bool hasLeftChunk {
            get { return leftChunk != null; }
        }

        /// <summary>Соседний блок справа от текущего блока</summary>
        public Chunk rightChunk = null;
        /// <summary>Имеет ли текущий блока соседний блок справа</summary>
        public bool hasRightChunk {
            get { return rightChunk != null; }
        }

        /// <summary>Соседний блок сверху от текущего блока</summary>
        public Chunk topChunk = null;
        /// <summary>Имеет ли текущий блока соседний блок сверху</summary>
        public bool hasTopChunk {
            get { return topChunk != null; }
        }

        /// <summary>Соседний блок снизу от текущего блока</summary>
        public Chunk bottomChunk = null;
        /// <summary>Имеет ли текущий блока соседний блок снизу</summary>
        public bool hasBottomChunk {
            get { return bottomChunk != null; }
        }
        #endregion


        #region Стены в блоке
        /// <summary>Левая стена блока</summary>
        public Wall leftWall = null;
        /// <summary>Имеет ли текущий блока стену слева</summary>
        public bool hasLeftWall {
            get { return leftWall != null; }
        }

        /// <summary>Правая стена блока</summary>
        public Wall rightWall = null;
        /// <summary>Имеет ли текущий блока стену справа</summary>
        public bool hasRightWall {
            get { return rightWall != null; }
        }

        /// <summary>Верхняя стена блока</summary>
        public Wall topWall = null;
        /// <summary>Имеет ли текущий блока стену сверху</summary>
        public bool hasTopWall {
            get { return topWall != null; }
        }

        /// <summary>Нижняя стена блока</summary>
        public Wall bottomWall = null;
        /// <summary>Имеет ли текущий блока стену нижнюю</summary>
        public bool hasBottomWall {
            get { return bottomWall != null; }
        }
        #endregion


        /// <summary>Счетчик количества стен в блоке</summary>
        public int wallCounter {
            get {
                int counter = 0;

                if (hasLeftWall) counter++;
                if (hasRightWall) counter++;
                if (hasTopWall) counter++;
                if (hasBottomWall) counter++;

                return counter;
            }
        }


        /// <summary>У блока есть все стены</summary>
        public bool hasAllWalls {
            get {
                return hasLeftWall && hasRightWall && hasTopWall && hasBottomWall;
            }
        }

        /// <summary>У блока нет ни одной стены</summary>
        public bool hasNoOneWalls {
            get {
                return !hasLeftWall && !hasRightWall && !hasTopWall && !hasBottomWall;
            }
        }

        /// <summary>У текущего блока есть все соседние блоки</summary>
        public bool hasAllChunks {
            get {
                return hasLeftChunk && hasRightChunk && hasTopChunk && hasBottomChunk;
            }
        }

        /// <summary>У блока нет ни одного соседнего блока</summary>
        public bool hasNoOneBlocks {
            get {
                return !hasLeftChunk && !hasRightChunk && !hasTopChunk && !hasBottomChunk;
            }
        }


        /// <summary>Расположен ли в блоке ключ от выхода</summary>
        public bool hasExitKey = false;

    }//class
}//namespace

