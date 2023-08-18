using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInput : MonoBehaviour
{
    private Vector3 mousePosition = Vector3.zero;
    private GameObject hitObject = null;

    public Vector3 MousePosition { get => mousePosition; }
    void Start()
    {
        mousePosition = Vector3.zero;
    }
    void Update()
    {
        
    }

    public Vector3 GetMousePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity))
        {
            mousePosition = hit.point;
            hitObject = hit.collider.gameObject;
        }
        return mousePosition;
    }

    public GameObject GetHitObject()
    {
        return hitObject;
    }

    public bool GetMouseClicked()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetMouseMoved()
    {
        // カーソルが移動したかどうか
        return Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f;
    }
}
