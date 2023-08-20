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

    private float timer = 0.0f;
    private bool isInteracted = false;

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
        timer = 0.0f;
        isInteracted = false;
    }

    void Update()
    {
        if(targetLight == null) return;
        if(!isInteracted) return;
        if(timer < delay)
        {
            timer += Time.deltaTime;
            return;
        }
        else
        {
            timer = 0.0f;
            SwitchLight();
            isInteracted = false;
        }
    }

    void SwitchLight()
    {
        targetLight.enabled = !targetLight.enabled;
        isOn = targetLight.enabled;
        Debug.Log(isOn);
    }

    public void Action(Prop previousProp)
    {
        timer = 0.0f;
        isInteracted = true;
    }

    public void Cancel(Prop nextProp)
    {
        return;
    }
}
