using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAction : MonoBehaviour
{
    Prop prop;
    private IPropAction[] actions = null;

    public Prop GetProp()
    {
        return prop;
    }

    void Reset()
    {
        prop = GetComponent<Prop>();
    }

    void Start()
    {
        prop = GetComponent<Prop>();
        actions = GetComponents<IPropAction>();
    }

    public void Action(Prop previousProp)
    {
        foreach(IPropAction action in actions)
        {
            action.Action(previousProp);
        }
    }

    public void Cancel(Prop nextProp)
    {
        foreach(IPropAction action in actions)
        {
            action.Cancel(nextProp);
        }
    }
}
