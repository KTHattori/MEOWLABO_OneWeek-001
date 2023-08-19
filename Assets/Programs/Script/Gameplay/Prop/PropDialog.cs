using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDialog : MonoBehaviour,IPropAction
{
    [SerializeField] private SCO_PropDialog dialogData = null;

    public void Action()
    {
        DialogManager.instance.ChangeContent(dialogData.GetDataAtWeekDay(DayManager.instance.CurrentWeekDay));
    }

    public void Cancel()
    {
        DialogManager.instance.ClearContent();
    }
}
