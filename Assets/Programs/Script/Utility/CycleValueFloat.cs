using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpolation;

public class CycleValueFloat : MonoBehaviour
{
    public enum Type
    {
        Sin,
    }

    [SerializeField]
    float offsetValue;
    public float OffsetValue { get{return offsetValue;} set{this.offsetValue = value;}} 

    [SerializeField]
    float minValue;
    public float MinValue { get{return minValue;} set{this.minValue = value;}}

    [SerializeField]
    float maxValue;
    public float MaxValue { get{return maxValue;} set{this.maxValue = value;}}

    [SerializeField]
    float loopTime;
    public float LoopTime {  get{return loopTime;} set{this.loopTime = value;} }

    [SerializeField]
    Type loopType;
    public Type LoopType { get{return loopType;} set{this.loopType = value;}}

    private float value;
    public float Value { get{return value;}}

    float currentTime;
    float currentCycleAngle;

    [SerializeField]
    bool isReverted = false;

    [SerializeField]
    bool playOnAwake = true;
    bool isPlaying;
    // Start is called before the first frame update

    void Awake()
    {
        if(playOnAwake) Play();
    }
    void Start()
    {
        currentTime = 0.0f;
        currentCycleAngle = 0.0f;
        value = offsetValue + minValue;
    }

    void Update()
    {
        if(isPlaying)
        {
            switch(loopType)
            {
                case Type.Sin:
                UpdateSin();
                break;
            }
        }
        
        currentCycleAngle = 360.0f * Easing.Linear(currentTime,loopTime);
        currentTime += 1.0f * Time.deltaTime;
        if(currentTime > loopTime)
        {
            currentTime = 0.0f;
        }
    }

    void UpdateSin()
    {
        if(isReverted)
        {
            value = offsetValue + minValue + maxValue * Mathf.Cos(currentCycleAngle * Mathf.Deg2Rad);
        }
        else
        {
            value = offsetValue + minValue + maxValue * Mathf.Sin(currentCycleAngle * Mathf.Deg2Rad);
        }
    }

    public void Play()
    {
        currentTime = 0.0f;
        isPlaying = true;
    }

    public void Pause()
    {
        isPlaying = false;
    }

    public void Resume()
    {
        isPlaying = true;
    }

    public void Stop()
    {
        currentTime = 0.0f;
        isPlaying = false;
    }
}
