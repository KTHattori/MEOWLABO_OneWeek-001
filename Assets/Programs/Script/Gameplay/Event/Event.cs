using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Event
{
    public bool isExecuting = false;
    public bool isFinished = false;
    public bool allowMultiple = false;
    public float waitTime = 0f;

    public void Finish()
    {
        OnFinished();
    }

    public void ForceFinish()
    {
        OnFinished();
    }

    public void OnFinished()
    {
        isExecuting = false;
        isFinished = true;
    }
}
