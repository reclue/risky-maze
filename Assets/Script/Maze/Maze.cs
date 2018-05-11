using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ru.lifanoff.Maze {

    /// <summary>
    /// Скрипт, визуализирующий лабиринт
    /// </summary>
    public class Maze : MonoBehaviour {
        /// <summary>Минимальный размер лабиринта</summary>
        private const int MIN_SIZE_MAZE = 20;
        /// <summary>Максимальный размер лабиринта</summary>
        private const int MAX_SIZE_MAZE = 40;


        /// <summary>Манипуляции со структурой лабиринта</summary>
        private MazeGenerate mazeGenerate;

        /// <summary>Игровой контроллер</summary>
        private GameController gameController;


        #region Unity Engine
        private void Awake() {
            // Сгенерировать лабиринт случайного размера
            int mazeSizeX = Random.Range(MIN_SIZE_MAZE, MAX_SIZE_MAZE);
            int mazeSizeY = Random.Range(MIN_SIZE_MAZE, MAX_SIZE_MAZE);
            mazeGenerate = new MazeGenerate(mazeSizeX, mazeSizeY);
        }

        void Start() {
            gameController = GameController.Instance;



        }
        #endregion



    }//class

}//namespace