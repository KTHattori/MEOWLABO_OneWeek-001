using UnityEngine;

public class LightShift : MonoBehaviour,ITimeShiftTarget
{
    [SerializeField]
    private Light directionalLight;

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
        directionalLight.intensity = TimeShift.LightShift.intensityCurve.Evaluate(progress);
        directionalLight.color = TimeShift.LightShift.colorGradient.Evaluate(progress);
        // directionalLight.colorTemperature = lightShiftData.tempertureCurve.Evaluate(progress);
    }
}
