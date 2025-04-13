using System.Collections;
using Player;
using UnityEngine;

namespace PowerUps
{
    public class SpeedBoostPowerUp : AbstractPowerUp
    {
        [SerializeField] private float multiplier;

        public override void Activate()
        {
            PlayerMovement movement = playerThatActivated.GetComponent<PlayerMovement>();

            movement.StartCoroutine(WaitNSeconds(5, movement));
        }

        private IEnumerator WaitNSeconds(int seconds, PlayerMovement movement)
        {
            Debug.Log("Speed boost started");
            float previousMaxSpeed = movement.GetMaxSpeed();
            movement.SetMaxSpeed(previousMaxSpeed * multiplier);

            yield return new WaitForSeconds(seconds);

            movement.SetMaxSpeed(previousMaxSpeed);
            Debug.Log("Previous speed returned.");
        }

        //protected override void DeepCopy(AbstractPowerUp abstractPowerUp) {
        //}
    }
}
