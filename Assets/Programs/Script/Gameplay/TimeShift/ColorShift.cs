using UnityEngine;
using UnityEngine.UI;

public class ColorShift : MonoBehaviour, ITimeShiftTarget
{
    [SerializeField]
    private Image image;

    [System.Serializable]
    public class ColorShiftData
    {
        public Gradient colorGradient = new Gradient();
    }

    [SerializeField]
    private ColorShiftData colorShiftData = new ColorShiftData();

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
        image.color = colorShiftData.colorGradient.Evaluate(progress);
    }

}
