using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropDialog", menuName = "ScriptableObject/PropDialog")]
public class SCO_PropDialog : ScriptableObject
{
    [System.Serializable]
    public class Dialog
    {
        [field:SerializeField,Multiline(5),Tooltip("テキスト")]
        public string content = "ここにテキストを入力";
        [SerializeField,Tooltip("表示時間")]
        public float duration = 1.0f;
        [SerializeField,Tooltip("文字数")]
        public int length = 0;

        public void CountCharacters()
        {
            length = content.Length;
        }
    }

    [field:SerializeField,Header("ターゲット")]
    public string TargetProp{get;private set;} = "";

    [field:SerializeField] public Dialog Monday{get;private set;}
    [field:SerializeField] public Dialog Tuesday{get;private set;}
    [field:SerializeField] public Dialog Wednesday{get;private set;}
    [field:SerializeField] public Dialog Thursday{get;private set;}
    [field:SerializeField] public Dialog Friday{get;private set;}

    public Dialog GetDataAtWeekDay(WeekDay weekDay)
    {
        switch(weekDay)
        {
            case WeekDay.Monday:
                return Monday;
            case WeekDay.Tuesday:
                return Tuesday;
            case WeekDay.Wednesday:
                return Wednesday;
            case WeekDay.Thursday:
                return Thursday;
            case WeekDay.Friday:
                return Friday;
            default:
                return null;
        }
    }

    void OnValidate()
    {
        Monday.CountCharacters();
        Tuesday.CountCharacters();
        Wednesday.CountCharacters();
        Thursday.CountCharacters();
        Friday.CountCharacters();
    }

}
