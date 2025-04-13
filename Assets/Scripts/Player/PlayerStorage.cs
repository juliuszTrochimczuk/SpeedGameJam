using PowerUps;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerStorage : MonoBehaviour
    {
        private AbstractPowerUp _currentPowerUp;

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
                    _currentPowerUp.Activate();
                    Destroy(_currentPowerUp);
                }
            };
        }
    }
}