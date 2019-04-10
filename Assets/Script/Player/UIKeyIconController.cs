using UnityEngine;

namespace ru.lifanoff.Player {

    public class UIKeyIconController : MonoBehaviour {

        [SerializeField] private RectTransform keyIcon = null;

        void Update() {
            if (keyIcon.gameObject.activeSelf != PlayerManager.Instance.hasExitKey) {
                keyIcon.gameObject.SetActive(PlayerManager.Instance.hasExitKey);
            }
        }

    }
}