using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    public Vector2 GetInput()
    {
        // return the input
        return new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
    }

    public bool GetInteract()
    {
        // return the input
        return Input.GetKeyDown(interactKey);
    }

    public bool GetInteractHold()
    {
        // return the input
        return Input.GetKey(interactKey);
    }

    public bool GetInteractRelease()
    {
        // return the input
        return Input.GetKeyUp(interactKey);
    }

    public bool GetHorizontal()
    {
        // return the input
        return Input.GetAxis(horizontalAxis) != 0;
    }

    public bool GetVertical()
    {
        // return the input
        return Input.GetAxis(verticalAxis) != 0;
    }

    public bool GetHorizontalHold()
    {
        // return the input
        return Input.GetAxis(horizontalAxis) != 0;
    }

    public bool GetVerticalHold()
    {
        // return the input
        return Input.GetAxis(verticalAxis) != 0;
    }

    public bool GetHorizontalRelease()
    {
        // return the input
        return Input.GetAxis(horizontalAxis) == 0;
    }

    public bool GetVerticalRelease()
    {
        // return the input
        return Input.GetAxis(verticalAxis) == 0;
    }

    public bool GetHorizontalPositive()
    {
        // return the input
        return Input.GetAxis(horizontalAxis) > 0;
    }

    public bool GetVerticalPositive()
    {
        // return the input
        return Input.GetAxis(verticalAxis) > 0;
    }

    public bool GetHorizontalNegative()
    {
        // return the input
        return Input.GetAxis(horizontalAxis) < 0;
    }

    public bool GetVerticalNegative()
    {
        // return the input
        return Input.GetAxis(verticalAxis) < 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
