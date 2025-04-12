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

                var distance = Vector3.Distance(player.transform.position, player.transform.forward);
                if (distance < rangeDistance)
                {
                    player.GetComponent<PlayerStorage>().AddPowerUp(CreateCopy(player.transform));
                    Destroy(this);
                }
            }
        }

        public abstract void Activate();

        protected abstract void DeepCopy(AbstractPowerUp abstractPowerUp);

        private AbstractPowerUp CreateCopy(Transform player) 
        {
            AbstractPowerUp newPowerUp = Instantiate(this, player);
            DeepCopy(newPowerUp);
            return newPowerUp;
        }
    }
}