using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        private InputSystem_Actions input;

        public static GameController Instance { get; private set; }

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
    }
}