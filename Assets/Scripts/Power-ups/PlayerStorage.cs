using Controllers;
using PowerUps;
using UnityEngine;

namespace Player
{
    public class PlayerStorage : MonoBehaviour
    {
        private AbstractPowerUp _currentPowerUp;

        void Start()
        {
            GameController.Instance.Input.Player.ActivitingPowerUp.performed += _ => ActivatePowerUp();
        }

        private void ActivatePowerUp()
        {
            _currentPowerUp.Activate();
        }

        public void AddPowerUp(AbstractPowerUp powerUp)
        {
            _currentPowerUp = powerUp;
        }
    }
}