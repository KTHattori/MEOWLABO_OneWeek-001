using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMaterialChangeable
{
    public void CheckSelection();
    public void Apply();
    public void Apply(int index);
    public MaterialPalette Palette { get; set; }
    public int Selection { get; set; }
}
