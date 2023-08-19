using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSelect : MonoBehaviour
{
    private Vector3 mousePosition = Vector3.zero;
    private GameObject hitObject = null;
    private GameObject hitProp = null;

    public Vector3 MousePosition { get => mousePosition; }
    void Start()
    {
        mousePosition = Vector3.zero;
    }
    void Update()
    {
        
    }

    public void RaycastTerrain()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = 1 << LayerMask.NameToLayer("Ghost") | 1 << LayerMask.NameToLayer("Ignore Raycast");
        mask = ~mask;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity,mask))
        {
            mousePosition = hit.point;
            hitObject = hit.collider.gameObject;
            Debug.Log(hitObject.name);
        }
        else
        {
            hitObject = null;
        }
    }

    public void RaycastProp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity,PropManager.instance.SelectableLayerMask))
        {
            hitProp = hit.collider.gameObject;
        }
        else
        {
            hitProp = null;
        }
        PropManager.UpdateHighlighted(hitProp);
    }

    public Vector3 GetMousePoint()
    {
        return mousePosition;
    }

    public GameObject GetHitObject()
    {
        return hitObject;
    }

    public GameObject GetHitProp()
    {
        return hitProp;
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
