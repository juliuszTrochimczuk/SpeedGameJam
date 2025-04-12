using Controllers;
using UnityEngine;

namespace Player
{
    public class PlayerSpinning : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerStatesHandler statesHandler;

        [SerializeField] private AnimationCurve spiningDecceleration;
        [SerializeField] private float spinningTime;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            statesHandler = GetComponent<PlayerStatesHandler>();
        }

        private void Start()
        {
            GameController.Instance.Input.Player.Spinning.performed += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 1);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Spinning;
            };
            GameController.Instance.Input.Player.Spinning.canceled += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 3);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Moving;
            };
        }
    }
}