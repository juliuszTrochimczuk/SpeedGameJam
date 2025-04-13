using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace PowerUps
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private List<AbstractPowerUp> possiblePowerUps;
        [SerializeField] private Transform pointToSpawn;
        [SerializeField] private float timer;
        [SerializeField] private bool onlyOnce;

        private bool firedCoroutine;
        private AbstractPowerUp activePowerUp;

        private void Update()
        {
            if (!firedCoroutine && GameController.Instance.GameStarts)
            {
                firedCoroutine = true;
                StartCoroutine(Spawning());
            }
        }

        private IEnumerator Spawning()
        {
            do
            {
                activePowerUp = Instantiate(possiblePowerUps[Random.Range(0, possiblePowerUps.Count)], pointToSpawn.transform.position, pointToSpawn.transform.rotation);
                float time = 0.0f;
                while (time < timer)
                {
                    time += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitUntil(() => activePowerUp != null);
            } while (!onlyOnce);
        }
    }
}