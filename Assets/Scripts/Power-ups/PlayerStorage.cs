using Controllers;
using PowerUps;
using UnityEngine;

namespace Player
{
    public class PlayerStorage : MonoBehaviour
    {
        private AbstractPowerUp _currentPowerUp;

        public void Start()
        {
            GameController.Instance.Input.Player.ActivitingPowerUp.performed += _ => ActivatePowerUp();
        }

        public void AddPowerUp(AbstractPowerUp powerUp)
        {
            _currentPowerUp = powerUp;
        }

        private void ActivatePowerUp()
        {
            if (_currentPowerUp != null)
            {
                _currentPowerUp.Activate();
                Destroy(_currentPowerUp);
            }
        }
    }
}