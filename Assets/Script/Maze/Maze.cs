using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ru.lifanoff.Maze {

    /// <summary>
    /// Скрипт, визуализирующий лабиринт
    /// </summary>
    [RequireComponent(typeof(MazePrefabContainer))]
    public class Maze : MonoBehaviour {
        /// <summary>Размер игрового блока</summary>
        private float chunkSize;

        /// <summary>Минимальный размер лабиринта</summary>
        private const int MIN_SIZE_MAZE = 20;
        /// <summary>Максимальный размер лабиринта</summary>
        private const int MAX_SIZE_MAZE = 40;

        /// <summary>Игровой контроллер</summary>
        private GameController gameController;
        /// <summary>Объект игрока на сцене</summary>
        private GameObject currentPlayer;
        /// <summary>Сгенерированный лабиринт</summary>
        private MazeGenerate mazeStructure;

        /// <summary>Коллайдер для пола</summary>
        private BoxCollider floorBoxCollider;
        /// <summary></summary>
        private MazePrefabContainer mazePrefabContainer;

        #region Unity Engine
        void Awake() {
            // Сгенерировать лабиринт случайного размера
            int mazeSizeX = Random.Range(MIN_SIZE_MAZE, MAX_SIZE_MAZE);
            int mazeSizeY = Random.Range(MIN_SIZE_MAZE, MAX_SIZE_MAZE);
            mazeStructure = new MazeGenerate(mazeSizeX, mazeSizeY);
        }

        void Start() {
            chunkSize = Chunk.CHUNK_SIZE;

            mazePrefabContainer = GetComponent<MazePrefabContainer>();

            currentPlayer = SecondaryFunctions.GetPlayer();

            gameController = GameController.Instance;
            gameController.playerHasKey = false;

            PlacePrefabsOnScene();

            AppendColliderFloor();

            RandomPlayerPosition();
        }
        #endregion

        /// <summary>Разместить префабы на сцене</summary>
        private void PlacePrefabsOnScene() {
            foreach (Chunk chunk in mazeStructure) {
                //PlaceWalls(chunk);
                PlaceFloors(chunk);
                //PlaceExitKey(chunk);
            }
        }

        /// <summary>Разместить префаб пола</summary>
        /// <param name="chunk">Текущий блок лабиринта</param>
        private void PlaceFloors(Chunk chunk) {
            MazePrefabID mazePrefabID = MazePrefabID.FLOOR;
            int numnberRandomPrefab = mazePrefabContainer.GetRandomNumberPrefab(mazePrefabID);

            GameObject cloningPrefab = mazePrefabContainer.prefabs[mazePrefabID][numnberRandomPrefab];

            GameObject gameObjectFloor = Instantiate(cloningPrefab, transform) as GameObject;

            Vector3 newPosition = Vector3.zero;
            newPosition.x = chunk.x * chunkSize + chunkSize / 2f;
            newPosition.z = chunk.y * chunkSize;

            gameObjectFloor.transform.position = newPosition;
            gameObjectFloor.SetActive(true);
        }


        /// <summary>Добавить и настроить общий коллайдер для всех полов лабиринта</summary>
        private void AppendColliderFloor() {
            floorBoxCollider = this.gameObject.AddComponent<BoxCollider>();

            Vector3 newSize = Vector3.zero;
            newSize.x = mazeStructure.sizeX * chunkSize;
            newSize.y = .05f;
            newSize.z = mazeStructure.sizeY * chunkSize;

            Vector3 newCenter = Vector3.zero;
            newCenter.x = newSize.x / 2f;
            newCenter.y = -0.025f;
            newCenter.z = (newSize.z / 2f) - (chunkSize / 2f);


            floorBoxCollider.isTrigger = false;
            floorBoxCollider.size = newSize;
            floorBoxCollider.center = newCenter;
            floorBoxCollider.enabled = true;
        }


        /// <summary>Разместить игрока в центре случайного блока</summary>
        private void RandomPlayerPosition() {
            Chunk chunk = mazeStructure.GetRandomChunk();

            Vector3 newPosition = Vector3.zero;
            newPosition.x = (chunk.x * chunkSize) + (chunkSize / 2f);
            newPosition.y = currentPlayer.transform.position.y + .5f;
            newPosition.z = chunk.y * chunkSize;

            currentPlayer.transform.position = newPosition;


            // Развернуть игрока в сторону, где нет стены
            Vector3 newRotation = Vector3.zero;
            newRotation.x = currentPlayer.transform.rotation.x;
            newRotation.z = currentPlayer.transform.rotation.z;

            if (!chunk.hasRightWall) {
                newRotation.y = 0f;
            } else if (!chunk.hasBottomWall) {
                newRotation.y = 90f;
            } else if (!chunk.hasLeftWall) {
                newRotation.y = 180f;
            } else if (!chunk.hasTopWall) {
                newRotation.y = 270f;
            } else { // Оставить прежний разворот
                newRotation.y = currentPlayer.transform.rotation.y;
            }

            currentPlayer.transform.rotation = Quaternion.Euler(newRotation);
        }

    }//class
}//namespace