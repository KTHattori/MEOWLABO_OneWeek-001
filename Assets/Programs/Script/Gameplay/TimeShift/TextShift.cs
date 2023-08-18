using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShift : MonoBehaviour, ITimeShiftTarget
{
    // TimeShiftに応じて設定されたテキストを徐々に表示する
    [SerializeField]
    private TMPro.TextMeshProUGUI text = null;
    private int currentDisplayIndex = 0;
    [SerializeField, TextArea(3, 10)]
    private string originalText = "";

    public TMPro.TextMeshProUGUI asadayo = null;

    void Reset()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();

        if(text == null)
        {
            Debug.LogError("TextMeshProUGUI is not found.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetIndex(int index)
    {
        if(index < 0) index = 0;
        if(index > originalText.Length) index = originalText.Length;

        currentDisplayIndex = index;
        text.text = originalText.Substring(0, currentDisplayIndex);
    }

    public void SetProgress(float progress)
    {
        // progressが0のときは何も表示せず
        // progressが1のときに全文を表示する
        // それ以外のときは、progressに応じて表示する文字数を変える
        if(progress <= 0.0f)
        {
            SetIndex(0);
        }
        else if(progress >= 1.0f)
        {
            SetIndex(originalText.Length);
            asadayo.SetText("あさだよ");
        }
        else
        {
            SetIndex(Mathf.FloorToInt(originalText.Length * progress));
        }
    }
}
