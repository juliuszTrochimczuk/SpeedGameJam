using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<TimeEvent> timeEvents;
    
    void Start()
    {
        foreach (var timeEvent in timeEvents)
        {
            StartCoroutine(timeEvent.StartCountDown());
        }
    }
    
}
