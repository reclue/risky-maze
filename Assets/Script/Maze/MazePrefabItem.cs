using System;
using UnityEngine;

namespace ru.lifanoff.Maze {

    /// <summary>
    /// Класс, описывающий единицу префаба
    /// </summary>
    [Serializable] // Serializable - чтобы можно было настроить через inspector
    public class MazePrefabItem {
        /// <summary>id префаба</summary>
        public MazePrefabID id;
        /// <summary>GameObject соответствующий описанию в id</summary>
        public GameObject prefab;
    }//class
}//namespace
