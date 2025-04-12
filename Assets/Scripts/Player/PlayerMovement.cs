using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField] private float angularAcceleration;

        private AnimationCurve inverseAcceleration;
        private AnimationCurve inverseDecceleration;
        private float currentAcceleration;
        private float currentRotation;
        private float buttonPressedTime;

        private float Acceleration
        {
            get
            {
                Debug.Log(currentAcceleration * maxSpeed * Time.fixedDeltaTime);
                return currentAcceleration * maxSpeed * Time.fixedDeltaTime;
            }
        }

        private float AngularSpeed
        {
            get
            {
                return currentRotation * angularSpeed * Time.fixedDeltaTime;
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
                buttonPressedTime = inverseAcceleration.Evaluate(buttonPressedTime);
            };
            GameController.Instance.Input.Player.Acceleration.canceled += _ =>
            {
                buttonPressedTime = deccelerationCurve.Evaluate(buttonPressedTime);
            };
        }

        private void FixedUpdate()
        {
            SmoothAcceleration();
            SmoothRotation();
            if (Physics.Raycast(transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground")))
                Move();
            Rotate();
        }

        private void SmoothAcceleration()
        {
            if (GameController.Instance.Input.Player.Acceleration.inProgress)
            {
                currentAcceleration = accelerationCurve.Evaluate(Mathf.Lerp(0, 1, buttonPressedTime));
                buttonPressedTime += Time.fixedDeltaTime;
                if (buttonPressedTime > 1)
                    buttonPressedTime = 1;
            }
            else
            {
                currentAcceleration = deccelerationCurve.Evaluate(Mathf.Lerp(0, 1, buttonPressedTime));
                buttonPressedTime -= Time.fixedDeltaTime;
                if (buttonPressedTime < 0)
                    buttonPressedTime = 0;
            }
        }

        private void SmoothRotation()
        {
            float rotation = GameController.Instance.Input.Player.Rotation.ReadValue<float>();
        }

        private void Move() => transform.Translate(Acceleration * Vector3.forward, Space.Self);

        private void Rotate()
        {
            float rotation = GameController.Instance.Input.Player.Rotation.ReadValue<float>();
            currentRotation = rotation;
            transform.Rotate(AngularSpeed * Vector3.up, Space.World);
        }
    }
}