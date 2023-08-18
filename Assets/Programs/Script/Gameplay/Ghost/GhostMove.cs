using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GhostMove : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 targetAngle = Vector3.zero;
    private Vector3 mousePoint = Vector3.zero;
    private Rigidbody rb = null;

    private void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void IndicatePoint(Vector3 position)
    {
        mousePoint = position;
    }

    public void Rotate()
    {
        targetAngle = mousePoint;
        
        transform.LookAt(targetAngle);
        if(transform.localEulerAngles.x > Ghost.instance.Move.tiltRange && transform.localEulerAngles.x < 360.0f - Ghost.instance.Move.tiltRange)
        {
            transform.localEulerAngles = new Vector3(Ghost.instance.Move.tiltRange, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    public void Move()
    {
        // 指定された方向にゆっくりとRigidbodyで移動する
        // 遠ければ遠いほど速く、近ければ近いほど遅く移動する
        if(rb.isKinematic) rb.isKinematic = false;
        targetPosition = mousePoint;
        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        float speed = Mathf.Clamp(Ghost.instance.Move.minSpeed, Ghost.instance.Move.maxSpeed, distance);
        rb.velocity = direction * speed;
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    public bool IsArrived()
    {
        return (targetPosition - transform.position).magnitude < Ghost.instance.Move.arrivedRange;
    }
}
