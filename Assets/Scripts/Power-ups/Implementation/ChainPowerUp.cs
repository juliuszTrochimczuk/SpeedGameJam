using System.Collections;
using Player;
using UnityEngine;

namespace PowerUps
{
    public class ChainPowerUp : AbstractPowerUp
    {
        [SerializeField] private float range;
        [SerializeField] private float _totalActivatedTime;
        [SerializeField] private float thrust;
        private GameObject _enemy;

        public GameObject GetEnemy()
        {
            return _enemy;
        }

        public void SetEnemy(GameObject enemy)
        {
            _enemy = enemy;
        }

        public override void Activate()
        {
            var collisionHandler = playerThatActivated.GetComponent<PlayerCollisionHandler>();
            collisionHandler.SetChainActivated(true);

            collisionHandler.StartCoroutine(Execute(collisionHandler));
        }

        private IEnumerator Execute(PlayerCollisionHandler collisionHandler)
        {
            float time = 0.0f;
            bool gotYaBitch = false;

            while (time < _totalActivatedTime)
            {
                if (_enemy != null)
                {
                    Debug.Log("TRYNNA GETTA BITCH");
                    var distance = Vector3.Distance(_enemy.transform.position, playerThatActivated.transform.position);
                    if (distance > range)
                    {
                        gotYaBitch = true;
                        Debug.Log("GOT YA HAHAHAHAH");
                        break;
                    }
                }
                else
                {
                    gotYaBitch = false;
                }
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (_enemy != null)
            {
                var enemyRigidBody = _enemy.GetComponent<Rigidbody>();
                enemyRigidBody.AddForce(_enemy.transform.forward * thrust, ForceMode.Impulse);
                Debug.Log("BITCH GOT SMOKED");
            }

            if (gotYaBitch)
            {
                var activePlayerRigidBody = playerThatActivated.GetComponent<Rigidbody>();
                activePlayerRigidBody.AddForce(activePlayerRigidBody.transform.forward * thrust, ForceMode.Impulse);
                Debug.Log("I GOT SMOKED");
            }

            collisionHandler.SetChainActivated(false);
        }
    }
}