using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    protected bool isInteractable = false;
    protected bool isNearest = false;
    
    [SerializeField]
    private Material material = null;

    void Reset()
    {
        material = GetComponentInChildren<MeshRenderer>().sharedMaterial;
        gameObject.tag = "Interactable";
    }

    void Start()
    {
        SetNearestFlag(false);
        SetInteractable(true);
        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    public bool TryInteract()
    {
        if(isInteractable)
        {
            SuccessInteract();
            return true;
        }
        else
        {
            FailedInteract();
            return false;
        }
    }

    protected virtual void SuccessInteract()
    {
        Debug.Log(gameObject + "Interacted");
    }

    protected virtual void FailedInteract()
    {
        Debug.Log(gameObject + "Failed to Interact");
    }

    public void SetInteractable(bool value)
    {
        isInteractable = value;
        if(value)
        {
            OnEnabledInteract();
        }
        else
        {
            OnDisabledInteract();
        }
    }

    public void EnableInteract()
    {
        OnEnabledInteract();
    }

    public void DisableInteract()
    {
        OnDisabledInteract();
    }

    protected virtual void OnEnabledInteract()
    {
        
    }

    protected virtual void OnDisabledInteract()
    {
        
    }

    public void SetNearestFlag(bool flag = true)
    {
        isNearest = flag;
        if(flag)
        {
            OnComeNearest();
        }
        else
        {
            OnLeaveNearest();
        }
    }

    protected virtual void OnComeNearest()
    {

    }

    protected virtual void OnLeaveNearest()
    {

    }
}
