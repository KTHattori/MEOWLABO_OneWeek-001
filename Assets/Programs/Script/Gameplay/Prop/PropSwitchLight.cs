using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PropAction))]
public class PropSwitchLight : MonoBehaviour,IPropAction
{
    [SerializeField]
    Light targetLight = null;
    [SerializeField]
    float delay = 0.0f;
    [SerializeField]
    bool isOn = false;

    void Reset()
    {
        targetLight = GetComponentInChildren<Light>();
    }

    void Start()
    {
        if(targetLight == null)
        {
            targetLight = GetComponentInChildren<Light>();
        }
    }

    void SwitchLight()
    {
        targetLight.enabled = !targetLight.enabled;
        isOn = targetLight.enabled;
    }

    public void Action()
    {
        Invoke("SwitchLight",delay);
        Debug.Log("SwitchLight");
    }

    public void Cancel()
    {
        return;
    }
}
