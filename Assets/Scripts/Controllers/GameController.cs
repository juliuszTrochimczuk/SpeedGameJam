using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        private InputSystem_Actions input;
        private PlayerInputManager inputManager;
        private List<PlayerInput> players = new();

        public static GameController Instance { get; private set; }

        [SerializeField] private LayerMask[] playerLayerMasks;
        [SerializeField] private LayerMask[] playersCullingMask;
        [SerializeField] private float killingYPos;
        [SerializeField] private Transform[] spawningPoints;

        private bool gameWon = false;
        private bool gameStarts = false;

        public bool GameStarts => gameStarts;


        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
            
            input = new();
            inputManager =  GetComponent<PlayerInputManager>();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            input.General_Purpose.ResetLevel.performed += _ => ResetLevel();
        }

        private void OnEnable()
        {
            if (input != null)
                input.Enable();
        }

        private void OnDisable()
        {
            if (input != null)
                input.Disable();
        }

        public void AddPlayer(PlayerInput player)
        {
            players.Add(player);

            int layerToAdd = (int)Mathf.Log(playerLayerMasks[players.Count - 1].value, 2);
            player.transform.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
            player.transform.GetComponentInChildren<Camera>().cullingMask = playersCullingMask[players.Count - 1].value;
            Camera cameraLocal = player.transform.GetComponentInChildren<Camera>();
            cameraLocal.cullingMask = playersCullingMask[players.Count - 1].value;
            cameraLocal.GetUniversalAdditionalCameraData().SetRenderer(players.Count - 1);


            if (players.Count == inputManager.maxPlayerCount)
                gameStarts = true;

        }

        public void CheckIfGameIsWon()
        {
            if (gameWon)
                return;

            for (int i = players.Count - 1; i >= 0; i--)
            {
                if (players[i].transform.GetChild(0).position.y <= killingYPos)
                {
                    if (!gameStarts)
                    {
                        players[i].transform.GetChild(0).position = new Vector3(0, 2.5f, 0);
                        players[i].transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        Destroy(players[i].transform.GetChild(0).gameObject);
                        players.RemoveAt(i);
                    }
                }
            }
            if (players.Count == 1 && gameStarts)
            {
                players[0].transform.GetChild(0).GetComponent<Player.PlayerWinningHandler>().PlayerWon();
                gameWon = true;
            }
        }

        private void ResetLevel()
        {
            players.Clear();
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}