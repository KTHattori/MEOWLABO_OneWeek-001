using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropMove : MonoBehaviour
{
    private Prop prop;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 targetAngle = Vector3.zero;
    private Vector3 mousePoint = Vector3.zero;
    private Rigidbody rb = null;
    private void Start()
    {
        prop = GetComponent<Prop>();
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void IndicatePoint(Vector3 position)
    {
        mousePoint = position;
    }

    public void Rotate()
    {
        // 指定された方向にゆっくりと回転する
        // X軸の回転は自身のいる位置との高さによって変化する

        // 向く方向を計算
        targetAngle = mousePoint - transform.position;

        // 向く方向の高さを計算
        float height = targetAngle.y;

        // 向く方向の高さを無視した平面上の角度を計算
        targetAngle.y = 0.0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetAngle);

        // 向く方向の高さを考慮した角度を計算
        targetRotation *= Quaternion.Euler(height * Ghost.instance.Move.tiltRange, 0.0f, 0.0f);

        // 自身の角度を計算
        Quaternion currentRotation = transform.rotation;

        // 向く方向と自身の角度の差を計算
        Quaternion diff = targetRotation * Quaternion.Inverse(currentRotation);

        // 向く方向と自身の角度の差をゆっくりと自身の角度に加える
        transform.rotation *= Quaternion.Slerp(Quaternion.identity, diff, Time.deltaTime);

    }

    public void Move()
    {
        // 指定された方向にゆっくりとRigidbodyで移動する
        targetPosition = mousePoint;
        // rb.velocity = (targetPosition - transform.position).normalized * prop.Move.speed;
    }
}
