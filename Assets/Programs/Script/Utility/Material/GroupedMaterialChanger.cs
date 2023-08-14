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

public class GroupedMaterialChanger : MonoBehaviour
{
    public List<ChangeAllMaterials> components = new List<ChangeAllMaterials>();
    public int selection = 0;
    private int oldSelection = 0;

    public int Selection
    {
        get { return selection; }
        set
        {
            selection = value;
            CheckSelection();
            for(int i = 0,max = components.Count;i < max;i++)
            {
                Apply(components[i]);
            }
        }
    }

    void Reset()
    {
        FetchComponents();
    }
    void OnValidate()
    {
        if(oldSelection != selection)
        {
            CheckSelection();
            for(int i = 0,max = components.Count;i < max;i++)
            {
                Apply(components[i]);
            }
            oldSelection = selection;
        }    
    }

    [ContextMenu("FetchComponents")]
    void FetchComponents()
    {
        components.Clear();
        components.AddRange(gameObject.GetComponentsInChildren<ChangeAllMaterials>());
    }

    public void CheckSelection()
    {
        if(0 > selection)
        {
            selection = 0;
        }
        else
        {
            List<int> maxMats = new List<int>();
            for(int i = 0,max = components.Count;i < max;i++)
            {
                maxMats.Add(components[i].palette.Material.Length);
            }
            int maxMat = Mathf.Max(maxMats.ToArray());

            if(selection >= maxMat)
            {
                selection = maxMat - 1;
            }
        }
    }

    int GetMaxSelection(ChangeAllMaterials component)
    {
        int maxMat = component.palette.Material.Length;
        if(selection >= maxMat)
        {
            selection = maxMat - 1;
        }
        return selection;
    }

    public void Apply()
    {
        for(int i = 0,max = components.Count;i < max;i++)
        {
            Apply(components[i]);
        }
    }

    public void Apply(int index)
    {
        selection = index;
        for(int i = 0,max = components.Count;i < max;i++)
        {
            Apply(components[i]);
        }
    }

    public void Apply(ChangeAllMaterials component)
    {
        component.selection = selection;
        component.Apply(GetMaxSelection(component));
    }
}
