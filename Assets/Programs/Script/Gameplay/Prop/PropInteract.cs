using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropInteract : MonoBehaviour
{
    private IPropAction[] actions = null;

    void Start()
    {
        actions = GetComponents<IPropAction>();
    }

    public void Interact()
    {
        foreach(IPropAction action in actions)
        {
            action.Action();
        }
    }
}
