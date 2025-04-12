using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                Activate();
            }
        }
    }

    public abstract void Activate();
}
