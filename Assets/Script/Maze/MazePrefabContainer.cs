using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff.Maze {

    /// <summary>
    /// Хранилище префабов дверей, стен, и т.д., из которых будет состоять лабиринт
    /// </summary>
    [RequireComponent(typeof(Maze))]
    public class MazePrefabContainer : MonoBehaviour {

        /// <summary>Список префабов в инспекторе</summary>
        [ContextMenuItem("Сортировать по Id, а затем по имени префаба", "SortByPrefabIdThenByName")]
        [ContextMenuItem("Удалить элементы без префаба", "RemoveItemWithoutPrefab")]
        [SerializeField]
        private MazePrefabItem[] prefabsInspector =
                            new MazePrefabItem[SecondaryFunctions.GetEnumLenght(typeof(MazePrefabID))];

        /// <summary>Список префабов в инспекторе преобразованный в словарь</summary>
        public Dictionary<MazePrefabID, List<GameObject>> prefabs { get; private set; }

        #region Unity events
        void Awake() {
            prefabs = new Dictionary<MazePrefabID, List<GameObject>>();
            UpdateDictionary();
        }
        #endregion

        /// <summary>Обновить <seealso cref="prefabs"/></summary>
        private void UpdateDictionary() {
            foreach (MazePrefabItem prefItem in prefabsInspector) {
                if (prefabs.ContainsKey(prefItem.id)) {
                    if (prefabs[prefItem.id] == null) {
                        prefabs[prefItem.id] = new List<GameObject>();
                    }

                    prefabs[prefItem.id].Add(prefItem.prefab);
                } else {
                    prefabs.Add(prefItem.id, new List<GameObject>() { prefItem.prefab });
                }
            }
        }

        /// <summary>Получить случайный префаб по заданному id</summary>
        public GameObject GetRandomPrefab(MazePrefabID mazePrefabID) {
            if (prefabs[mazePrefabID].Count == 0) return null;

            return prefabs[mazePrefabID][GetRandomNumberPrefab(mazePrefabID)];
        }

        /// <summary>Получить случайный номер префаба в списке по заданному id</summary>
        public int GetRandomNumberPrefab(MazePrefabID mazePrefabID) {
            return Random.Range(0, prefabs[mazePrefabID].Count);
        }


        #region Функции для панели Inspector
        /// <summary>Сортировать в инспекторе список сначала по Id затем по имени префаба</summary>
        private void SortByPrefabIdThenByName() {
            try {
                prefabsInspector = prefabsInspector.OrderBy(x => x.id).ThenBy(x => x.prefab.name).ToArray();
            } catch (System.Exception) { }
        }

        /// <summary>Удалить элементы массива MazePrefabItem[], который не содержит префаб</summary>
        private void RemoveItemWithoutPrefab() {
            try {
                prefabsInspector = prefabsInspector.Where(x => x.prefab != null).ToArray();
            } catch (System.Exception) { }
        }
        #endregion Inspector

    }//class
}//namespace
