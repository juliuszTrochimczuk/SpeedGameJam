using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerSpinning : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerStatesHandler statesHandler;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            statesHandler = GetComponent<PlayerStatesHandler>();
        }

        private void FixedUpdate()
        {
            if (statesHandler.CurrentState != PlayerStatesHandler.PlayerState.Spinning)
                return;

            transform.Translate(movement.Acceleration * Vector3.forward, Space.Self);
        }

        public void Spinning(InputAction.CallbackContext context)
        {
            context.action.performed += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 1);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Spinning;
            };
            context.action.canceled += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 3);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Moving;
            };
        }
    }
}