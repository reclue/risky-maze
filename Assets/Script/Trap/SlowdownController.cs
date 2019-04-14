using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ru.lifanoff.Player;

namespace ru.lifanoff.Trap {
    public class SlowdownController : MonoBehaviour {
        void Start() {
            Vector3 newRot = transform.eulerAngles;
            newRot.y = Random.Range(0, 360);
            transform.eulerAngles = newRot;
        }

        void OnTriggerEnter(Collider other) {
            if (other.CompareTag(Unchangeable.PLAYER_TAG)) {
                PlayerManager.Instance.isSlowdown = true;
            }
        }

        void OnTriggerStay(Collider other) {
            if (other.CompareTag(Unchangeable.PLAYER_TAG)) {
                if (!PlayerManager.Instance.isSlowdown) {
                    PlayerManager.Instance.isSlowdown = true;
                }
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.CompareTag(Unchangeable.PLAYER_TAG)) {
                PlayerManager.Instance.isSlowdown = false;
            }
        }
    }
}
