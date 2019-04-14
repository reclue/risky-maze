using UnityEngine;

namespace ru.lifanoff.Player {

    /// <summary>
    /// Управление передвижением игрока
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour {

        /// <summary>Камера игрока</summary>
        private Camera cameraPlayer = null;
        /// <summary>Компонент CapsuleCollider прикрепленный к игроку</summary>
        private CharacterController characterController = null;

        private PlayerManager pm = null;
        private SoundController soundController = null;

        #region Unity Events
        void Start() {
            pm = PlayerManager.Instance;
            soundController = SoundController.Instance;

            characterController = GetComponent<CharacterController>();
            cameraPlayer = SecondaryFunctions.GetCameraPlayer();

            PlayerRotation();
        }

        void Update() {
            if (pm.canMoving) {
                pm.isGrounded = IsGrounded();
                pm.isRunning = Input.GetButton(Unchangeable.RUN_INPUT);

                if (pm.isSlowdown) {
                    pm.currentSpeed = pm.speedSlowdown;
                } else {
                    pm.currentSpeed = pm.isRunning ? pm.speedRunning : pm.speedWalking;
                }

                if (!pm.isJumping) {
                    if (pm.isRunning) {
                        soundController.PlayRuningPlayer();
                    } else {
                        soundController.PlayWalkingPlayer();
                    }
                }

                PlayerMove();
                PlayerJump();
                PlayerRotation();

                if (pm.currentMovement.x == 0 && pm.currentMovement.z == 0) {
                    soundController.StopPlayerAudioSource();
                }

                characterController.Move(pm.currentMovement * Time.deltaTime);
            }
        }
        #endregion


        /// <summary>Передвижение игрока</summary>
        private void PlayerMove() {
            float horizontal = Input.GetButton(Unchangeable.HORIZONTAL_INPUT) ? Input.GetAxis(Unchangeable.HORIZONTAL_INPUT) : 0f;
            float vertical = Input.GetButton(Unchangeable.VERTICAL_INPUT) ? Input.GetAxis(Unchangeable.VERTICAL_INPUT) : 0f;

            Vector3 directionMovement = new Vector3(horizontal, 0f, vertical);

            if (pm.isJumping) {
                AirControl(directionMovement, ref pm.currentMovement);
                pm.currentMovement.y += 2f * pm.gravity * Time.deltaTime;
            } else {
                pm.currentMovement = MakeCurrentMovement(directionMovement);
            }//fi isJumping

            LimitFallingSpeed();
        }


        /// <summary>Прыжок игрока</summary>
        private void PlayerJump() {
            if (pm.isJumping) {
                if (pm.isGrounded) {
                    pm.isJumping = false;
                    pm.currentMovement.y = 0f;
                    soundController.PlayEndJumpPlayer();
                }//fi
            } else {
                if (Input.GetButton(Unchangeable.JUMP_INPUT)) {
                    if (pm.isGrounded) {
                        pm.currentMovement.y = Mathf.Sqrt(-4f * pm.gravity * pm.jumpHeight);
                        pm.isJumping = true;
                        soundController.PlayStartJumpPlayer(40);
                    }//fi
                }//fi
            }//fi isJumping
        }


        /// <summary>Поворот игрока по оси Y относительно угла камеры</summary>
        private void PlayerRotation() {
            transform.rotation = Quaternion.AngleAxis(cameraPlayer.transform.eulerAngles.y, transform.up);
        }


        /// <summary>Контроль направления движения игрока в полете(в падении, прыжке и т.д.)</summary>
        /// <param name="directionMovement">Предлагаемое направаление движения в полете</param>
        /// <param name="currentMovement">Текущее направление движения игрока</param>
        private void AirControl(Vector3 directionMovement, ref Vector3 currentMovement) {
            directionMovement = transform.TransformDirection(directionMovement);
            directionMovement.Normalize();

            // Если currentMovement и directionMovement направлены в разные стороны,
            // параллельны или немного в одну сторону, то надо постепенно корректировать вектор currentMovement
            // в соответствии с directionMovement
            if (Vector3.Dot(currentMovement.normalized, directionMovement) < .1f) {
                currentMovement += directionMovement * 2f;
            }
        }

        /// <summary>Новый вектор движения</summary>
        /// <param name="directionMovement">Предлагаемое направаление движения</param>
        private Vector3 MakeCurrentMovement(Vector3 directionMovement) {
            directionMovement.Normalize();
            directionMovement = directionMovement * pm.currentSpeed;

            if (pm.isGrounded) {
                directionMovement.y = pm.currentMovement.y;
            } else { // направление движения в случае падения
                directionMovement.y += pm.currentMovement.y + pm.gravity * Time.fixedDeltaTime;
            }

            return transform.TransformDirection(directionMovement);
        }

        /// <summary>Ограничить скорость падения</summary>
        private void LimitFallingSpeed() {
            float minLimit = -50f;
            float mediumLimit = -15f;
            float stepFalling = 1f;

            if (!pm.isGrounded) {
                if (pm.currentMovement.y < minLimit) {
                    pm.currentMovement.y = minLimit;
                } else if (pm.currentMovement.y < mediumLimit) {
                    pm.currentMovement.y -= stepFalling;
                }
            }
        }


        /// <summary>Касается ли игрок поверхности</summary>
        private bool IsGrounded() {
            return characterController.isGrounded;
        }

    }//class
}//namespace
