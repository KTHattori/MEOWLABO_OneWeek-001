using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HueShift : MonoBehaviour, ITimeShiftTarget
{
    [SerializeField]
    private Volume volume = null;
    private ColorAdjustments colorAdjustments = null;

    void Reset()
    {
        volume = GetComponent<Volume>();

        if(volume == null)
        {
            Debug.LogError("Volume is not found.");
        }
        else
        {
            if(!volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                Debug.LogError("ColorAdjustments is not found.");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            Debug.LogError("ColorAdjustments is not found.");
        }
    }

    public void SetProgress(float progress)
    {
        if(colorAdjustments) colorAdjustments.hueShift.value = TimeShift.HueShift.hueCurve.Evaluate(progress);
    }
}
