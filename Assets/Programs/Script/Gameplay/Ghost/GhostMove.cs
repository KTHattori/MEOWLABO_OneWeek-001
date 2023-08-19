using System.Collections;
using System.Collections.Generic;
using Interpolation;
using UnityEngine;
using UnityEngine.Rendering;

public class GhostMove : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField]
    private Vector3 targetAngle = Vector3.zero;
    private Vector3 mousePoint = Vector3.zero;
    private Rigidbody rb = null;
    private float currentRotationTime = 0.0f;
    private float rotateProgress = 0.0f;
    

    private void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void IndicatePoint(Vector3 position)
    {
        if((mousePoint - position).magnitude > Ghost.instance.Move.arrivedRange)
        {
            mousePoint = position;
        }

        targetPosition = (mousePoint - transform.position) * Ghost.instance.Move.interpolationSpeed + transform.position;
    }

    public void Rotate()
    {
        targetAngle = targetPosition;
        Vector3 diff = targetAngle - transform.position;
        diff.Normalize();
        // forward と diff.y の内積を計算
        float dot = Vector3.Dot(transform.forward, new Vector3(0.0f,diff.y,0.0f));
        targetAngle.y = transform.position.y + diff.y * -Mathf.Clamp01(dot * 2);
        transform.LookAt(targetAngle);
    }

    public void Move()
    {
        // 指定された方向にゆっくりとRigidbodyで移動する
        // 遠ければ遠いほど速く、近ければ近いほど遅く移動する
        if(rb.isKinematic) rb.isKinematic = false;
        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        float speed = Mathf.Clamp(Ghost.instance.Move.minSpeed, Ghost.instance.Move.maxSpeed, distance);
        rb.velocity = direction * speed;
    }

    public void Stop()
    {
        rb.velocity = rb.velocity * 0.1f;
        rb.isKinematic = true;
    }

    public bool IsArrived()
    {
        return (targetPosition - transform.position).magnitude < Ghost.instance.Move.arrivedRange;
    }
}
