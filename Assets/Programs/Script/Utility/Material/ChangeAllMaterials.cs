//-----------------------------------------------
// ChangeAllMaterials.cs
// - A Script to change all materials in child.
//--[Author]-------------------------------------
// Keita Hattori
//--[History]------------------------------------
// YYYY_MM_DD--EDITOR_NAME--EDIT_DESCRIPTION
// 2023_03_28--Keita_Hattori--Created_This_File
//--[About]--------------------------------------
// This script changes all materials in child.
// Can choose mat from Material Palette object.
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAllMaterials : MonoBehaviour,IMaterialChangeable
{
    public MaterialPalette palette = null;
    public List<Renderer> renderers = new List<Renderer>();
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

    void Reset()
    {
        FetchComponents();
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

    [ContextMenu("FetchComponents")]
    void FetchComponents()
    {
        renderers.Clear();
        renderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());
    }

    public void Apply()
    {
        foreach(Renderer renderer in renderers)
        {
            int matLength = renderer.sharedMaterials.Length;
            for(int i = 0;i < matLength;i++)
            {
                renderer.sharedMaterials[i] = palette.Material[selection];
            }
            renderer.sharedMaterial = palette.Material[selection];
        }
        appliedMaterial = palette.Material[selection];  
    }

    public void Apply(int index)
    {
        if(renderers.Count <= 0) return;
        selection = index;
        foreach(Renderer renderer in renderers)
        {
            int matLength = renderer.sharedMaterials.Length;
            for(int i = 0;i < matLength;i++)
            {
                renderer.sharedMaterials[i] = palette.Material[selection];
            }
            renderer.sharedMaterial = palette.Material[selection];
        }
        if(palette.Material[selection] != null) appliedMaterial = palette.Material[selection];  
    }
}
