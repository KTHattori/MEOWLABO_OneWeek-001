using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PropAction))]
public class PropMove : MonoBehaviour, IPropAction
{
    Prop prop = null;
    [SerializeField]
    float moveSpeed = 2.5f;
    private Vector3 targetAngle = Vector3.zero;
    private Collider col = null;
    private Rigidbody rb = null;
    bool isMoving = false;
    
    private Transform TargetLocator
    {
        get
        {
            return PropManager.instance.TargetLocator;
        }
    }

    private void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponentInChildren<Rigidbody>();
        prop = GetComponent<Prop>();
    }

    private void Update()
    {
        if(isMoving)
        {
            Rotate();
            Move();
        }
    }

    public void Action(Prop previous)
    {
        isMoving = true;
        col.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        prop.SetHold(true);      
    }

    public void Cancel(Prop next)
    {
        isMoving = false;
        col.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        prop.SetHold(false);
    }

    void Rotate()
    {
        // 指定された方向にゆっくりと回転する
        // X軸の回転は自身のいる位置との高さによって変化する

        // 向く方向を計算
        targetAngle = TargetLocator.position - transform.position;

        // 向く方向の高さを計算
        float height = targetAngle.y;

        // 向く方向の高さを無視した平面上の角度を計算
        targetAngle.y = 0.0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetAngle);

        // 向く方向の高さを考慮した角度を計算
        targetRotation *= Quaternion.Euler(height * 30.0f, 0.0f, 0.0f);

        // 自身の角度を計算
        Quaternion currentRotation = transform.rotation;

        // 向く方向と自身の角度の差を計算
        Quaternion diff = targetRotation * Quaternion.Inverse(currentRotation);

        // 向く方向と自身の角度の差をゆっくりと自身の角度に加える
        transform.rotation *= Quaternion.Slerp(Quaternion.identity, diff, Time.deltaTime);
    }

    void Move()
    {
        // 指定された方向にゆっくりとRigidbodyで移動する
        transform.position = Vector3.Lerp(transform.position, TargetLocator.position, Time.deltaTime * moveSpeed);
    }
}
