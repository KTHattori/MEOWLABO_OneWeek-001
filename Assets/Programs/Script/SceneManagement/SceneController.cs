using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Interpolation;

public class SceneController : MonoSingleton<SceneController>
{
    public bool isInTransition;
    private int loadSceneIndex = 0;
    private string loadSceneName = "";

    [SerializeField,Header("デフォルト設定"),Tooltip("遷移にかかる時間")]
    float transitTime = 2.0f;

    [SerializeField,Header("遷移の階調")]
    int transitStep = 1;

    [SerializeField,Tooltip("遷移時に使用するイージング")]
    Easing.Style transitEaseStyle = Easing.Style.Cubic;

    [SerializeField,Tooltip("カット設定")]
    CutTransition.UniqueSettings cutSettings;

    [SerializeField,Tooltip("フェード設定")]
    FadeTransition.UniqueSettings fadeSettings;

    [SerializeField,Tooltip("ワイプ設定")]
    WipeTransition.UniqueSettings wipeSettings;

    [SerializeField,Tooltip("円ワイプ設定")]
    CircleWipeTransition.UniqueSettings circleWipeSettings;
    [SerializeField,Tooltip("スライド設定")]
    SlideTransition.UniqueSettings slideSettings;

    [SerializeField,Header("遷移時に使用するキャンバス")]
    Canvas usingCanvas;

    [SerializeField,Header("デフォルトで使用するCutインスタンス")]
    CutTransition usingCutInstance;
    [SerializeField,Header("デフォルトで使用するFadeインスタンス")]
    FadeTransition usingFadeInstance;
    [SerializeField,Header("デフォルトで使用するWipeインスタンス")]
    WipeTransition usingWipeInstance;
    [SerializeField,Header("デフォルトで使用するCircleWipeインスタンス")]
    CircleWipeTransition usingCircleWipeInstance;
    [SerializeField,Header("デフォルトで使用するSlideインスタンス")]
    SlideTransition usingSlideInstance;

    public float TransitTime { set{this.transitTime = value;} }
    public Easing.Style TransitEaseStyle { set{this.transitEaseStyle = value;} }
    public Color FadeColor {set {this.fadeSettings.color = value;}}
    public Sprite FadeSprite { set{this.fadeSettings.imageSprite = value;}}

    
    void OnValidate()
    {
        // FetchTransitions();
    }

    [ContextMenu("FetchTransitions")]
    void FetchTransitions()
    {
        // registeredTransitions.Clear();
        // registeredTransitions.AddRange(TransitionManager.instance.Transitions);
    }

    [ContextMenu("ApplyDefaultCanvases")]
    public void ApplyDefaultCanvases()
    {
        usingCircleWipeInstance.TargetCanvas = usingCanvas.gameObject;
        usingCutInstance.TargetCanvas = usingCanvas.gameObject;
        usingFadeInstance.TargetCanvas = usingCanvas.gameObject;
        usingWipeInstance.TargetCanvas = usingCanvas.gameObject;
        usingSlideInstance.TargetCanvas = usingCanvas.gameObject;
    }

    void Start()
    {
        isInTransition = false;
        loadSceneIndex = 0;
    }


    void RegisterLoadEventToTransition(float dummy)
    {
        SceneManager.LoadScene(loadSceneName);
    }

    void CompleteSceneTransit(float dummy)
    {
        instance.isInTransition = false;
    }

    void RegisterTransitionEvent(Transition transition,Transition.IOType ioType = Transition.IOType.OutIn)
    {
        switch(ioType)
        {
            case Transition.IOType.OutIn:
                transition.EventList.onTransitOutInSwitch.waitForEventComplete = true;
                transition.EventList.onTransitOutInSwitch.events.AddListener(RegisterLoadEventToTransition);
                break;
            default:
                transition.EventList.onTransitEnd.events.AddListener(RegisterLoadEventToTransition);
                
                break;
        }
        transition.EventList.onTransitEnd.events.AddListener(CompleteSceneTransit);
    }

    /// 設定されたTransition設定をそのまま使用する
    static public void TransitToScene(string sceneName,Transition transition)
    {
        if(instance.isInTransition) return;
        instance.isInTransition = true;
        instance.SetLoadSceneName(sceneName);
        TransitToScene(transition);
    }

    static public void TransitToScene(Transition transition)
    {
        if(instance.isInTransition) return;
        instance.isInTransition = true;
        instance.ValidateSceneName();
        Debug.Log("Starting transit to Scene: " + instance.loadSceneName);

        transition.TransitInOutType = Transition.IOType.OutIn;
        transition.UseUnscaledTime = true;
        
        instance.RegisterTransitionEvent(transition);

        TransitionManager.instance.BeginTransit(transition);
    }

    static public void TransitToScene(string sceneName,Transition.Type transitType,float duration,Easing.Style easeStyle,Transition.IOType inoutType = Transition.IOType.OutIn)
    {
        if(instance.isInTransition) return;
        instance.isInTransition = true;
        instance.SetLoadSceneName(sceneName);
        if(transitType == Transition.Type.Cut) TransitToSceneCut(sceneName,duration,easeStyle,instance.cutSettings.color,instance.cutSettings.imageSprite,inoutType);
        if(transitType == Transition.Type.Fade) TransitToSceneFade(sceneName,duration,easeStyle,instance.fadeSettings.color,instance.fadeSettings.imageSprite,inoutType);
        if(transitType == Transition.Type.Wipe) TransitToSceneWipe(sceneName,duration,easeStyle,instance.wipeSettings.color,instance.wipeSettings.imageSprite,instance.wipeSettings.direction,inoutType);
        if(transitType == Transition.Type.CircleWipe) TransitToSceneCircleWipe(sceneName,duration,easeStyle,instance.circleWipeSettings.color,instance.circleWipeSettings.imageSprite,instance.circleWipeSettings.origin,instance.circleWipeSettings.clockwise,instance.circleWipeSettings.revertRotation,instance.circleWipeSettings.circleStart,inoutType);
        if(transitType == Transition.Type.Slide) TransitToSceneSlide(sceneName,duration,easeStyle,instance.slideSettings.color,instance.slideSettings.imageSprite,instance.slideSettings.direction,instance.slideSettings.revertDirection,inoutType);
    }

    static public void TransitToSceneCut(string sceneName,float duration,Easing.Style easeStyle,Color color,Sprite sprite = null,Transition.IOType inoutType = Transition.IOType.OutIn)
    {
        if(instance.isInTransition) return;
        instance.isInTransition = true;
        instance.SetLoadSceneName(sceneName);
        Debug.Log("Starting transit Cut to Scene: " + instance.loadSceneName);
        instance.usingCutInstance.Settings.color = color;
        instance.usingCutInstance.Settings.imageSprite = sprite;
        instance.usingCutInstance.BaseInfo.step = instance.transitStep;

        instance.usingCutInstance.TransitInOutType = inoutType;
        instance.usingCutInstance.TotalTime = duration;
        instance.usingCutInstance.UseUnscaledTime = true;
        instance.usingCutInstance.EaseType = new Easing.Curve(Easing.Type.InOut,easeStyle);
        
        instance.RegisterTransitionEvent(instance.usingCutInstance,inoutType);

        TransitionManager.instance.BeginTransit(instance.usingFadeInstance);
    }

    static public void TransitToSceneFade(string sceneName,float duration,Easing.Style easeStyle,Color color,Sprite sprite = null,Transition.IOType inoutType = Transition.IOType.OutIn)
    {
        if(instance.isInTransition) return;
        instance.isInTransition = true;
        instance.SetLoadSceneName(sceneName);
        Debug.Log("Starting transit Fade to Scene: " + instance.loadSceneName);
        instance.usingFadeInstance.Settings.color = color;
        instance.usingFadeInstance.Settings.imageSprite = sprite;
        instance.usingFadeInstance.BaseInfo.step = instance.transitStep;

        instance.usingFadeInstance.TransitInOutType = inoutType;
        instance.usingFadeInstance.TotalTime = duration;
        instance.usingFadeInstance.UseUnscaledTime = true;
        instance.usingFadeInstance.EaseType = new Easing.Curve(Easing.Type.InOut,easeStyle);
        
        instance.RegisterTransitionEvent(instance.usingFadeInstance,inoutType);

        TransitionManager.instance.BeginTransit(instance.usingFadeInstance);
    }
    static public void TransitToSceneWipe(string sceneName,float duration,Easing.Style easeStyle,Color color,Sprite sprite = null,WipeTransition.WipeDirection direction = WipeTransition.WipeDirection.Down,Transition.IOType inoutType = Transition.IOType.OutIn)
    {
        if(instance.isInTransition) return;
        instance.isInTransition = true;
        instance.SetLoadSceneName(sceneName);
        Debug.Log("Starting transit Wipe to Scene: " + instance.loadSceneName);
        instance.usingWipeInstance.Settings.color = color;
        instance.usingWipeInstance.Settings.imageSprite = sprite;
        instance.usingWipeInstance.Settings.direction = direction;
        instance.usingWipeInstance.BaseInfo.step = instance.transitStep;

        instance.usingWipeInstance.TransitInOutType = inoutType;
        instance.usingWipeInstance.TotalTime = duration;
        instance.usingWipeInstance.UseUnscaledTime = true;
        instance.usingWipeInstance.EaseType = new Easing.Curve(Easing.Type.InOut,easeStyle);
        
        instance.RegisterTransitionEvent(instance.usingWipeInstance,inoutType);

        TransitionManager.instance.BeginTransit(instance.usingWipeInstance);
    }

    static public void TransitToSceneCircleWipe(string sceneName,float duration,Easing.Style easeStyle,Color color,Sprite sprite = null,
                                                CircleWipeTransition.CircleOrigin origin = CircleWipeTransition.CircleOrigin.MiddleCenter,
                                                bool clockWise = true,bool revertOnSwitch = true,Image.Origin360 circleStart = Image.Origin360.Top,
                                                Transition.IOType inoutType = Transition.IOType.OutIn)
    {
        if(instance.isInTransition) return;
        instance.isInTransition = true;
        instance.SetLoadSceneName(sceneName);
        Debug.Log("Starting transit Wipe to Scene: " + instance.loadSceneName);
        instance.usingCircleWipeInstance.Settings.color = color;
        instance.usingCircleWipeInstance.Settings.imageSprite = sprite;
        instance.usingCircleWipeInstance.Settings.origin = origin;
        instance.usingCircleWipeInstance.Settings.circleStart = circleStart;
        instance.usingCircleWipeInstance.Settings.clockwise = clockWise;
        instance.usingCircleWipeInstance.Settings.revertRotation = revertOnSwitch;
        instance.usingCircleWipeInstance.BaseInfo.step = instance.transitStep;

        instance.usingCircleWipeInstance.TransitInOutType = inoutType;
        instance.usingCircleWipeInstance.TotalTime = duration;
        instance.usingCircleWipeInstance.UseUnscaledTime = true;
        instance.usingCircleWipeInstance.EaseType = new Easing.Curve(Easing.Type.InOut,easeStyle);
        
        instance.RegisterTransitionEvent(instance.usingCircleWipeInstance,inoutType);

        TransitionManager.instance.BeginTransit(instance.usingCircleWipeInstance);
    }

    static public void TransitToSceneSlide(string sceneName,float duration,Easing.Style easeStyle,Color color,Sprite sprite = null,SlideTransition.SlideDirection direction = SlideTransition.SlideDirection.Up,bool revertDirection = false,Transition.IOType inoutType = Transition.IOType.OutIn)
    {
        if(instance.isInTransition) return;
        instance.isInTransition = true;
        instance.SetLoadSceneName(sceneName);
        Debug.Log("Starting transit Wipe to Scene: " + instance.loadSceneName);
        instance.usingSlideInstance.Settings.color = color;
        instance.usingSlideInstance.Settings.imageSprite = sprite;
        instance.usingSlideInstance.Settings.direction = direction;
        instance.usingSlideInstance.Settings.revertDirection = revertDirection;
        instance.usingSlideInstance.BaseInfo.step = instance.transitStep;

        instance.usingSlideInstance.TransitInOutType = inoutType;
        instance.usingSlideInstance.TotalTime = duration;
        instance.usingSlideInstance.UseUnscaledTime = true;
        instance.usingSlideInstance.EaseType = new Easing.Curve(Easing.Type.InOut,easeStyle);
        
        instance.RegisterTransitionEvent(instance.usingSlideInstance,inoutType);

        TransitionManager.instance.BeginTransit(instance.usingSlideInstance);
    }

    void SetFadeSettings(FadeTransition fade)
    {
        fade.Settings = fadeSettings;
    }

    void SetCutSettings(CutTransition cut)
    {
        cut.Settings = cutSettings;
    }

    public void SetLoadSceneName(string sceneName)
    {
        loadSceneName = sceneName;
        instance.ValidateSceneName();
    }

    void ValidateSceneName()
    {
        if(instance.loadSceneName == "") instance.loadSceneName = SceneManager.GetActiveScene().name;
    }
    
    static public void SetScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
