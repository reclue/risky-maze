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
        private float jumpHeight = 1.5f;
        /// <summary>Значение гравитации</summary>
        private float gravity;

        /// <summary>Находится ли игрок в прыжке</summary>
        private bool isJumping = false;

        /// <summary>Текущий вектор движения игрока<summary>
        private Vector3 currentMovement;
        /// <summary>Текущий вектор движения игрока<summary>
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

        void FixedUpdate() {
            PlayerMove();
            PlayerJump();
            PlayerRotation();
            characterController.Move(currentMovement * Time.deltaTime);
        }
        #endregion



        /// <summary>Передвижение игрока</summary>
        private void PlayerMove() {
            currentSpeed = Input.GetButton(Unchangeable.RUN_INPUT) ? speedRunning : speedWalking;

            float horizontal = Input.GetButton(Unchangeable.HORIZONTAL_INPUT) ? Input.GetAxis(Unchangeable.HORIZONTAL_INPUT) : 0f;
            float vertical = Input.GetButton(Unchangeable.VERTICAL_INPUT) ? Input.GetAxis(Unchangeable.VERTICAL_INPUT) : 0f;

            if (isJumping) {
                currentMovement.y += gravity * Time.deltaTime;
            } else {
                Vector3 directionMovement = new Vector3(horizontal, 0f, vertical);
                directionMovement.Normalize();
                directionMovement *= currentSpeed;

                directionMovement = Vector3.ClampMagnitude(directionMovement, currentSpeed);

                if (IsGrounded()) {
                    directionMovement.y = currentMovement.y;
                } else { // направление движения в случае падения
                    directionMovement.y += currentMovement.y + gravity * Time.deltaTime;
                }

                currentMovement = transform.TransformDirection(directionMovement);
            }//fi isJumping

            // Ограничить скорость падения
            if (!IsGrounded()) {
                if (currentMovement.y < -50f) {
                    currentMovement.y = -50f;
                } else if (currentMovement.y < -15f) {
                    currentMovement.y -= 1f;
                }
            }
        }

        /// <summary>Прыжок игрока</summary>
        private void PlayerJump() {
            if (isJumping) {
                if (IsGrounded()) {
                    isJumping = false;
                    currentMovement.y = 0f;
                }//fi
            } else {
                if (Input.GetButton(Unchangeable.JUMP_INPUT)) {
                    if (IsGrounded()) {
                        currentMovement.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
                        isJumping = true;
                    }//fi
                }//fi
            }//fi isJumping
        }

        /// <summary>Поворот игрока по оси Y относительно угла камеры</summary>
        private void PlayerRotation() {
            transform.rotation = Quaternion.AngleAxis(cameraPlayer.transform.eulerAngles.y, transform.up);
        }


        /// <summary>Касается ли игрок поверхности</summary>
        private bool IsGrounded() {
            return characterController.isGrounded;
        }

    }//class
}//namespace
