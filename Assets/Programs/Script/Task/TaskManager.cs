using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoSingleton<TaskManager>
{
    public class Task
    {
        public PropTask task;
        public bool isCompleted = false;
        public bool isOnGoing = false;
        public int taskIndex = 0;
        public Image iconImage = null;
        public Task(PropTask task)
        {
            this.task = task;
        }
    }

    [SerializeField]
    private Dictionary<PropTask,Task> taskList = new Dictionary<PropTask,Task>();

    [SerializeField]
    private RectTransform taskUI = null;

    [SerializeField]
    private GameObject taskIconPrefab = null;

    [SerializeField]
    private Color completedColor = Color.white;

    [SerializeField]
    private Color onGoingColor = Color.yellow;

    [SerializeField]
    private Color notStartedColor = Color.gray;

    private int completedTaskCount = 0;

    void OnAllTaskCompleted()
    {
        Debug.Log("All Task Completed");
    }


    static public void AddTask(PropTask task)
    {
        Task addingTask = new Task(task);
        addingTask.taskIndex = instance.taskList.Count;
        GameObject taskIcon = Instantiate(instance.taskIconPrefab,instance.taskUI);
        addingTask.iconImage = taskIcon.GetComponent<Image>();
        addingTask.iconImage.sprite = task.TaskData.Icon;
        addingTask.iconImage.color = instance.notStartedColor;
        instance.taskList.Add(task,addingTask);
    }

    static public void OnGoingTask(PropTask propTask)
    {
        instance.taskList[propTask].isOnGoing = true;
        instance.taskList[propTask].iconImage.color = instance.onGoingColor;
    }

    static public void CancelTask(PropTask propTask)
    {
        instance.taskList[propTask].isOnGoing = false;
        instance.taskList[propTask].iconImage.color = instance.notStartedColor;
    }

    static public void CompleteTask(PropTask propTask)
    {
        instance.taskList[propTask].isCompleted = true;
        instance.taskList[propTask].isOnGoing = false;
        instance.taskList[propTask].iconImage.color = instance.completedColor;
        instance.completedTaskCount++;
        instance.CheckAllTaskCompleted();
    }

    bool IsAllTaskCompleted()
    {
        if(instance.completedTaskCount == instance.taskList.Count)
        {
            return true;
        }
        return false;
    }

    void CheckAllTaskCompleted()
    {
        if(IsAllTaskCompleted()) OnAllTaskCompleted();
    }
}
