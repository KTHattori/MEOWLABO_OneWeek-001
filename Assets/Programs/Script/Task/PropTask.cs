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

    public bool OnGoing
    {
        get
        {
            return onGoing;
        }
    }

    [SerializeField]
    private float completionTime = 0.5f;
    [SerializeField]
    private float heightMultiplier = 1.0f;

    private float timer = 0.0f;

    Vector3 startPos = Vector3.zero;
    Vector3 endPos = Vector3.zero;


    void Start()
    {
        TaskManager.AddTask(this);
        deliverTarget = taskData.DeliverTargetProp;
        onGoing = false;
        isCompleted = false;
    }

    void Update()
    {
        if(isCompleted && timer < completionTime)
        {
            timer += Time.deltaTime;
            float progress = timer / completionTime;
            UpdateCompleteMove(progress);
            if(timer >= completionTime)
            {
                TaskManager.CompleteTask(this);
                OnComplete();
            }
        }
    }

    public void Action(Prop previousProp)
    {
        if(isCompleted) return;
        TaskManager.OnGoingTask(this);
        onGoing = true;
    }

    public void Cancel(Prop nextProp)
    {
        if(deliverTarget == nextProp)
        {
            StartComplete();
            isCompleted = true;
        }
        else
        {
            TaskManager.CancelTask(this);
        }
        onGoing = false;
    }

    void UpdateCompleteMove(float progress)
    {
        // progress に応じて、このオブジェクトを放物線を描きながら移動させる
        // progress: 0.0f ~ 1.0f
        // 0.0f: startPos
        // 1.0f: endPos

        // 位置を計算
        Vector3 pos = Vector3.Lerp(startPos,endPos,progress);

        // 高さを計算
        float height = Mathf.Sin(Mathf.PI * progress);

        // 高さを加える
        pos.y += height * heightMultiplier;

        // 位置を更新
        transform.position = pos;

        // 向きを計算
        Vector3 targetAngle = endPos - startPos;
        targetAngle.y = 0.0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetAngle);

        // 向きを更新
        transform.rotation = targetRotation;
    }

    void StartComplete()
    {
        startPos = transform.position;
        endPos = deliverTarget.transform.position;
        Debug.Log("Task Completed");
    }

    void OnComplete()
    {
        // ここにタスク完了時の処理を書く
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPos, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(endPos, 0.1f);
    }
}
