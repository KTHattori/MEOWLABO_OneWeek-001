using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInput : MonoBehaviour
{
    private Vector3 mousePosition = Vector3.zero;
    private List<GameObject> hitObjects = new List<GameObject>();

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
        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
            hitObjects.Clear();
            hitObjects.Add(hit.collider.gameObject);
        }
        return mousePosition;
    }

    public GameObject GetClickedObject()
    {
        if (hitObjects.Count > 0)
        {
            return hitObjects[0];
        }
        return null;
    }

    public bool GetMouseClicked()
    {
        return Input.GetMouseButtonDown(0);
    }
}
