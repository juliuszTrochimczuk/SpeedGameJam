using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpSript : MonoBehaviour
{
    private int i = 0;
    public string name;

    public void LogThis()
    {
        Debug.Log(name +" "+ i++);
    }
}
