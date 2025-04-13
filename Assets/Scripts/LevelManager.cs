using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    private class TimeEvent
    {
        public string name;

        [Header("Time in seconds")]
        public float time;
        public bool repeat;
        public UnityEvent onTimeUp;

        public IEnumerator StartCountDown()
        {
            do
            {
                yield return new WaitForSeconds(time);
                onTimeUp?.Invoke();
            } while (repeat);
        }
    }

    [SerializeField] private List<TimeEvent> timeEvents;
    
    void Start()
    {
        foreach (var timeEvent in timeEvents)
        {
            StartCoroutine(timeEvent.StartCountDown());
        }
    }
}
