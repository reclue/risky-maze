using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace ru.lifanoff.Maze {

    /// <summary>
    /// Скрипт, генерирующий лабиринт
    /// </summary>
    public sealed class MazeGenerate : IEnumerable, IEnumerator {
        /// <summary>Статичная переменная для операций, использующих случайное значение</summary>
        private static Random rnd = new Random();

        /// <summary>Размер лабиринта по оси X (количество блоков по оси X)</summary>
        public int sizeX { get; private set; }
        /// <summary>Размер лабиринта по оси Y (количество блоков по оси Y)</summary>
        public int sizeY { get; private set; }


        /// <summary>Структура лабиринта</summary>
        private List<List<Chunk>> maze;

        /// <summary>Получить доступ к элементу из списка блоков</summary>
        /// <param name="x">Положение блока по X</param>
        /// <param name="y">Положение блока по Y</param>
        /// <returns>Объект класса Chunk</returns>
        public Chunk this[int x, int y] {
            get {
                try {
                    return maze[x][y];
                } catch (ArgumentException) {
                    return null;
                }
            }
            private set {
                try {
                    maze[x][y] = value;
                } catch (ArgumentException) { }
            }
        }

        #region реалирация методов интерфейсов IEnumerable and IEnumerator
        private int enumeratorIndexX = 0;
        private int enumeratorIndexY = -1;

        public IEnumerator GetEnumerator() {
            return this;
        }


        public object Current {
            get { return this[enumeratorIndexX, enumeratorIndexY]; }
        }

        public bool MoveNext() {
            enumeratorIndexY++;

            if (enumeratorIndexY >= sizeY) {
                enumeratorIndexX++;
                enumeratorIndexY = 0;
            }

            if (enumeratorIndexX >= sizeX) {
                Reset();
                return false;
            }

            return true;
        }

        public void Reset() {
            enumeratorIndexX = 0;
            enumeratorIndexY = -1;
        }

        public IEnumerable<Chunk> GetSequenceOfBlocks() {
            foreach (List<Chunk> chunks in maze) {
                foreach (Chunk chunk in chunks) {
                    yield return chunk;
                }
            }
        }
        #endregion IEnumerable and IEnumerator

        /// <param name="x">Размер лабиринта по оси X (количество блоков по оси X)</param>
        /// <param name="y">Размер лабиринта по оси Y (количество блоков по оси Y)</param>
        public MazeGenerate(int x, int y) {
            sizeX = x;
            sizeY = y;
            maze = new List<List<Chunk>>();

            Generate();
        }


        /// <summary>Генерация лабиринта</summary>
        private void Generate() {
            FillMaze();
            AssignNeighborChunks();
            AssignWallsChunks();
            MakeMaze();
        }

        /// <summary>Заполнить <seealso cref="maze"/></summary>
        private void FillMaze() {
            for (int x = 0; x < sizeX; x++) {
                maze.Add(new List<Chunk>());

                for (int y = 0; y < sizeY; y++) {
                    maze.Last().Add(new Chunk(x, y));
                }
            }
        }

        /// <summary>Назначить соседние блоки</summary>
        private void AssignNeighborChunks() {
            foreach (Chunk chunk in this) {
                chunk.leftChunk = this[chunk.x, chunk.y - 1];
                chunk.rightChunk = this[chunk.x, chunk.y + 1];
                chunk.topChunk = this[chunk.x - 1, chunk.y];
                chunk.bottomChunk = this[chunk.x + 1, chunk.y];
            }
        }

        /// <summary>Установить стены во все блоки</summary>
        private void AssignWallsChunks() {
            foreach (Chunk chunk in this) {
                chunk.leftWall = new Wall(Side.LEFT);
                chunk.rightWall = new Wall(Side.RIGHT);
                chunk.topWall = new Wall(Side.TOP);
                chunk.bottomWall = new Wall(Side.BOTTOM);
            }
        }

        #region MakeMaze
        /// <summary>Составить лабиринт</summary>
        private void MakeMaze() {
            HuntAndKill(GetRandomChunk());
        }

        #region HuntAndKill
        /// <summary>Генерация лабиринта по алгоритму Hunt-and-Kill</summary>
        /// <param name="currentChunk">Блок, с которого алгоритм начинает работать</param>
        private void HuntAndKill(Chunk currentChunk) {
            while (true) {
                Walk_HuntAndKill(ref currentChunk);
                Hunt_HuntAndKill(ref currentChunk);

                if (currentChunk == null) break;
            }
        }

        /// <summary>
        /// Пройтись по списку убирая стены между текущим блоком и соседним непосещенным блоком.
        /// </summary>
        /// <param name="currentChunk">Текущий стартовый блок</param>
        private void Walk_HuntAndKill(ref Chunk currentChunk) {
            currentChunk.isChecked = true;
            Walk_RemoveRandomWall(ref currentChunk);
        }


        /// <summary>
        /// "Отловить" непосещенный блок, у которого есть соседний посещенный блок.
        /// Убрать стены между текущим блоком и соседним посещенным блоком.
        /// Если непосещенные блоки не найдены, то алгоритм завершен.
        /// </summary>
        /// <param name="currentChunk">Текущий стартовый блок</param>
        private void Hunt_HuntAndKill(ref Chunk currentChunk) {
            if (currentChunk != null) return;

            foreach (Chunk chunk in this) {
                if (!chunk.isChecked) {
                    if ((chunk.hasLeftChunk && chunk.leftChunk.isChecked) ||
                        (chunk.hasRightChunk && chunk.rightChunk.isChecked) ||
                        (chunk.hasTopChunk && chunk.topChunk.isChecked) ||
                        (chunk.hasBottomChunk && chunk.bottomChunk.isChecked)) {

                        currentChunk = chunk;
                        Hunt_RemoveRandomWall(ref currentChunk);
                        this.Reset();
                        break;
                    }
                }
            }//hcaerof
        }

        /// <summary>Убрать стену между текущим блоком и случайным непосещенным соседним блоком.</summary>
        /// <remarks>Используется только в методе <seealso cref="Walk_HuntAndKill"/></remarks>
        /// <param name="currentChunk">Текущий стартовый блок</param>
        private void Walk_RemoveRandomWall(ref Chunk currentChunk) {
            // Список имеющихся соседних блоков, между которыми можно убрать стены
            List<Side> sides = new List<Side>();

            if (currentChunk.hasLeftChunk && !currentChunk.leftChunk.isChecked) sides.Add(Side.LEFT);
            if (currentChunk.hasRightChunk && !currentChunk.rightChunk.isChecked) sides.Add(Side.RIGHT);
            if (currentChunk.hasTopChunk && !currentChunk.topChunk.isChecked) sides.Add(Side.TOP);
            if (currentChunk.hasBottomChunk && !currentChunk.bottomChunk.isChecked) sides.Add(Side.BOTTOM);

            if (sides.Count == 0) {
                currentChunk = null;
                return;
            }

            // выбрать случайную сторону
            switch (sides[rnd.Next(sides.Count)]) {
                // Убрать стену между текущим блоком и блоком с выбранной стороны,
                // а также, назначить выбранный блок текущим.
                case Side.LEFT:
                    currentChunk.leftWall = null;
                    currentChunk.leftChunk.rightWall = null;
                    currentChunk = currentChunk.leftChunk;
                    break;
                case Side.RIGHT:
                    currentChunk.rightWall = null;
                    currentChunk.rightChunk.leftWall = null;
                    currentChunk = currentChunk.rightChunk;
                    break;
                case Side.TOP:
                    currentChunk.topWall = null;
                    currentChunk.topChunk.bottomWall = null;
                    currentChunk = currentChunk.topChunk;
                    break;
                case Side.BOTTOM:
                    currentChunk.bottomWall = null;
                    currentChunk.bottomChunk.topWall = null;
                    currentChunk = currentChunk.bottomChunk;
                    break;
            }
        }

        /// <summary>Убрать стену между текущим блоком и случайным посещенным соседним блоком.</summary>
        /// <remarks>Используется только в методе <seealso cref="Hunt_HuntAndKill"/></remarks>
        /// <param name="currentChunk">Текущий стартовый блок</param>
        private void Hunt_RemoveRandomWall(ref Chunk currentChunk) {
            // Список имеющихся соседних блоков, между которыми можно убрать стены
            List<Side> sides = new List<Side>();

            if (currentChunk.hasLeftChunk && currentChunk.leftChunk.isChecked) sides.Add(Side.LEFT);
            if (currentChunk.hasRightChunk && currentChunk.rightChunk.isChecked) sides.Add(Side.RIGHT);
            if (currentChunk.hasTopChunk && currentChunk.topChunk.isChecked) sides.Add(Side.TOP);
            if (currentChunk.hasBottomChunk && currentChunk.bottomChunk.isChecked) sides.Add(Side.BOTTOM);

            if (sides.Count == 0) {
                return;
            }

            // выбрать случайную сторону
            switch (sides[rnd.Next(sides.Count)]) {
                // Убрать стену между текущим блоком и блоком с выбранной стороны,
                // а также, назначить выбранный блок текущим.
                case Side.LEFT:
                    currentChunk.leftWall = null;
                    currentChunk.leftChunk.rightWall = null;
                    currentChunk = currentChunk.leftChunk;
                    break;
                case Side.RIGHT:
                    currentChunk.rightWall = null;
                    currentChunk.rightChunk.leftWall = null;
                    currentChunk = currentChunk.rightChunk;
                    break;
                case Side.TOP:
                    currentChunk.topWall = null;
                    currentChunk.topChunk.bottomWall = null;
                    currentChunk = currentChunk.topChunk;
                    break;
                case Side.BOTTOM:
                    currentChunk.bottomWall = null;
                    currentChunk.bottomChunk.leftWall = null;
                    currentChunk = currentChunk.bottomChunk;
                    break;
            }
        }
        #endregion HuntAndKill
        #endregion MakeMaze


        /// <summary>Получить случайный блок из списка</summary>
        public Chunk GetRandomChunk() {
            int x = rnd.Next(sizeX);
            int y = rnd.Next(sizeY);
            return this[x, y];
        }

    }//class
}//namespace
