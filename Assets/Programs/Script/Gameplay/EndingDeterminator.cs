using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDeterminator : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    AnimationClip badEnding;
    [SerializeField]
    AnimationClip goodEnding;

    void Start ()
    {

    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
        if(DayManager.IsRemainingZero())
        {
            animator.Play(goodEnding.name);
        }
        else
        {
            animator.Play(badEnding.name);
        }
    }

    public void BackToTitle()
    {
        SceneController.TransitToSceneFade("Title_AfterEnd",1.0f,Interpolation.Easing.Style.Quart,Color.black,null,Transition.IOType.OutIn);
    }
}
