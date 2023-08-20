using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpolation;
using UnityEngine.Events;

[System.Serializable]
[DefaultExecutionOrder(200)]
public abstract class Transition
{
    public enum Type
    {
        Undefined,
        Cut,
        Fade,
        Wipe,
        CircleWipe,
        Slide,
    }

    public enum IOType
    {
        In,
        Out,
        OutIn
    }

    public enum Timing
    {
        OnTransitBegin,
        OnTransitEnd,
        OnTransitOutBegin,
        OnTransitOutUpdate,
        OnTransitOutEnd,
        OnTransitOutInSwitch,
        OnTransitInBegin,
        OnTransitInUpdate,
        OnTransitInEnd,
        [InspectorName("")]
        MAX_AMOUNT
    }

    [System.Serializable]
    public class TransitStage
    {
        public bool waitForEventComplete;
        public UnityEvent<float> events = new UnityEvent<float>();
    }

    [System.Serializable]
    public class InTransitionEvents
    {
        public TransitStage onTransitBegin = new TransitStage();
        public TransitStage onTransitEnd = new TransitStage();
        public TransitStage onTransitOutBegin = new TransitStage();
        public TransitStage onTransitOutUpdate = new TransitStage();
        public TransitStage onTransitOutEnd = new TransitStage();
        public TransitStage onTransitOutInSwitch = new TransitStage();
        public TransitStage onTransitInBegin = new TransitStage();
        public TransitStage onTransitInUpdate = new TransitStage();
        public TransitStage onTransitInEnd = new TransitStage();
        public TransitStage onEventWait = new TransitStage();
    }

    [System.Serializable]
    public class TransitionBaseInfo
    {
        /// <summary> 適用先キャンバス </summary>
        [SerializeField,Header("適用先キャンバス")]
        public GameObject targetCanvas;
        /// <summary> 遷移合計時間 </summary>
        [SerializeField,Header("遷移合計時間")]
        public float totalTime;
        [SerializeField,Header("遷移の階調")]
        public int step = 1;
        /// <summary> Timescaleを無視するかどうか(UnscaledTimeを使用します) </summary>
        [SerializeField,Header("Timescaleを無視する")]
        public bool useUnscaledTime;
        [SerializeField,Header("イン・アウト")]
        public IOType inOutType;
        [SerializeField,Header("イージングカーブ")]
        public Easing.Curve easeType;
        [SerializeField,Header("シーンロード時に破棄しない")]
        public bool dontDestroyOnLoad = false;
        [SerializeField,Header("遷移中に設定するイベント")]
        public InTransitionEvents inTransitionEventList;
    }

    // --- serialized variables ---
    // protected
    [SerializeField,Header("名称")]
    public string name;
    [SerializeField,Header("遷移基礎情報")]
    protected TransitionBaseInfo baseInfo;

    // --- static variables ---
    static protected int onGoingAmount = 0;

    private bool eventCompleted = true;

    // --- variables ---
    // protected
    protected Type type;

    // --- static properties ---
    static public TransitionManager current { get { return TransitionManager.instance;} }

    // --- properties ---
    public GameObject TargetCanvas { get { return baseInfo.targetCanvas;} set { this.baseInfo.targetCanvas = value; } }
    public float TotalTime { get { return baseInfo.totalTime;} set { this.baseInfo.totalTime = value; } }
    public bool UseUnscaledTime { get { return baseInfo.useUnscaledTime;} set { this.baseInfo.useUnscaledTime = value; } }
    public Type TransitType { get { return type; }}
    public IOType TransitInOutType { get { return baseInfo.inOutType; } set { this.baseInfo.inOutType = value;} }
    public Easing.Curve EaseType { get { return baseInfo.easeType;}  set { this.baseInfo.easeType = value;} }
    public bool IsEventCompleted { get { return eventCompleted; } }
    public InTransitionEvents EventList { get { return baseInfo.inTransitionEventList;}}
    public TransitionBaseInfo BaseInfo { get { return baseInfo; } }

    // static
    // template function: create instance of transition with type
    static public Transition Create(Type type)
    {
        switch(type)
        {
            case Type.Cut:
                return new CutTransition();
            case Type.Fade:
                return new FadeTransition();
            case Type.Wipe:
                return new WipeTransition();
            case Type.CircleWipe:
                return new CircleWipeTransition();
            case Type.Slide:
                return new SlideTransition();
            default:
                return null;
        }
    }
    
    
    protected void EnableWaitEventComplete()
    {
        Debug.Log("Enabled Wait Event Completed.");
        eventCompleted = false;
    }

    protected void EventCompleted(float dummy)
    {
        Debug.Log("Event Completed.");
        eventCompleted = true;
    }

    protected void WaitEventComplete(string eventName)
    {
        float waitCount = 0.0f;

        while(!eventCompleted)
        {
            waitCount += 1.0f;
            Debug.Log(waitCount);
            baseInfo.inTransitionEventList.onEventWait.events.Invoke(waitCount);
        }
    }

    public abstract void Initialize();
    public abstract void Uninitialize();
    public abstract void TransitIn(float value);
    public abstract void TransitOut(float value);
    protected abstract void TransitSwitch();
    public void OnTransitBegin()
    {
        eventCompleted = true;
        if(baseInfo.inTransitionEventList.onTransitBegin.waitForEventComplete)
        {
            EnableWaitEventComplete();
            baseInfo.inTransitionEventList.onTransitBegin.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitBegin.events.Invoke(0.0f);
        WaitEventComplete("OnTransitBegin");
    }
    public void OnTransitOutBegin()
    {
        if(baseInfo.inTransitionEventList.onTransitOutBegin.waitForEventComplete)
        {
            EnableWaitEventComplete();
            baseInfo.inTransitionEventList.onTransitOutBegin.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitOutBegin.events.Invoke(0.0f);
        WaitEventComplete("OnTransitOutBegin");
    }
    public void OnTransitOutUpdate(float value)
    {
        if(baseInfo.inTransitionEventList.onTransitOutUpdate.waitForEventComplete)
        {
            EnableWaitEventComplete();
            baseInfo.inTransitionEventList.onTransitOutUpdate.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitOutUpdate.events.Invoke(value);
        WaitEventComplete("OnTransitOutUpdate");
    }
    public void OnTransitOutEnd()
    {
        if(baseInfo.inTransitionEventList.onTransitOutEnd.waitForEventComplete)
        {
            EnableWaitEventComplete();
           baseInfo.inTransitionEventList.onTransitOutEnd.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitOutEnd.events.Invoke(1.0f);
        WaitEventComplete("OnTransitOutEnd");
    }
    public void OnTransitOutInSwitch()
    {
        TransitSwitch();
        if(baseInfo.inTransitionEventList.onTransitOutInSwitch.waitForEventComplete)
        {
            EnableWaitEventComplete();
            baseInfo.inTransitionEventList.onTransitOutInSwitch.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitOutInSwitch.events.Invoke(0.5f);
        WaitEventComplete("OnTransitOutInSwitch");
    }
    public void OnTransitInBegin()
    {
        if(baseInfo.inTransitionEventList.onTransitInBegin.waitForEventComplete)
        {
            EnableWaitEventComplete();
            baseInfo.inTransitionEventList.onTransitInBegin.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitInBegin.events.Invoke(0.0f);
        WaitEventComplete("OnTransitInBegin");
    }

    public void OnTransitInUpdate(float value)
    {
        if(baseInfo.inTransitionEventList.onTransitInUpdate.waitForEventComplete)
        {
            EnableWaitEventComplete();
            baseInfo.inTransitionEventList.onTransitInUpdate.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitInUpdate.events.Invoke(value);
        WaitEventComplete("OnTransitInUpdate");
    }
    public void OnTransitInEnd()
    {
        if(baseInfo.inTransitionEventList.onTransitInEnd.waitForEventComplete)
        {
            EnableWaitEventComplete();
            baseInfo.inTransitionEventList.onTransitInEnd.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitInEnd.events.Invoke(1.0f);
        WaitEventComplete("OnTransitInEnd");
    }
    public void OnTransitEnd()
    {
        if(baseInfo.inTransitionEventList.onTransitEnd.waitForEventComplete)
        {
            EnableWaitEventComplete();
            baseInfo.inTransitionEventList.onTransitEnd.events.AddListener(delegate {EventCompleted(1.0f);});
        }
        baseInfo.inTransitionEventList.onTransitEnd.events.Invoke(1.0f);
        WaitEventComplete("OnTransitEnd");
    }
}
