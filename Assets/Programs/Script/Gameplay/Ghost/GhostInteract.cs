using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInteract : MonoBehaviour
{
    private Interactor interactor;

    public bool IsExistNearestObject
    {
        get
        {
            return interactor.NearestObject != null;
        }
    }


    private void Start()
    {
        interactor = GetComponent<Interactor>();
    }
    

}
