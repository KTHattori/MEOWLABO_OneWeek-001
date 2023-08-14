using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;

    // Hold camera
    [SerializeField] private Transform cameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Walk(Vector2 input)
    {
        // determine the direction to move by using the camera
        Vector3 direction = cameraTransform.forward * input.y + cameraTransform.right * input.x;
        // remove the y component
        direction.y = 0;
        // normalize the direction
        direction.Normalize();
        // calculate the movement
        Vector3 move = direction * speed;
        // apply the movement
        controller.Move(move * speed * Time.deltaTime);
    }

    public void Fall()
    {
        // calculate the fall velocity
        Vector3 fallVelocity = Vector3.up * gravity;
        // apply the fall velocity
        controller.Move(fallVelocity * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        // check if the player is on the ground
        return controller.isGrounded;
    }
}
