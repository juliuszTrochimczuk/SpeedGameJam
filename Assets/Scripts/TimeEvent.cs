using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    [System.Serializable]
    public class TimeEvent
    {
        [Header("Time in seconds")]
        public int time;
        public bool repeat;
        public UnityEvent onTimeUp;
        
        public IEnumerator StartCountDown()
        {
            do {
                yield return new WaitForSeconds(time);
                onTimeUp?.Invoke();
            }while(repeat);
        }
    }
}