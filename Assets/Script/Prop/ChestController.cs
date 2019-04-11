using System;
using System.Collections;
using UnityEngine;

using ru.lifanoff.Intarface;

namespace ru.lifanoff.Prop {

    public class ChestController : MonoBehaviour, IUsable {
        private static System.Random rnd = new System.Random(Convert.ToInt32(DateTimeOffset.Now.ToUnixTimeSeconds()));

        /// <summary>Нужен ли ключ, чтобы открыть сундук</summary>
        private bool needKey = false;
        /// <summary>Есть ли лут в сундуке</summary>
        private bool hasLoot = false;
        /// <summary>Открыт ли сундук</summary>
        private bool isOpen = false;

        /// <summary>Если лут есть, то какой</summary>
        private GameObject loot = null;

        [Tooltip("Крышка сундука")]
        [SerializeField] private GameObject chestCap = null;
        [Header("Лут")]
        [Tooltip("Позиция спавна лута")]
        [SerializeField] private Transform lootSpawnTransfrom = null;
        [Tooltip("Варианты лута")]
        [SerializeField] private GameObject[] loots = new GameObject[0];


        #region Unity events
        void Start() {
            needKey = rnd.Next(0, 100) < 30;
            hasLoot = rnd.Next(0, 100) < 70;

            if (lootSpawnTransfrom == null) lootSpawnTransfrom = transform;

            if (hasLoot) {
                loot = Instantiate(loots[rnd.Next(0, loots.Length)], lootSpawnTransfrom);
                loot.transform.SetParent(null);
            }
        }
        #endregion


        public void Use() {
            if (isOpen) return;
            if (needKey && !PlayerManager.Instance.hasExitKey) {
                PlayerManager.Instance.SendMessageToPlayer("You need the key!");
                return;
            }

            Destroy(GetComponent<Collider>());
            StartCoroutine(OpenChest());
        }

        private IEnumerator OpenChest() {
            Quaternion targetRotation = 
                Quaternion.Euler(chestCap.transform.localRotation.eulerAngles - new Vector3(80f, 0f, 0f));
            float smooth = 80f;

            while (chestCap.transform.rotation != targetRotation) {
                chestCap.transform.localRotation = Quaternion.RotateTowards(chestCap.transform.localRotation, targetRotation, Time.deltaTime * smooth);
                yield return null;
            }

            Destroy(this);
        }
    }

}