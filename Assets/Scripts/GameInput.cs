 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameInput : MonoBehaviour
{

    private PlayerInputActions playerInputActions;

    //create an event that triggers when interact is triggered
    public UnityEvent OnInteractAction;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        //Here I am adding a function subscription to a specific user input (Interact aka E/space)
        //for movement, it makes sense to check it on update every frame.
        //but interactions are less common! So we want to use this inbuilt event thing to be notified when it might happen.
        //this += adds a listener to this event that is triggered by the input system. 
        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction.Invoke();
    }

    public Vector2 GetNormalizedMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    //legacy solution
    //    public Vector2 GetNormalizedMovementVector()
    //    {
    //        Vector2 inputVector = new Vector2(0,0);

    //        if (Input.GetKey(KeyCode.W))
    //        {
    //            inputVector.y = +1;
    //        }
    //        if (Input.GetKey(KeyCode.S))
    //        {
    //            inputVector.y = -1;
    //        }
    //        if (Input.GetKey(KeyCode.A))
    //        {
    //            inputVector.x = -1;
    //        }
    //        if (Input.GetKey(KeyCode.D))
    //        {
    //            inputVector.x = +1;
    //        }

    //        inputVector = inputVector.normalized;

    //        return inputVector;
    //    }
    //}

}