using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerWinningHandler : MonoBehaviour
    {
        private PlayerStatesHandler statesHandler;

        [SerializeField] private GameObject winningWindowUI;

        private void Awake()
        {
            statesHandler = GetComponent<PlayerStatesHandler>();
            winningWindowUI.SetActive(false);
        }

        public void PlayerWon()
        {
            statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Not_In_Move;
            winningWindowUI.SetActive(true);
        }
    }
}