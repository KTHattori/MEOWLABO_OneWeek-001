using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPropAction
{
    public void Action(Prop previousProp = null);

    public void Cancel(Prop nextProp = null);
}
