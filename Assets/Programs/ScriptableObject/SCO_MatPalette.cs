//-----------------------------------------------
// MaterialPalette.cs
// - A ScriptableObject for holding materials
//--[Author]-------------------------------------
// Keita Hattori
//--[History]------------------------------------
// YYYY_MM_DD--EDITOR_NAME--EDIT_DESCRIPTION
// 2023_03_28--Keita_Hattori--Created_This_File
//--[About]--------------------------------------
// This object contains some materials,
// Can use like a palette for materials.
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MaterialPalette")]
public class MaterialPalette : ScriptableObject
{
    [Header("マテリアル")]
    [SerializeField]
    private Material[] material;
    public Material[] Material
    {
        get { return material; }
    }
}
