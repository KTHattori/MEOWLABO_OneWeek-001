using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Sprite(ScriptableObject)", menuName = "ScriptableObjects/Sprite")]
public class SCO_SpriteScriptableObject : ScriptableObject
{
    [field:SerializeField]
    public Sprite sprite{get;private set;}
}
