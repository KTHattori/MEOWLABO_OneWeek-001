using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PropAction))]
public class PropDialog : MonoBehaviour,IPropAction
{
    [SerializeField] private SCO_PropDialog dialogData = null;

    public void Action(Prop previousProp)
    {
        DialogManager.instance.ChangeContent(dialogData.GetDataAtWeekDay(DayManager.instance.CurrentWeekDay));
    }

    public void Cancel(Prop nextProp)
    {
        DialogManager.instance.ClearContent();
    }
}
