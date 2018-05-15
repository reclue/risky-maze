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


        /// <summary>Стена, которая содержит выход с уровня</summary>
        public Wall exitWall { get; private set; }
        /// <summary>Есть ли выход с уровня</summary>
        public bool hasExit {
            get { return exitWall != null; }
        }

        /// <summary>Блок, который содержит ключ от выхода с уровня</summary>
        public Chunk exitKeyChunk { get; private set; }
        /// <summary>Есть ли ключ от выхода с уровня</summary>
        public bool hasExitKey {
            get { return exitKeyChunk != null; }
        }


        /// <param name="x">Размер лабиринта по оси X (количество блоков по оси X)</param>
        /// <param name="y">Размер лабиринта по оси Y (количество блоков по оси Y)</param>
        public MazeGenerate(int x, int y) {
            sizeX = x;
            sizeY = y;
            maze = new List<List<Chunk>>();

            Generate();
        }


        #region реалирация методов интерфейсов IEnumerable и IEnumerator
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
        #endregion IEnumerable, IEnumerator


        /// <summary>Генерация лабиринта</summary>
        private void Generate() {
            FillMaze();
            AssignNeighborChunks();
            AssignWallsChunks();
            MakeMaze();
            AddExit();
            AddExitKey();
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
                chunk.leftWall = new Wall(Side.LEFT, chunk);
                chunk.rightWall = new Wall(Side.RIGHT, chunk);
                chunk.topWall = new Wall(Side.TOP, chunk);
                chunk.bottomWall = new Wall(Side.BOTTOM, chunk);
            }
        }

        #region MakeMaze
        /// <summary>Составить лабиринт</summary>
        private void MakeMaze() {
            HuntAndKill(GetRandomChunk());
            ResetChunkIsChecked();
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

            currentChunk = RemoveWall(currentChunk, sides[rnd.Next(sides.Count)]);
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

            currentChunk = RemoveWall(currentChunk, sides[rnd.Next(sides.Count)]);
        }

        /// <summary>
        /// Удалить стену в указанном блоке <paramref name="currentChunk"/>
        /// и вернуть соседний блок, в котором тоже была удалена стена
        /// </summary>
        /// <param name="currentChunk">Блок, в котором будет удалена стена</param>
        /// <param name="side">Сторона, с которой будет удалена стена</param>
        /// <returns>
        /// Соседний блок относительно блока <paramref name="currentChunk"/>, 
        /// в котором была удалена стена
        /// </returns>
        private Chunk RemoveWall(Chunk currentChunk, Side side) {
            Chunk newCurrentChunk = null;

            switch (side) {
                // Убрать стену между текущим блоком и блоком с выбранной стороны,
                // а также, назначить выбранный блок текущим.
                case Side.LEFT:
                    currentChunk.leftWall = null;
                    currentChunk.leftChunk.rightWall = null;
                    newCurrentChunk = currentChunk.leftChunk;
                    break;
                case Side.RIGHT:
                    currentChunk.rightWall = null;
                    currentChunk.rightChunk.leftWall = null;
                    newCurrentChunk = currentChunk.rightChunk;
                    break;
                case Side.TOP:
                    currentChunk.topWall = null;
                    currentChunk.topChunk.bottomWall = null;
                    newCurrentChunk = currentChunk.topChunk;
                    break;
                case Side.BOTTOM:
                    currentChunk.bottomWall = null;
                    currentChunk.bottomChunk.leftWall = null;
                    newCurrentChunk = currentChunk.bottomChunk;
                    break;
            }

            return newCurrentChunk;
        }
        #endregion HuntAndKill
        #endregion MakeMaze


        #region AddExit
        /// <summary>Добавить выход в одну из крайних стен</summary>
        private void AddExit() {
            if (hasExit) return;

            List<Wall> outerWalls = FindOuterWalls();

            exitWall = outerWalls[rnd.Next(outerWalls.Count)];
            exitWall.hasExit = true;
        }

        /// <summary>Найти все внешние стены</summary>
        /// <returns>Список, в который будут сохранены найденные стены</returns>
        private List<Wall> FindOuterWalls() {
            List<Wall> outerWalls = new List<Wall>();

            foreach (Chunk chunk in this) {
                if (chunk.x == 0 || chunk.y == 0) {
                    if (!chunk.hasLeftChunk && chunk.hasLeftWall) outerWalls.Add(chunk.leftWall);
                    if (!chunk.hasRightChunk && chunk.hasRightWall) outerWalls.Add(chunk.rightWall);
                    if (!chunk.hasTopChunk && chunk.hasTopWall) outerWalls.Add(chunk.topWall);
                    if (!chunk.hasBottomChunk && chunk.hasBottomWall) outerWalls.Add(chunk.bottomWall);
                }
            }//hcaerof

            return outerWalls;
        }
        #endregion AddExit


        #region AddExitKey
        /// <summary>Добавить ключ в самый дальний блок относительно блока с выходом</summary>
        private void AddExitKey() {
            if (hasExitKey) return;

            List<Chunk> farthestChunks = FindFarthestChunksFromExit();

            exitKeyChunk = farthestChunks[rnd.Next(farthestChunks.Count)];
            exitKeyChunk.hasExitKey = true;
        }

        /// <summary>Найти все самые дальние блоки относительно блока с выходом</summary>
        /// <returns>Список, в который будут сохранены найденные блоки</returns>
        private List<Chunk> FindFarthestChunksFromExit() {
            List<List<Chunk>> chunkPaths = MakeListPaths(exitWall.chunk);
            ResetChunkIsChecked();
            return GetFarthestChunksFromListPaths(chunkPaths);
        }

        /// <summary>Составить список путей</summary>
        /// <param name="startChunk">Стартовый блок</param>
        /// <param name="chunkPaths">Список, в который созранятся пути</param>
        /// <returns>Список путей</returns>
        private List<List<Chunk>> MakeListPaths(Chunk startChunk) {
            List<List<Chunk>> pathsChunks = new List<List<Chunk>>() { new List<Chunk>() { startChunk } };

            Queue<List<Chunk>> fifoStackChunks = new Queue<List<Chunk>>();
            fifoStackChunks.Enqueue(new List<Chunk>() { startChunk });

            while (fifoStackChunks.Count > 0) {
                List<Chunk> currentChunks = fifoStackChunks.Dequeue().ToList();

                Chunk lastChunk = currentChunks.Last();

                if (lastChunk.isChecked) {
                    continue;
                } else {
                    lastChunk.isChecked = true;
                }//fi

                List<Chunk> neighboringUncheckedChunks = FindNeighboringUncheckedChunks(lastChunk);

                foreach (Chunk neighboringChunk in neighboringUncheckedChunks) {
                    currentChunks.Add(neighboringChunk);
                    pathsChunks.Add(currentChunks);
                    fifoStackChunks.Enqueue(currentChunks.ToList());
                }//hcaerof

            }//elihw

            return pathsChunks;
        }

        /// <summary>Добавить непосещенные соседние блоки, между которыми нет стены</summary>
        /// <param name="currentChunk">Блок, относительно которого ведется поиск</param>
        /// <returns>Список сосеждних блоков относительно блока <paramref name="currentChunk"/></returns>
        private List<Chunk> FindNeighboringUncheckedChunks(Chunk currentChunk) {
            List<Chunk> neighboringUncheckedChunks = new List<Chunk>();

            if (!currentChunk.hasLeftWall &&
                currentChunk.hasLeftChunk &&
                !currentChunk.leftChunk.isChecked) {

                neighboringUncheckedChunks.Add(currentChunk.leftChunk);
            }//if left

            if (!currentChunk.hasRightWall &&
                currentChunk.hasRightChunk &&
                !currentChunk.rightChunk.isChecked) {

                neighboringUncheckedChunks.Add(currentChunk.rightChunk);
            }//if right

            if (!currentChunk.hasTopWall &&
                currentChunk.hasTopChunk &&
                !currentChunk.topChunk.isChecked) {

                neighboringUncheckedChunks.Add(currentChunk.topChunk);
            }//if top

            if (!currentChunk.hasBottomWall &&
                currentChunk.hasBottomChunk &&
                !currentChunk.bottomChunk.isChecked) {

                neighboringUncheckedChunks.Add(currentChunk.bottomChunk);
            }//if bottom

            return neighboringUncheckedChunks;
        }

        /// <summary>Получить дальние блоки из списка путей</summary>
        /// <param name="chunkPaths">Список путей</param>
        /// <returns>Список дальних блоков</returns>
        private List<Chunk> GetFarthestChunksFromListPaths(List<List<Chunk>> chunkPaths) {
            List<Chunk> farthestChunks = chunkPaths.GroupBy(c => c.Count).
                                                    OrderByDescending(g => g.Key).First().
                                                    Select(f => f.Last()).ToList();

            return farthestChunks;
        }
        #endregion AddExitKey


        /// <summary>Получить случайный блок из списка</summary>
        public Chunk GetRandomChunk() {
            int x = rnd.Next(sizeX);
            int y = rnd.Next(sizeY);
            return this[x, y];
        }


        #region Chunk.isChecked
        /// <summary>Сбросить <seealso cref="Chunk.isChecked"/> во всех блоках текущего объекта</summary>
        public void ResetChunkIsChecked() {
            ChunkIsChecked(false);
        }

        /// <summary>
        /// Изменить <seealso cref="Chunk.isChecked"/> во всех блоках текущего объекта,
        /// в соответствии с параметром <paramref name="isChecked"/>.
        /// </summary>
        /// <param name="isChecked">новое значение для всех <seealso cref="Chunk.isChecked"/></param>
        private void ChunkIsChecked(bool isChecked) {
            foreach (Chunk chunk in this) {
                chunk.isChecked = isChecked;
            }
        }
        #endregion Chunk.isChecked

    }//class
}//namespace
