using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff.Player {

    /// <summary>
    /// Управление передвижением игрока
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour {

        /// <summary>Скорость обычного шага игрока</summary>
        private float speedWalking = 3f;
        /// <summary>Скорость бега игрока</summary>
        private float speedRunning = 7f;
        /// <summary>Текущая скорость игрока</summary>
        private float currentSpeed = 0f;

        /// <summary>Высота прыжка</summary>
        private float jumpHeight = 1.15f;
        /// <summary>Значение гравитации</summary>
        private float gravity;

        /// <summary>Находится ли игрок на поверхности</summary>
        private bool isGrounded = false;
        /// <summary>Бежит ли игрок</summary>
        private bool isRunning = false;
        /// <summary>В прыжке ли игрок</summary>
        private bool isJumping = false;

        /// <summary>Текущий вектор движения игрока<summary>
        private Vector3 currentMovement;
        /// <summary>Текущий кватернион игрока для вращения<summary>
        private Quaternion currentRotation;

        /// <summary>Камера игрока</summary>
        private Camera cameraPlayer;
        /// <summary>Компонент CapsuleCollider прикрепленный к игроку</summary>
        private CharacterController characterController;

        #region Unity Events
        void Start() {
            characterController = GetComponent<CharacterController>();
            cameraPlayer = SecondaryFunctions.GetCameraPlayer();

            gravity = Physics.gravity.y;

            currentMovement = Vector3.zero;
            PlayerRotation();
        }

        void Update() {
            isGrounded = IsGrounded();
            isRunning = Input.GetButton(Unchangeable.RUN_INPUT);
            currentSpeed = isRunning ? speedRunning : speedWalking;

            PlayerMove();
            PlayerJump();
            PlayerRotation();

            characterController.Move(currentMovement * Time.deltaTime);
        }
        #endregion


        /// <summary>Передвижение игрока</summary>
        private void PlayerMove() {
            float horizontal = Input.GetButton(Unchangeable.HORIZONTAL_INPUT) ? Input.GetAxis(Unchangeable.HORIZONTAL_INPUT) : 0f;
            float vertical = Input.GetButton(Unchangeable.VERTICAL_INPUT) ? Input.GetAxis(Unchangeable.VERTICAL_INPUT) : 0f;

            Vector3 directionMovement = new Vector3(horizontal, 0f, vertical);

            if (isJumping) {
                AirControl(directionMovement, ref currentMovement);
                currentMovement.y += 2f * gravity * Time.deltaTime;
            } else {
                currentMovement = MakeCurrentMovement(directionMovement);
            }//fi isJumping

            LimitFallingSpeed();
        }


        /// <summary>Прыжок игрока</summary>
        private void PlayerJump() {
            if (isJumping) {
                if (isGrounded) {
                    isJumping = false;
                    currentMovement.y = 0f;
                }//fi
            } else {
                if (Input.GetButton(Unchangeable.JUMP_INPUT)) {
                    if (isGrounded) {
                        currentMovement.y = Mathf.Sqrt(-4f * gravity * jumpHeight);
                        isJumping = true;
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
            directionMovement = directionMovement * currentSpeed;

            if (isGrounded) {
                directionMovement.y = currentMovement.y;
            } else { // направление движения в случае падения
                directionMovement.y += currentMovement.y + gravity * Time.fixedDeltaTime;
            }

            return transform.TransformDirection(directionMovement);
        }

        /// <summary>Ограничить скорость падения</summary>
        private void LimitFallingSpeed() {
            float minLimit = -50f;
            float mediumLimit = -15f;
            float stepFalling = 1f;

            if (!isGrounded) {
                if (currentMovement.y < minLimit) {
                    currentMovement.y = minLimit;
                } else if (currentMovement.y < mediumLimit) {
                    currentMovement.y -= stepFalling;
                }
            }
        }


        /// <summary>Касается ли игрок поверхности</summary>
        private bool IsGrounded() {
            return characterController.isGrounded;
        }

    }//class
}//namespace
