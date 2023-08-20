using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpolation;

public class TransitionManager : MonoSingleton<TransitionManager>
{
    // --- serialized variables ---
    [SerializeField]
    GameObject targetCanvas = null;

    /// <summary> 遷移スピード倍率 </summary>
    [SerializeReference,Header("遷移リスト")]
    List<Transition> transitions = new List<Transition>();

    [SerializeField,Header("遷移スピード倍率")]
    float speedMultiplier = 1.0f;

    // --- variables ---
    // private
    int registeredTransition = 0;
    Dictionary<string,Transition> transitionDictionary;
    Transition current;
    Transition next;
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private float targetTime;
    [SerializeField]
    private bool isTransition;
    [SerializeField]
    private bool isCompletedTransitIn;
    [SerializeField]
    private bool isCompletedTransitOut;
    [SerializeField]
    private float value_in;
    [SerializeField]
    private float value_out;
    [SerializeField]
    private float halfTotalTime;
    private bool isInOut;

    // --- properties ---
    public GameObject TargetCanvas { get { return targetCanvas;} }
    public float CurrentTime { get { return currentTime;}}
    public bool IsTransition { get { return isTransition;}}
    public float SpeedMultiplier { get{ return speedMultiplier; } set { this.speedMultiplier = value;} }
    public float TransitProgress { get { return Mathf.Clamp01(currentTime / current.TotalTime);} }

    public List<Transition> Transitions { get { return transitions;} }

    

    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        dontDestroyOnLoad = true;
        current = null;
        next = null;
        isTransition = false;
        transitionDictionary = new Dictionary<string, Transition>();

        foreach(Transition transition in transitions)
        {
            if(transitionDictionary.TryAdd(transition.name,transition))
            {
                Debug.Log("TransitionManager: Registered <" + transition.name + ">.");
            }
            else
            {
                Debug.LogWarning("TransitionManager: Duplicated transition key <" + transition.name +  "> detected.\nPlease rename the object to register correctly."); 
                continue;
            }
        }

        transitions.Clear();
        InitializeValues();
    }

    void Update()
    {

    }

    void InitializeValues()
    {
        currentTime = 0.0f;
        targetTime = 0.0f;
        halfTotalTime = 0.0f;
        value_in = 0.0f;
        value_out = 0.0f;
        isCompletedTransitIn = true;
        isCompletedTransitOut = true;
        next = null;
        isInOut = false;
    }

    void SetTransitIn()
    {
        isCompletedTransitIn = false;
    }

    void SetTransitOut()
    {
        isCompletedTransitOut = false;
    }

    void SetTransitOutIn()
    {
        SetTransitIn();
        SetTransitOut();
        isInOut = true;
        halfTotalTime = targetTime * 0.5f;
        targetTime = halfTotalTime;
    }

    static public Transition GetTransition(string transitName)
    {
        if(instance.transitionDictionary.ContainsKey(transitName)) return instance.transitionDictionary[transitName];
        else return null;
    }

    static public void BeginTransit(string transitName)
    {
        if(instance.transitionDictionary.ContainsKey(transitName)) instance.BeginTransit(instance.transitionDictionary[transitName]);
    }

    static public void SetBeginTransit(Transition transition)
    {
        instance.BeginTransit(transition);
    }

    public void BeginTransit(Transition transition)
    {
        if(transition == null) return;
        if(isTransition) return;
        InitializeValues();
    
        next = transition;
        targetTime = transition.TotalTime;
        isTransition = true;
        if(transition.TargetCanvas == null) transition.TargetCanvas = targetCanvas;
        transition.Initialize();
        transition.OnTransitBegin();

        switch(transition.TransitInOutType)
        {
            case Transition.IOType.In:
                SetTransitIn();
                transition.OnTransitInBegin();
            break;
            case Transition.IOType.Out:
                SetTransitOut();
                transition.OnTransitOutBegin();
            break;
            case Transition.IOType.OutIn:
                SetTransitOutIn();
                transition.OnTransitOutBegin();
            break;
        }
    }

    // 一時停止
    public void PauseTransit()
    {
        if(!isTransition) return;
        isTransition = false;
    }

    // 中断（破棄）
    public void AbortTransit()
    {
        if(!isTransition) return;
        currentTime = 0.0f;
        isTransition = false;
        current = null;
    }

    // リセットして開始
    public void RestartTransit()
    {
        if(!isTransition) return;
        BeginTransit(current);
    }

    // 終了
    void EndTransit()
    {
        if(!isTransition) return;
        current.OnTransitEnd();
        current.Uninitialize();
        isTransition = false;
        current = null;
    }
  
    void UpdateTimer()
    {
        if(current.UseUnscaledTime) currentTime += speedMultiplier * TimeUtility.UnscaledDeltaTime;
        else currentTime += speedMultiplier * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        if(!isTransition) return;

        if(next != null)
        {
            current = next;
            next = null;
        }

        if(!isCompletedTransitOut)
        {
            if(isInOut) value_out = Easing.Ease(currentTime,targetTime,Easing.Type.Out,current.EaseType.style);
            else value_out = Easing.Ease(currentTime,targetTime,current.EaseType);
            if(current.BaseInfo.step > 1) value_out = MathUtility.Posterize(value_out,current.BaseInfo.step);
            current.TransitOut(value_out);
            current.OnTransitOutUpdate(value_out);
        }
        else if(!isCompletedTransitIn)
        {
            if(isInOut) value_in = Easing.Ease(currentTime - halfTotalTime,targetTime,Easing.Type.In,current.EaseType.style);
            else value_in = Easing.Ease(currentTime - halfTotalTime,targetTime,current.EaseType);
            if(current.BaseInfo.step > 1) value_in = MathUtility.Posterize(value_in,current.BaseInfo.step);
            current.TransitIn(value_in);
            current.OnTransitInUpdate(value_in);
        }

        UpdateTimer();

        if(!isCompletedTransitOut && !isCompletedTransitIn && currentTime >= halfTotalTime)
        {
            isCompletedTransitOut = true;
            targetTime = halfTotalTime;
            current.OnTransitOutEnd();
            current.OnTransitOutInSwitch();
        }
        else if(currentTime >= current.TotalTime)
        {
            if(!isCompletedTransitOut)
            {
                isCompletedTransitOut = true;
                current.OnTransitOutEnd();
            }
            if(!isCompletedTransitIn)
            {
                isCompletedTransitIn = true;
                current.OnTransitInEnd();
            }
            EndTransit();
        }
    }    
}
