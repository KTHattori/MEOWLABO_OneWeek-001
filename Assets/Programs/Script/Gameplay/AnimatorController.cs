using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        // fetch the animator component
        TryGetComponent<Animator>(out animator);
        OnStart();
    }

    protected virtual void OnStart() {}

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate() {}

    public void SetAnimation(string animationName)
    {
        // set the animation
        animator.Play(animationName);
    }

    public void SetAnimation(string animationName, bool value)
    {
        // set the animation
        animator.SetBool(animationName, value);
    }

    public void SetAnimation(string animationName, float value)
    {
        // set the animation
        animator.SetFloat(animationName, value);
    }

    public void SetAnimation(string animationName, int value)
    {
        // set the animation
        animator.SetInteger(animationName, value);
    }

    public void SetAnimation(string animationName, string value)
    {
        // set the animation
        animator.SetTrigger(animationName);
    }

    public void SetAnimation(string animationName, Vector2 value)
    {
        // set the animation
        animator.SetFloat(animationName + "X", value.x);
        animator.SetFloat(animationName + "Y", value.y);
    }

    public void SetAnimation(string animationName, Vector3 value)
    {
        // set the animation
        animator.SetFloat(animationName + "X", value.x);
        animator.SetFloat(animationName + "Y", value.y);
        animator.SetFloat(animationName + "Z", value.z);
    }

    public void SetAnimation(string animationName, Vector4 value)
    {
        // set the animation
        animator.SetFloat(animationName + "X", value.x);
        animator.SetFloat(animationName + "Y", value.y);
        animator.SetFloat(animationName + "Z", value.z);
        animator.SetFloat(animationName + "W", value.w);
    }

    public void SetAnimation(string animationName, Quaternion value)
    {
        // set the animation
        animator.SetFloat(animationName + "X", value.x);
        animator.SetFloat(animationName + "Y", value.y);
        animator.SetFloat(animationName + "Z", value.z);
        animator.SetFloat(animationName + "W", value.w);
    }
}
