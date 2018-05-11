using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace ru.lifanoff.Maze {

    /// <summary>
    /// Скрипт, генерирующий лабиринт
    /// </summary>
    public sealed class MazeGenerate {

        /// <summary>Размер лабиринта по оси X (количество блоков по оси X)</summary>
        public int sizeX { get; private set; }
        /// <summary>Размер лабиринта по оси Y (количество блоков по оси Y)</summary>
        public int sizeY { get; private set; }


        /// <summary>Структура лабиринта</summary>
        public List<List<Chunk>> maze { get; private set; }


        /// <param name="x">Размер лабиринта по оси X (количество блоков по оси X)</param>
        /// <param name="y">Размер лабиринта по оси Y (количество блоков по оси Y)</param>
        public MazeGenerate(int x, int y) {
            sizeX = x;
            sizeY = y;

            Generate();
        }

        /// <summary>Генерация лабиринта</summary>
        private void Generate() {
            InitMaze();
        }

        /// <summary>Инициализация <seealso cref="maze"/></summary>
        private void InitMaze() {
            for (int x = 0; x < sizeX; x++) {
                maze.Add(new List<Chunk>());

                for (int y = 0; y < sizeY; y++) {
                    maze.Last().Add(new Chunk(x, y));
                }
            }
        }

    }//class
}//namespace
