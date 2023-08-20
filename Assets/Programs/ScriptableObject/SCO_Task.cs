using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "ScriptableObject/Task")]
public class SCO_Task : ScriptableObject
{
    [field:SerializeField]
    public string TaskName { get; private set;} = "タスク名";

    [field:SerializeField,Multiline(5)]
    public string TaskDescription { get; private set;} = "タスクの説明";

    [field:SerializeField]
    public SCO_ObjectRelay deliverTarget { get; private set;} = null;

    [field:SerializeField]
    public Sprite Icon{get;private set;} = null;

    [field:SerializeField]
    public bool IsCompleted { get; set;} = false;

    public Prop DeliverTargetProp { get{return deliverTarget.gameObject.GetComponent<Prop>();}}
}
