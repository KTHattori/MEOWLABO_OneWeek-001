using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f;
    public bool isInverted = false;
    public int skipFrame = 0;
    public int currentFrameCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(skipFrame == 0)
        {
            Rotate(isInverted);
            return;
        }
        
        if(currentFrameCount < skipFrame)
        {
            currentFrameCount++;
            return;
        }
        else
        {
            currentFrameCount = 0;
        }
        for(int i = 0;i < skipFrame;i++)
        {
            Rotate(isInverted);
        }
    }

    void Rotate(bool invert)
    {
        if(invert)
        {
            transform.RotateAround(target.position, Vector3.up, -speed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        }
    }
}
