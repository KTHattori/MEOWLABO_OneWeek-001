using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI text = null;
    private int currentDisplayIndex = 0;
    [SerializeField, TextArea(5, 10)]
    private string contentText = "";

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


    void UpdateDisplay(float progress)
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
            SetIndex(contentText.Length);
        }
        else
        {
            SetIndex(Mathf.FloorToInt(contentText.Length * progress));
        }
    }

    void SetIndex(int index)
    {
        if(index < 0) index = 0;
        if(index > contentText.Length) index = contentText.Length;

        currentDisplayIndex = index;
        text.SetText(contentText.Substring(0, currentDisplayIndex));
    }

    public void SetProgress(float progress)
    {
        UpdateDisplay(progress);
    }

    public void ChangeContent(string text)
    {
        contentText = text;
        SetProgress(0.0f);
    }

    
    public void ClearContent()
    {
        ChangeContent("");
    }
}
