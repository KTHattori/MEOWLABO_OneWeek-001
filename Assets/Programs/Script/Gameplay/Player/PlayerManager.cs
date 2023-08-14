using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private enum PlayerState
    {
        Idle,
        Walk,
        Interact
    }

    [SerializeField] private MovePlayer movePlayer;
    [SerializeField] private AnimatePlayer animatePlayer;
    [SerializeField] private PlayerInputManager playerInput;
    [SerializeField] private Interactor interactor;
    private PlayerState playerState = PlayerState.Idle;

    public DebugPlayer debugPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // fetch all the components
        TryGetComponent<MovePlayer>(out movePlayer);
        TryGetComponent<AnimatePlayer>(out animatePlayer);
        TryGetComponent<PlayerInputManager>(out playerInput);
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is walking
        if (playerInput.GetInput().magnitude > 0)
        {
            // set the player state
            playerState = PlayerState.Walk;
        }
        else
        {
            // set the player state
            playerState = PlayerState.Idle;
        }

        // check if the player is interacting
        if (playerInput.GetInteract())
        {
            if(interactor.TryInteract())
            {
                // set the player state
                playerState = PlayerState.Interact;
            }
        }

        // check the player state
        switch (playerState)
        {
            case PlayerState.Idle:
                // set the animation
                animatePlayer.Idle();

                // set the debug text
                debugPlayer.Idle();

                break;
            case PlayerState.Walk:
                // set the animation
                animatePlayer.Walk();
                // move the player
                movePlayer.Walk(playerInput.GetInput());

                // set the debug text
                debugPlayer.Walk();

                break;
            case PlayerState.Interact:
                // set the animation
                animatePlayer.Interact();

                // set the debug text
                debugPlayer.Interact();
                break;
        }
    }
}
