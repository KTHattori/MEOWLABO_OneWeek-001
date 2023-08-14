using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カーソルの当たっている3D空間上の位置にゆっくりと追従させる
// デバッグのために当たっている3D空間上の位置とオブジェクト名を表示する

public class GhostMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float range = 1.0f;

    private Vector3 targetPosition = Vector3.zero;
    private Vector3 targetAngle = Vector3.zero;

    public Vector3 mousePosition = Vector3.zero;
    public List<GameObject> hitObjects = new List<GameObject>();

    

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        // カメラからマウスカーソルの位置に向かう光線を飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
            hitObjects.Clear();
            hitObjects.Add(hit.collider.gameObject);
            Debug.Log(hit.collider.gameObject.name);
        }

        // カーソルの当たっている3D空間上の位置に回転させる
        transform.LookAt(mousePosition, Vector3.up);

        // カーソルの当たっている3D空間上の位置にゆっくりと追従させる
        targetPosition = Vector3.Lerp(targetPosition, mousePosition, Time.deltaTime * Ghost.instance.Move.speed);

        transform.position = targetPosition;
    }
}
