using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    protected bool isInteracting = false;
    protected bool isInteractable = false;
    [SerializeField] protected List<GameObject> interactablesInRange = new List<GameObject>();
    [SerializeField] protected Interactable nearestInteractable = null;

    public GameObject NearestObject
    {
        get
        {
            if(nearestInteractable == null) return null;
            return nearestInteractable.gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void UpdateNearestInteractable()
    {
        float minDistance = Mathf.Infinity;
        GameObject nearestObj = null;
        foreach(GameObject interactable in interactablesInRange)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestObj = interactable;
            }
        }

        if(nearestObj != null)
        {
            isInteractable = true;
            nearestInteractable?.SetNearestFlag(false);
            nearestInteractable = nearestObj.GetComponent<Interactable>();
            nearestInteractable.SetNearestFlag(true);
        }
        else
        {
            isInteractable = false;
            nearestInteractable?.SetNearestFlag(false);
            nearestInteractable = null;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Interactable")
        {
            Debug.Log(other.name);
            interactablesInRange.Add(other.gameObject);
            UpdateNearestInteractable();
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if(other.tag == "Interactable")
        {
            Debug.Log(other.name);
            interactablesInRange.Remove(other.gameObject);
            UpdateNearestInteractable();
        }
    }

    protected void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Interactable"))
        {
            Debug.Log(other.gameObject.name);
            interactablesInRange.Add(other.gameObject);
            UpdateNearestInteractable();
        } 
    }

    protected void OnCollisionExit(Collision other) {
        if(other.gameObject.CompareTag("Interactable"))
        {
            Debug.Log(other.gameObject.name);
            interactablesInRange.Remove(other.gameObject);
            UpdateNearestInteractable();
        } 
    }

    public bool TryInteract()
    {
        if(!isInteractable) return false;
        return nearestInteractable.TryInteract();
    }
}
