using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropTask : MonoBehaviour, IPropAction
{
    [SerializeField]
    Prop deliverTarget = null;

    [SerializeField]
    private SCO_Task taskData = null;

    public SCO_Task TaskData
    {
        get
        {
            return taskData;
        }
    }

    [SerializeField]
    private bool isCompleted = false;

    public bool IsCompleted
    {
        get
        {
            return isCompleted;
        }
        set
        {
            isCompleted = value;
        }
    }

    [SerializeField]
    private bool onGoing = false;

    void Start()
    {
        TaskManager.AddTask(this);
        deliverTarget = taskData.DeliverTargetProp;
    }

    public void Action(Prop previousProp)
    {
        if(isCompleted) return;
        TaskManager.OnGoingTask(this);
        onGoing = true;
    }

    public void Cancel(Prop nextProp)
    {
        if(deliverTarget = nextProp)
        {
            TaskManager.CompleteTask(this);
            isCompleted = true;
        }
        else
        {
            TaskManager.CancelTask(this);
        }
        onGoing = false;
    }
}
