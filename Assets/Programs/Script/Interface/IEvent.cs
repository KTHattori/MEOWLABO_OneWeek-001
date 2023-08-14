using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICustomEvent
{
    void Execute(float time = 0f,bool force = false);
}
