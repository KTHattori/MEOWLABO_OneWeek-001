using System;
using UnityEngine;


public class TimeShift : MonoSingleton<TimeShift>
{
    [SerializeField,Tooltip("True = Morning, False = Night"),Header("Is Morning")]
    private bool isMorning = false;
    [SerializeField,Range(0.0f,1.0f),Tooltip("0.0f = Night, 1.0f = Morning"),Header("Time")]
    private float progress = 0.0f;

    [System.Serializable]
    public class ColorShiftData
    {
        public Gradient colorGradient = new Gradient();
    }
    
    [System.Serializable]
    public class GaugeShiftData
    {
        public Gradient colorGradient = new Gradient();
    }

    [System.Serializable]
    public class LightShiftData
    {
        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 0.0f));
        public AnimationCurve tempertureCurve = new AnimationCurve(new Keyframe(0.0f, 1000.0f), new Keyframe(1.0f, 20000.0f));
        [GradientUsage(true)]
        public Gradient colorGradient = new Gradient();
    }

    [System.Serializable]
    public class HueShiftData
    {
        public AnimationCurve hueCurve = new AnimationCurve(new Keyframe(0.0f, 10.0f), new Keyframe(1.0f, -5.0f));
    }

    [SerializeField]
    private ColorShiftData colorShiftData = new ColorShiftData();

    [SerializeField]
    private GaugeShiftData gaugeShiftData = new GaugeShiftData();

    [SerializeField]
    private LightShiftData lightShiftData = new LightShiftData();

    [SerializeField]
    private HueShiftData hueShiftData = new HueShiftData();

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

    static public ColorShiftData ColorShift
    {
        get
        {
            return instance.colorShiftData;
        }
        set
        {
            instance.colorShiftData = value;
        }
    }

    static public GaugeShiftData GaugeShift
    {
        get
        {
            return instance.gaugeShiftData;
        }
        set
        {
            instance.gaugeShiftData = value;
        }
    }

    static public LightShiftData LightShift
    {
        get
        {
            return instance.lightShiftData;
        }
        set
        {
            instance.lightShiftData = value;
        }
    }

    static public HueShiftData HueShift
    {
        get
        {
            return instance.hueShiftData;
        }
        set
        {
            instance.hueShiftData = value;
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
