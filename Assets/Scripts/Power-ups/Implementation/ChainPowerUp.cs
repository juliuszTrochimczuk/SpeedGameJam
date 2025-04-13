using System.Collections;
using Player;
using PowerUps;
using UnityEngine;

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
        var collisionHandler = GetComponent<PlayerCollisionHandler>();
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
                var distance = Vector3.Distance(_enemy.transform.position, playerThatActivated.transform.position);
                if (distance < range)
                {
                    gotYaBitch = true;
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
        var enemyRigidBody = _enemy.GetComponent<Rigidbody>();
        enemyRigidBody.AddForce(0, 0, thrust, ForceMode.Impulse);

        if (gotYaBitch)
        {
            var activePlayerRigidBody = playerThatActivated.GetComponent<Rigidbody>();
            activePlayerRigidBody.AddForce(0, 0, thrust, ForceMode.Impulse);
        }
        
        collisionHandler.SetChainActivated(false);
    }
}
