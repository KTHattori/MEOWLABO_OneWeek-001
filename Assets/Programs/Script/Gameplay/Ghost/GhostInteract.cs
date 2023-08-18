using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInteract : MonoBehaviour
{
    private Interactor interactor;
    private GameObject target = null;

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

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
    

}
