using ExtensionMethods;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour   
    {
        private PlayerStatesHandler statesHandler;

        [SerializeField] private float maxSpeed;
        [SerializeField] private float angularSpeed;

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

        private bool isAccelerating;
        private float detectedRotation;

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

        public float Speed => currentSpeed;

        private void Awake()
        {
            inverseAcceleration = accelerationCurve.Inverse();
            inverseDecceleration = deccelerationCurve.Inverse();

            statesHandler = GetComponent<PlayerStatesHandler>();
        }

        private void FixedUpdate()
        {
            if (statesHandler.CurrentState != PlayerStatesHandler.PlayerState.Moving)
                return;

            currentSpeed = Mathf.Lerp(0, maxSpeed, accelerationButtonPressedTime);
            currentAngularSpeed = Mathf.Lerp(0, angularSpeed, Mathf.Abs(rotationButtonPressedTime)) * accelerationButtonPressedTime;

            SmoothAcceleration();
            SmoothRotation();
            if (Physics.Raycast(transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground")))
                Move();
            Rotate();
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

        private void SmoothRotation()
        {
            currentRotation = angularCurve.Evaluate(rotationButtonPressedTime);
            if (detectedRotation < 0)
            {
                rotationButtonPressedTime -= Time.fixedDeltaTime;
                if (rotationButtonPressedTime < -1)
                    rotationButtonPressedTime = -1;
            }
            else if (detectedRotation > 0)
            {
                rotationButtonPressedTime += Time.fixedDeltaTime;
                if (rotationButtonPressedTime > 1)
                    rotationButtonPressedTime = 1;
            }
        }

        private void Move() => transform.Translate(Acceleration * Vector3.forward, Space.Self);

        private void Rotate() => transform.Rotate(AngularSpeed * Vector3.up, Space.World);
    }
}