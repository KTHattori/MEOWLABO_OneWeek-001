using System;
using UnityEngine;


public class TimeShift : MonoSingleton<TimeShift>
{
    [SerializeField,Tooltip("True = Morning, False = Night"),Header("Is Morning")]
    private bool isMorning = false;
    [SerializeField,Range(0.0f,1.0f),Tooltip("0.0f = Night, 1.0f = Morning"),Header("Time")]
    private float progress = 0.0f;

    public bool IsMorning
    {
        get
        {
            return isMorning;
        }
        set
        {
            isMorning = value;
        }
    }
    public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            progress = value;
        }
    }

    public void SetProgress(float progress)
    {
        this.progress = progress;
        OnValueChanged(progress);
    }

    void OnValueChanged(float progress)
    {
        ITimeShiftTarget[] targets = GameObjectUtility.FindObjectsOfInterface<ITimeShiftTarget>();
        foreach ( var n in targets )
        {
            n.SetProgress(progress);
        }

    }

    void OnValidate()
    {
        OnValueChanged(progress);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
