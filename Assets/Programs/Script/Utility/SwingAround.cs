using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAround : MonoBehaviour
{
    public Transform target;
    public float swingTime = 3.0f;
    public float angle = 60.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }
}
