using UnityEngine;

namespace ru.lifanoff {

    enum FallingMode {
        DESTROY, SPAWN
    }

    /// <summary>
    /// Класс, отвечающий за респаун объекта.
    /// В случае, если объект оказался ниже контрольной точки по оси Y, он будет переброшен в точку респауна
    /// </summary>
    public class FallingBehindWall : MonoBehaviour {

        [Tooltip("Что делать, если предмет пересек допустимую границу")]
        [SerializeField] private FallingMode fallingMode = FallingMode.SPAWN;

        private Vector3 spawnPosition; // Стартовая позиция объекта (респаун)
        private Rigidbody _rb;
        private CharacterController _cc;

        #region Unity Events
        void Start() {
            spawnPosition = transform.position;
            _rb = GetComponent<Rigidbody>();
            _cc = GetComponent<CharacterController>();
        }

        void Update() {
            if (transform.position.y < Unchangeable.RESPAWN_POSITION_Y) {
                if (fallingMode == FallingMode.DESTROY) {
                    Destroy(gameObject);
                } else {
                    if (_rb != null) {
                        _rb.velocity = Vector3.zero;
                        _rb.angularVelocity = Vector3.zero;
                    }//fi

                    if (_cc != null) {
                        _cc.SimpleMove(Vector3.zero);
                    }//fi

                    transform.position = spawnPosition;
                }//fi
            }//fi
        }
        #endregion

    }

}