using UnityEngine;
using UnityEngine.Rendering;

public class FullScreenMatChanger : MonoBehaviour,IMaterialChangeable
{
    public FullScreenPassRendererFeature feature = null;
    public MaterialPalette palette = null;
    public Material appliedMaterial;
    public int selection = 0;
    private int oldSelection = 0;

    public int Selection
    {
        get { return selection; }
        set
        {
            selection = value;
            CheckSelection();
            Apply();
        }
    }

    public MaterialPalette Palette
    {
        get { return palette; }
        set
        {
            palette = value;
            CheckSelection();
            Apply();
        }
    }

    void OnValidate()
    {
        if(palette == null) { return; }
        if(oldSelection != selection)
        {
            CheckSelection();
            Apply();
            oldSelection = selection;
        }    
    }

    public void CheckSelection()
    {
        if(0 > selection)
        {
            selection = 0;
        }
        else if(selection >= palette.Material.Length)
        {
            selection = palette.Material.Length - 1;
        }
    }

    public void Apply()
    {
        if(feature != null)
        {
            feature.passMaterial = palette.Material[selection];
        }
        appliedMaterial = palette.Material[selection];  
    }

    public void Apply(int index)
    {
        selection = index;
        Apply();
    }
}
