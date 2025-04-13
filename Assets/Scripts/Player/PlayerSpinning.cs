using Controllers;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerSpinning : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerStatesHandler statesHandler;

        private Vector3 direction;
        private Coroutine lerpingSpeed;

        [SerializeField] private float speedDecreasingTime;
        [SerializeField] private float strength = 1;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            statesHandler = GetComponent<PlayerStatesHandler>();
        }

        private void FixedUpdate()
        {
            if (statesHandler.CurrentState != PlayerStatesHandler.PlayerState.Spinning)
                return;

            if (!Physics.Raycast(transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground")))
                return;

            transform.Translate(movement.Speed * Time.fixedDeltaTime * direction * strength, Space.World);
            if (strength == 1)
                movement.Speed -= 0.5f * Time.fixedDeltaTime;
        }

        public void Spinning(InputAction.CallbackContext context)
        {
            if (statesHandler.CurrentState == PlayerStatesHandler.PlayerState.Not_In_Move)
                return;

            context.action.performed += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 1);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Spinning;
                direction = transform.forward;
                lerpingSpeed = StartCoroutine(LerpingSpeed());
            };
            context.action.canceled += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 3);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Moving;
                StopCoroutine(lerpingSpeed);
            };
        }

        public void ChangeDirection(Vector3 direction) => this.direction = direction;

        public void ChangeStrength(float newStrength, float duration)
        {
            strength = newStrength;
            StartCoroutine(LerpingStrength(duration));
        }

        private IEnumerator LerpingStrength(float duration)
        {
            float time = 0.0f;
            float oldStrength = strength;
            while (time < duration)
            {
                strength = Mathf.Lerp(oldStrength, 1, time / duration);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            strength = 1;
        }

        private IEnumerator LerpingSpeed()
        {
            float oldSpeed = movement.Speed;
            float time = 0.0f;
            while (time < speedDecreasingTime)
            {
                movement.Speed = Mathf.Lerp(oldSpeed, 0, time / speedDecreasingTime);
                if (strength == 1)
                    time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            movement.Speed = 0;
        }
    }
}