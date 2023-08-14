using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatPalleteSelector : MonoBehaviour
{
    public List<GroupedMaterialChanger> groupedChangers = new List<GroupedMaterialChanger>();
    public List<FullScreenMatChanger> fullScreenChangers = new List<FullScreenMatChanger>();
    public int selection = 0;
    private int oldSelection = -1;
    void OnValidate()
    {
        if(oldSelection != selection)
        {
            SetPaletteSelection(selection);
            oldSelection = selection;
        }
    }

    [ContextMenu("FetchComponents")]
    void FetchComponents()
    {
        groupedChangers.Clear();
        fullScreenChangers.Clear();
        groupedChangers.AddRange(gameObject.GetComponentsInChildren<GroupedMaterialChanger>());
        fullScreenChangers.AddRange(gameObject.GetComponentsInChildren<FullScreenMatChanger>());
    }

    public void SetFullscreenSelection(int index)
    {
        selection = index;
        for(int i = 0,max = fullScreenChangers.Count;i < max;i++)
        {
            fullScreenChangers[i].Selection = index;
        }
    }

    public void SetGroupedSelection(int index)
    {
        selection = index;
        for(int i = 0,max = groupedChangers.Count;i < max;i++)
        {
            groupedChangers[i].Selection = index;
        }
    }

    public void SetPaletteSelection(int index)
    {
        selection = index;
        for(int i = 0,max = groupedChangers.Count;i < max;i++)
        {
            groupedChangers[i].Selection = index;
        }
        for(int i = 0,max = fullScreenChangers.Count;i < max;i++)
        {
            fullScreenChangers[i].Selection = index;
        }
    }
}
