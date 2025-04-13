using System;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour   
    {
        private PlayerStatesHandler statesHandler;
        private Rigidbody rb;

        [SerializeField] private float maxSpeed;
        [SerializeField] private float maxAngularSpeed;

        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private AnimationCurve deccelerationCurve;
        [SerializeField] private AnimationCurve angularCurve;

        private AnimationCurve inverseAcceleration;
        private AnimationCurve inverseDecceleration;

        private float currentAcceleration;

        private float currentSpeed;
        private float currentAngularSpeed;

        private float accelerationButtonPressedTime;

        private bool isAccelerating;
        private bool fallOffEnergy;
        private float detectedRotation;

        public float maxSpeed1 => maxSpeed;

        public float Speed
        {
            get
            {
                return currentAcceleration * currentSpeed;
            }
            set
            {
                if (value < 0)
                    value = 0;
                currentSpeed = value;
            }
        }

        public float AngularSpeed
        {
            get
            {
                return detectedRotation * currentAngularSpeed * currentAcceleration;
            }
        }


        private void Awake()
        {
            inverseAcceleration = accelerationCurve.Inverse();
            inverseDecceleration = deccelerationCurve.Inverse();

            statesHandler = GetComponent<PlayerStatesHandler>();
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (statesHandler.CurrentState != PlayerStatesHandler.PlayerState.Moving)
                return;

            currentSpeed = Mathf.Lerp(0, maxSpeed, accelerationButtonPressedTime);
            currentAngularSpeed = Mathf.Lerp(0, maxAngularSpeed, accelerationButtonPressedTime);

            SmoothAcceleration();
            if (Physics.Raycast(transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                fallOffEnergy = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ;
                Move();
            }
            else
            {
                rb.constraints = RigidbodyConstraints.None;
                if (!fallOffEnergy)
                {
                    rb.AddForce(Speed * transform.forward, ForceMode.VelocityChange);
                    fallOffEnergy = true;
                }
            }

            if (Physics.Raycast(transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground")))
                Move();
            else if (Physics.Raycast(transform.position, Vector3.up, 0.55f, LayerMask.GetMask("Ground")))
                transform.rotation = Quaternion.Euler(0, 0, 0);

            Rotate();
            transform.rotation = Quaternion.Euler(
                rb.constraints == RigidbodyConstraints.FreezeRotationX ? 0.0f : transform.rotation.eulerAngles.x, 
                rb.constraints == RigidbodyConstraints.FreezeRotationY ? 0.0f: transform.rotation.eulerAngles.y, 
                0.0f
            );
        }

        public void DetectAcceleration(InputAction.CallbackContext context)
        {
            context.action.performed += _ =>
            {
                accelerationButtonPressedTime = inverseAcceleration.Evaluate(accelerationButtonPressedTime);
                isAccelerating = true;
            };
            context.action.canceled += _ =>
            {
                accelerationButtonPressedTime = inverseDecceleration.Evaluate(accelerationButtonPressedTime);
                isAccelerating = false;
            };
        }

        public void DetectRotation(InputAction.CallbackContext context)
        {
            if (context.action.inProgress)
                detectedRotation = context.action.ReadValue<float>();
            else
                detectedRotation = 0;
        }

        private void SmoothAcceleration()
        {
            if (isAccelerating)
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

        private void Move() => transform.Translate(Speed * Time.fixedDeltaTime * Vector3.forward, Space.Self);

        private void Rotate() => transform.Rotate(AngularSpeed * Time.fixedDeltaTime * Vector3.up, Space.World);
    }
}