using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInteract : MonoBehaviour
{
    [SerializeField]
    PropAction target = null;
    [SerializeField]
    PropAction previousTarget = null;

    public GameObject TargetObject
    {
        get { return target.gameObject; }
    }

    public bool HasTarget()
    {
        if(target == null) { return false; }
        return true;
    }

    public bool IsTargetProp()
    {
        if(target == null) { return false; }
        if(target.GetComponent<PropAction>() != null)
        {
            return true;
        }
        return false;
    }

    public bool IsTargetInRange()
    {
        if(target == null) { return false; }
        if(Vector3.Distance(transform.position,target.transform.position) < Ghost.instance.Interact.range)
        {
            return true;
        }
        return false;
    }

    public void UpdateTarget(GameObject target)
    {
        if(target == null) { return; }
        if(target.TryGetComponent<PropAction>(out this.target))
        {
            
        }
        else
        {
            this.target = null;
        }
    }

    public void Interact()
    {
        if(target == null) { return; }
        if(previousTarget != null)
        {
            previousTarget.Cancel(target.GetProp());
            target.Action(previousTarget.GetProp());
        }
        target.Action(null);
        ResetTarget();
    }

    public void Cancel()
    {
        if(target == null) { return; }
        Debug.Log("Cancel");
        target.Cancel(previousTarget.GetProp());
        ResetTarget();
    }

    void ResetTarget()
    {
        previousTarget = target;
        target = null;
    }
}
