using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

namespace PowerUps
{
    public abstract class AbstractPowerUp : MonoBehaviour
    {
        private List<GameObject> players;
        [SerializeField]
        private float rangeDistance;
        protected Transform playerThatActivated;

        public void Awake()
        {
            players = GameObject.FindGameObjectsWithTag("Player").ToList();
        }

        public void FixedUpdate()
        {
            foreach (var player in players)
            {
                if (player == null)
                    players.Remove(player);

                var distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < rangeDistance)
                {
                    Debug.Log("Pick up");
                    player.GetComponent<PlayerStorage>().AddPowerUp(CreateCopy(player.transform));
                    Destroy(gameObject);
                    Debug.Log("Destroyed");
                }
            }
        }

        public abstract void Activate();

        //protected abstract void DeepCopy(AbstractPowerUp abstractPowerUp);

        private AbstractPowerUp CreateCopy(Transform player)
        {
            AbstractPowerUp newPowerUp = Instantiate(this, player);
            var renderer = newPowerUp.gameObject.GetComponent<Renderer>();
            renderer.enabled = false;
            newPowerUp.playerThatActivated = player;
            //DeepCopy(newPowerUp);
            return newPowerUp;
        }
    }
}