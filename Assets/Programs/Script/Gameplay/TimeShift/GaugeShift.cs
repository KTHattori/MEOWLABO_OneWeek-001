using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeShift : MonoBehaviour, ITimeShiftTarget
{
    [SerializeField]
    private Image gaugeFill;

    void Reset()
    {
        gaugeFill = GetComponent<Image>();

        if (gaugeFill == null)
        {
            Debug.LogError("Image Compoenent is not found");
        }

        gaugeFill.type = Image.Type.Filled;
    }

    public void SetProgress(float progress)
    {
        gaugeFill.fillAmount = progress;
        gaugeFill.color = TimeShift.GaugeShift.colorGradient.Evaluate(progress);
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
