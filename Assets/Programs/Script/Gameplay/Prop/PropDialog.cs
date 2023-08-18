using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDialog : PropAction
{
    [SerializeField]
    private string dialog = "";
    [SerializeField]
    private float duration = 1.0f;

    public override void Action()
    {
        DialogManager.instance.ChangeContent(dialog, duration);
    }

    public override void Cancel()
    {
        DialogManager.instance.ClearContent();
    }
}
