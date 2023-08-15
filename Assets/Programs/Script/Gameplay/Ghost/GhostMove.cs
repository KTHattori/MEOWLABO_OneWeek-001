using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 targetAngle = Vector3.zero;
    private void Start()
    {
        targetPosition = transform.position;
    }

    public void Rotate(Vector3 position)
    {
        // positionで指定された方向を向く
        targetAngle = position;
        transform.LookAt(targetAngle);
    }

    public void Move(Vector3 position)
    {
        // カーソルの当たっている3D空間上の位置にゆっくりと追従させる
        targetPosition = Vector3.Lerp(targetPosition, position, Time.deltaTime * Ghost.instance.Move.speed);
        transform.position = targetPosition;
    }
}
