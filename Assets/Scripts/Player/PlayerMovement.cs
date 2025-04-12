using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float angularSpeed;
        //[SerializeField] private float buttonLerpingTine;

        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private AnimationCurve deccelerationCurve;
        [SerializeField] private AnimationCurve angularCurve;

        private AnimationCurve inverseAcceleration;
        private AnimationCurve inverseDecceleration;

        private float currentAcceleration;
        private float currentRotation;

        private float currentSpeed;
        private float currentAngularSpeed;

        private float accelerationButtonPressedTime;
        private float rotationButtonPressedTime;

        private float Acceleration
        {
            get
            {
                return currentAcceleration * currentSpeed * Time.fixedDeltaTime;
            }
        }

        private float AngularSpeed
        {
            get
            {
                return currentRotation * currentAngularSpeed * Time.fixedDeltaTime;
            }
        }

        private void Awake()
        {
            inverseAcceleration = accelerationCurve.Inverse();
            inverseDecceleration = deccelerationCurve.Inverse();
        }

        private void Start()
        {
            GameController.Instance.Input.Player.Acceleration.performed += _ =>
            {
                accelerationButtonPressedTime = inverseAcceleration.Evaluate(accelerationButtonPressedTime);
            };
            GameController.Instance.Input.Player.Acceleration.canceled += _ =>
            {
                accelerationButtonPressedTime = inverseDecceleration.Evaluate(accelerationButtonPressedTime);
            };
        }

        private void FixedUpdate()
        {
            currentSpeed = Mathf.Lerp(0, maxSpeed, accelerationButtonPressedTime);
            currentAngularSpeed = Mathf.Lerp(0, angularSpeed, Mathf.Abs(rotationButtonPressedTime)) * accelerationButtonPressedTime;

            //SmoothAcceleration();
            SmoothRotation();
            if (Physics.Raycast(transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground")))
                Move();
            Rotate();
        }

        public void SmoothAcceleration(CallbackContext context)
        {
            if (GameController.Instance.Input.Player.Acceleration.inProgress)
            {
                currentAcceleration = accelerationCurve.Evaluate(accelerationButtonPressedTime);
                accelerationButtonPressedTime += Time.fixedDeltaTime;
                if (accelerationButtonPressedTime > 1)
                    accelerationButtonPressedTime = 1;
            }
            else
            {
                currentAcceleration = deccelerationCurve.Evaluate(accelerationButtonPressedTime);
                accelerationButtonPressedTime -= Time.fixedDeltaTime;
                if (accelerationButtonPressedTime < 0)
                    accelerationButtonPressedTime = 0;
            }
        }

        private void SmoothRotation()
        {
            float direction = GameController.Instance.Input.Player.Rotation.ReadValue<float>();
            currentRotation = angularCurve.Evaluate(rotationButtonPressedTime);
            if (GameController.Instance.Input.Player.Rotation.inProgress)
            {
                if (direction < 0)
                {
                    rotationButtonPressedTime -= Time.fixedDeltaTime;
                    if (rotationButtonPressedTime < -1)
                        rotationButtonPressedTime = -1;
                }
                else if (direction > 0)
                {
                    rotationButtonPressedTime += Time.fixedDeltaTime;
                    if (rotationButtonPressedTime > 1)
                        rotationButtonPressedTime = 1;
                }
            }
            else
                rotationButtonPressedTime = 0;
        }

        private void Move() => transform.Translate(Acceleration * Vector3.forward, Space.Self);

        private void Rotate() => transform.Rotate(AngularSpeed * Vector3.up, Space.World);
    }
}