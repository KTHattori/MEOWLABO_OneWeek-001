using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RegisterObject.Register)), CreateAssetMenu(fileName = "new ObjectRelay", menuName = "ScriptableObject/Object Relay")]
public class SCO_ObjectRelay : ScriptableObject
{
    [field: SerializeField]
    public GameObject gameObject{get;set;}

    public void ActivateObject()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
}
