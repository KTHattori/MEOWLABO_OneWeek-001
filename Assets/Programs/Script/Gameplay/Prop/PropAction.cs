using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Action()
    {
        Debug.Log("Action");
    }

    public virtual void Cancel()
    {
        Debug.Log("Cancel");
    }


}