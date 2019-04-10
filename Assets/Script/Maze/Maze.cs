using System;
using UnityEngine;

namespace ru.lifanoff.Maze {

    /// <summary>
    /// Скрипт, визуализирующий лабиринт
    /// </summary>
    [RequireComponent(typeof(MazePrefabContainer))]
    public class Maze : MonoBehaviour {
        private System.Random rnd = new System.Random(Convert.ToInt32(DateTimeOffset.Now.ToUnixTimeSeconds()));

        /// <summary>Размер игрового блока</summary>
        private float chunkSize;

        /// <summary>Объект игрока на сцене</summary>
        private GameObject currentPlayer;
        /// <summary>Сгенерированный лабиринт</summary>
        private MazeGenerate mazeStructure = null;
        /// <summary>Сгенерированный лабиринт</summary>
        public MazeGenerate GetMazeStructure {
            get { return mazeStructure; }
        }

        /// <summary>Коллайдер для пола</summary>
        private BoxCollider floorBoxCollider;
        /// <summary></summary>
        private MazePrefabContainer mazePrefabContainer;

        #region Unity Events
        void Awake() {
            // Сгенерировать лабиринт случайного размера
            int minSizeMaze = GameController.Instance.getMinSizeMaze;
            int maxSizeMaze = GameController.Instance.getMinSizeMaze;
            int mazeSizeX = rnd.Next(minSizeMaze, maxSizeMaze);
            int mazeSizeY = rnd.Next(minSizeMaze, maxSizeMaze);
            mazeStructure = new MazeGenerate(mazeSizeX, mazeSizeY);
        }

        void Start() {
            chunkSize = Chunk.CHUNK_SIZE;

            mazePrefabContainer = GetComponent<MazePrefabContainer>();

            currentPlayer = SecondaryFunctions.GetPlayer();

            PlayerManager.Instance.hasExitKey = false;
            PlayerManager.Instance.canMoving = true;

            PlacePrefabsOnScene();

            AppendColliderFloor();
            
            RandomPlayerPosition();
        }
        #endregion

        /// <summary>Разместить префабы на сцене</summary>
        private void PlacePrefabsOnScene() {
            foreach (Chunk chunk in mazeStructure) {
                PlaceWalls(chunk);
                PlaceFloors(chunk);
                PlaceExitKey(chunk);
            }
        }


        #region PlaceWalls
        /// <summary>Разместить префабы стен</summary>
        /// <param name="chunk">Текущий блок лабиринта</param>
        private void PlaceWalls(Chunk chunk) {
            if (chunk.hasLeftWall) PlaceLeftWalls(chunk);
            if (chunk.hasRightWall) PlaceRightWalls(chunk);
            if (chunk.hasTopWall) PlaceTopWalls(chunk);
            if (chunk.hasBottomWall) PlaceBottomWalls(chunk);
        }

        /// <summary>Разместить префаб левой стены</summary>
        /// <param name="chunk">Текущий блок лабиринта</param>
        private void PlaceLeftWalls(Chunk chunk) {
            Vector3 newPosition = Vector3.zero;
            newPosition.x = chunk.x * chunkSize + chunkSize / 2f;
            newPosition.z = chunk.y * chunkSize - chunkSize / 2f;

            Vector3 newRotation = Vector3.zero;

            GameObject gameObjectWall = PlaceAnyWalls(newPosition, Quaternion.Euler(newRotation));
            if (chunk.leftWall.hasExit) InsertExitDoor(gameObjectWall);
        }

        /// <summary>Разместить префаб правой стены</summary>
        /// <param name="chunk">Текущий блок лабиринта</param>
        private void PlaceRightWalls(Chunk chunk) {
            Vector3 newPosition = Vector3.zero;
            newPosition.x = chunk.x * chunkSize + chunkSize / 2f;
            newPosition.z = chunk.y * chunkSize + chunkSize / 2f;

            Vector3 newRotation = Vector3.zero;
            newRotation.y = 180f;

            GameObject gameObjectWall = PlaceAnyWalls(newPosition, Quaternion.Euler(newRotation));
            if (chunk.rightWall.hasExit) InsertExitDoor(gameObjectWall);
        }

        /// <summary>Разместить префаб верхней стены</summary>
        /// <param name="chunk">Текущий блок лабиринта</param>
        private void PlaceTopWalls(Chunk chunk) {
            Vector3 newPosition = Vector3.zero;
            newPosition.x = chunk.x * chunkSize;
            newPosition.z = chunk.y * chunkSize;

            Vector3 newRotation = Vector3.zero;
            newRotation.y = 90f;

            GameObject gameObjectWall = PlaceAnyWalls(newPosition, Quaternion.Euler(newRotation));
            if (chunk.topWall.hasExit) InsertExitDoor(gameObjectWall);
        }

        /// <summary>Разместить префаб нижней стены</summary>
        /// <param name="chunk">Текущий блок лабиринта</param>
        private void PlaceBottomWalls(Chunk chunk) {
            Vector3 newPosition = Vector3.zero;
            newPosition.x = chunk.x * chunkSize + chunkSize;
            newPosition.z = chunk.y * chunkSize;

            Vector3 newRotation = Vector3.zero;
            newRotation.y = -90f;

            GameObject gameObjectWall = PlaceAnyWalls(newPosition, Quaternion.Euler(newRotation));
            if (chunk.bottomWall.hasExit) InsertExitDoor(gameObjectWall);
        }

        /// <summary>Разместить префаб любой стены</summary>
        /// <param name="wallPosition">Место расположения на поле</param>
        /// <param name="wallRotation">Угол разворота</param>
        /// <returns>Возвращает размещенный на сцене объект стены</returns>
        private GameObject PlaceAnyWalls(Vector3 wallPosition, Quaternion wallRotation) {
            MazePrefabID mazePrefabID = MazePrefabID.WALL;
            int numnberRandomPrefab = mazePrefabContainer.GetRandomNumberPrefab(mazePrefabID);

            GameObject cloningPrefab = mazePrefabContainer.prefabs[mazePrefabID][numnberRandomPrefab];

            GameObject gameObjectWall = Instantiate(cloningPrefab, transform) as GameObject;

            gameObjectWall.transform.position = wallPosition;
            gameObjectWall.transform.rotation = wallRotation;
            gameObjectWall.SetActive(true);

            return gameObjectWall;
        }

        /// <summary>Вставить дверь в объект <paramref name="anyGameObject"/></summary>
        /// <param name="anyGameObject">Объект, в котоый будет помещена дверь</param>
        private void InsertExitDoor(GameObject anyGameObject) {
            MazePrefabID mazePrefabID = MazePrefabID.EXIT_DOOR;
            int numnberRandomPrefab = mazePrefabContainer.GetRandomNumberPrefab(mazePrefabID);

            GameObject cloningPrefab = mazePrefabContainer.prefabs[mazePrefabID][numnberRandomPrefab];

            GameObject gameObjectExitDoor = Instantiate(cloningPrefab, anyGameObject.transform) as GameObject;

            gameObjectExitDoor.SetActive(true);
        }
        #endregion PlaceWalls


        /// <summary>Разместить префабы пола</summary>
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

        /// <summary>Разместить префабы пола</summary>
        /// <param name="chunk">Текущий блок лабиринта</param>
        private void PlaceExitKey(Chunk chunk) {
            if (!chunk.hasExitKey) return;

            MazePrefabID mazePrefabID = MazePrefabID.EXIT_KEY;
            int numnberRandomPrefab = mazePrefabContainer.GetRandomNumberPrefab(mazePrefabID);

            GameObject cloningPrefab = mazePrefabContainer.prefabs[mazePrefabID][numnberRandomPrefab];

            GameObject gameObjectExitKey = Instantiate(cloningPrefab, transform) as GameObject;

            Vector3 newPosition = Vector3.zero;
            newPosition.x = chunk.x * chunkSize + chunkSize / 2f;
            newPosition.z = chunk.y * chunkSize;

            gameObjectExitKey.transform.position = newPosition;

            // Развернуть стенд с ключем в сторону отсутствующей стены
            Vector3 newRotation = Vector3.zero;
            newRotation.x = gameObjectExitKey.transform.eulerAngles.x;
            newRotation.y = gameObjectExitKey.transform.eulerAngles.y;
            newRotation.z = gameObjectExitKey.transform.eulerAngles.z;

            if (!chunk.hasRightWall) {
                newRotation.y -= 90f;
            } else if (!chunk.hasLeftWall) {
                newRotation.y += 90f;
            } else if (!chunk.hasTopWall) {
                newRotation.y += 180f;
            } // в остальных случаях newRotation.y остается прежним

            gameObjectExitKey.transform.eulerAngles = newRotation;


            gameObjectExitKey.SetActive(true);
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
            Chunk chunk = null;

            // Исключить возможность появления игрока в одном блоке с ключем от выхода
            while (chunk == null || chunk.hasExitKey) {
                chunk = mazeStructure.GetRandomChunk();
            }

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
                newRotation.y = -90;
            } else { // Оставить прежний разворот
                newRotation.y = currentPlayer.transform.rotation.y;
            }

            currentPlayer.transform.rotation = Quaternion.Euler(newRotation);
        }

    }//class
}//namespace