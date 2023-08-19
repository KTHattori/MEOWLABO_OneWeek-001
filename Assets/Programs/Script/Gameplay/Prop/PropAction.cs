using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAction : MonoBehaviour
{
    private IPropAction[] actions = null;

    void Start()
    {
        actions = GetComponents<IPropAction>();
    }

    public void Action()
    {
        foreach(IPropAction action in actions)
        {
            action.Action();
        }
    }
}
