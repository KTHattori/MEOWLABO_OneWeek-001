using UnityEngine;
using UnityEngine.UI;

public class ColorShift : MonoBehaviour, ITimeShiftTarget
{
    [SerializeField]
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("Image is not found");
        }
    }

    public void SetProgress(float progress)
    {
        image.color = TimeShift.ColorShift.colorGradient.Evaluate(progress);
    }

}
