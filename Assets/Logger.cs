using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log($"Pause {pauseStatus}");
    }
    public void EventListener(string name)
    {
        Debug.Log($"event {name} {Time.time} {Time.realtimeSinceStartup}");
    }
}
