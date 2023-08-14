using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayer : AnimatorController
{
    void OnStart()
    {
        // set the animation
        SetAnimation("Idle");
    }

    // Update is called once per frame
    void OnUpdate()
    {
        
    }

    public void Idle()
    {
        // set the animation
        SetAnimation("Idle");
    }

    public void Walk()
    {
        // set the animation
        SetAnimation("Walk");
    }

    public void Interact()
    {
        // set the animation
        SetAnimation("Interact");
    }

    public void Fall()
    {
        // set the animation
        SetAnimation("Fall");
    }



}
