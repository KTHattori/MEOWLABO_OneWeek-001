using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipRelay", menuName = "ScriptableObject/AudioClipRelay")]
public class SCO_AudioClipRelay : ScriptableObject
{
    [field:SerializeField] public AudioClip AudioClip{ get; private set;}
    
    [field:SerializeField,Range(0.0f,1.0f)]
    public float Volume{ get; private set;}
}
