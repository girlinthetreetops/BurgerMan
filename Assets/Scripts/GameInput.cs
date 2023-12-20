using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    //legacy solution
    public Vector2 GetNormalizedMovementVector()
    {
        Vector2 inputVector = new Vector2(0,0);

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }

        inputVector = inputVector.normalized;

        return inputVector;
    }
}

//new system solution

//public class GameInput : MonoBehaviour
//{

//    private PlayerInputActions playerInputActions;

//    private void Awake()
//    {
//        playerInputActions = new PlayerInputActions();
//        playerInputActions.Player.Enable();
//    }

//    public Vector2 GetNormalizedMovementVector()
//    {
//        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
//        inputVector = inputVector.normalized;
//        return inputVector;
//    }
//}