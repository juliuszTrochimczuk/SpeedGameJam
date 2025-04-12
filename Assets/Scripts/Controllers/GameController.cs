using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        private InputSystem_Actions input;
        private List<PlayerInput> players = new();

        public static GameController Instance { get; private set; }

        [SerializeField] private LayerMask[] playerLayerMasks;
        [SerializeField] private LayerMask[] playersCullingMask;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
            
            input = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }

        public void AddPlayer(PlayerInput player)
        {
            players.Add(player);

            Transform playerParent = player.transform.root;

            int layerToAdd = (int)Mathf.Log(playerLayerMasks[players.Count - 1].value, 2);
            playerParent.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
            playerParent.GetComponentInChildren<Camera>().cullingMask = playersCullingMask[players.Count - 1].value;

        }
    }
}