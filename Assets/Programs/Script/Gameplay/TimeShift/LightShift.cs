using UnityEngine;

public class LightShift : MonoBehaviour,ITimeShiftTarget
{
    [SerializeField]
    private Light directionalLight;

    [System.Serializable]
    public class LightShiftData
    {
        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 0.0f));
        public AnimationCurve tempertureCurve = new AnimationCurve(new Keyframe(0.0f, 1000.0f), new Keyframe(1.0f, 20000.0f));
        [GradientUsage(true)]
        public Gradient colorGradient = new Gradient();
    }

    [SerializeField]
    private LightShiftData lightShiftData = new LightShiftData();

    // Start is called before the first frame update
    void Start()
    {
        directionalLight = GetComponent<Light>();

        if ( directionalLight == null )
        {
            Debug.LogError("Directional Light is not found");
        }
    }

    public void SetProgress(float progress)
    {
        directionalLight.intensity = lightShiftData.intensityCurve.Evaluate(progress);
        directionalLight.color = lightShiftData.colorGradient.Evaluate(progress);
        // directionalLight.colorTemperature = lightShiftData.tempertureCurve.Evaluate(progress);
    }
}
