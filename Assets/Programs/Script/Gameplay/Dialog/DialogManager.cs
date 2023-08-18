using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoSingleton<DialogManager>
{
    public enum DialogState
    {
        StandBy,
        Displaying,
        Finished,
        Clearing,
    }

    [SerializeField]
    private GameObject dialogUI = null;

    [SerializeField]
    DialogBox dialogBox = null;

    [SerializeField]
    const float DefaultDuration = 1.0f;

    private DialogState state = DialogState.StandBy;

    private float timeToDisplayAll = DefaultDuration;
    private float timeToClear = DefaultDuration;

    private float progress = 0.0f;

    public void ChangeContent(string content, float duration = DefaultDuration)
    {
        dialogBox.ChangeContent(content);
        timeToDisplayAll = duration;
        progress = 0.0f;
        state = DialogState.Displaying;
    }

    public void ClearContent(float duration = DefaultDuration)
    {
        timeToClear = duration;
        state = DialogState.Clearing;
    }

    public DialogState GetState()
    {
        return state;
    }

    void SetState(DialogState state)
    {
        this.state = state;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeContent("Hello World");
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            ClearContent();
        }
#endif
        
        // stateがDisplayingのときに進行度を進める
        if(state == DialogState.Displaying)
        {
            progress += Time.deltaTime / timeToDisplayAll;
            dialogBox.SetProgress(progress);

            if(progress >= 1.0f)
            {
                state = DialogState.Finished;
            }
        }

        // stateがClearingのときにカウントダウンを進める
        if(state == DialogState.Clearing)
        {
            progress += Time.deltaTime / timeToClear;
            dialogBox.SetProgress(1.0f - progress);

            if(progress >= 1.0f)
            {
                state = DialogState.StandBy;
            }
        }
    
    }
}
