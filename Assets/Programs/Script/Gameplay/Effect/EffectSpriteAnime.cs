using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpriteAnime : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            AnimeEnd();
        }
    }
    public void AnimeEnd()
    {
        Destroy(gameObject);
    }
}
