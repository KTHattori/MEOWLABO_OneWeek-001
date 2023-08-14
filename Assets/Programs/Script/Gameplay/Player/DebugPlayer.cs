using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPlayer : MonoBehaviour
{
    public TextMeshPro text;
    public bool drawGizmos = true;
    // Start is called before the first frame update
    void Start()
    {
        text.SetText("");
    }

    void Update()
    {
        // keep always the text towards the camera and invert the rotation
        text.transform.rotation = Quaternion.LookRotation(text.transform.position - Camera.main.transform.position);
    }

    public void Idle()
    {
        text.SetText("Idle");
    }

    public void Walk()
    {
        text.SetText("Walk");
    }

    public void Interact()
    {
        text.SetText("Interact");
    }

    private void OnDrawGizmos() {
        if(!drawGizmos) return;

        // Draw player's front
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2);

        // Draw player's interact range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2);

        // Draw player's ground check
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 1, 0), 0.1f);
    }
}
