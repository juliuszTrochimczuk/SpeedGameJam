using PowerUps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player
{
    public class PlayerStorage : MonoBehaviour
    {
        private AbstractPowerUp _currentPowerUp;
        private bool _isActivated = false;
        [SerializeField] private PowerUpUIController powerUpUiController;

        public AbstractPowerUp GetCurrentPowerUp() {
            return _currentPowerUp;
        }

        public bool isActivated() {
            return _isActivated;
        }

        public void AddPowerUp(AbstractPowerUp powerUp)
        {
            _currentPowerUp = powerUp;
        }

        public void ActivatePowerUp(InputAction.CallbackContext context)
        {
            context.action.performed += _ => 
            {
                if (_currentPowerUp != null)
                {
                    Debug.Log("Activate power up");
                    _isActivated = true;
                    powerUpUiController.UpdateImage(_currentPowerUp.Sprite);

                    _currentPowerUp.Activate();
                    Destroy(_currentPowerUp.gameObject); // does it make _currentPowerUp null?
                    _isActivated = false;
                }
            };
        }
    }
}