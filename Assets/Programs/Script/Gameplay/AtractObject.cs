using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpolation;
using UnityEngine.Events;

public class AtractObject : MonoBehaviour
{
    [SerializeField]
    private float attractTime = 1f;
    [SerializeField]
    public Vector3 offset = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 swingAngle = new Vector3(30.0f,0.0f,0.0f);
    [SerializeField]
    private float swingRepeat = 3.0f;
    [SerializeField]
    Easing.Curve swingCurve = Easing.Curve.Linear;
    [SerializeField]
    Easing.Curve attractCurve = Easing.Curve.Linear;

    [SerializeField]
    UnityAction onAttractComplete;

    private GameObject target;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 startAngle;
    private float currentTime = 0f;
    private bool isAttracting = false;
    private bool isFollowing = false;
    private float progress = 0f;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = transform.position + offset;
        progress = 0f;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void Attract(GameObject target)
    {
        if(isFollowing) { isFollowing = false; return; }
        this.target = target;
        isAttracting = true;
        startPosition = target.transform.position;
        endPosition = transform.position + offset;
        currentTime = 0f;
        progress = 0f;
    }

    private void Update()
    {
        
    }

    public void OnAttractComplete()
    {
        onAttractComplete?.Invoke();
        isFollowing = true;
    }
}
