using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 20f;
    [SerializeField] private float rotateSpeed = 30f;

    [SerializeField] private GameInput gameInput;

    private bool isWalking;

    private float playerRadius = .7f;
    private float playerHeight = 2f;

    private void Update()
    {
        //vector storing input collected from gameInput class
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();

        //set a vector3 movement Direction based on the vector2 input received
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);


        //parameters: bottom and top of capsule, radius, direction, move distnace. Returns true if it hits something and false if not
        float moveDistance = playerSpeed * Time.deltaTime; //stores the length away from the capsule that you would be moving and which thus must be checked
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        //allow slide-type movement if youre colliding and canMove returns false
        if (!canMove)
        {
            //attempt to move at least on the x axis. Change the movedirection to one that ignores your Z axis input, and then check again:
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            //if you CAN move on X, change the direction to the one that was accepted and do it!
            if (canMove)
            {
                moveDirection = moveDirectionX;
            } else { //attempt to move along the Z instead
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if(canMove)
                {
                    moveDirection = moveDirectionZ;
                } else
                {
                    Debug.Log("youre stuck");
                }
            }
        }

        //movePlayer
        if (canMove)
        {
            transform.position += moveDirection * playerSpeed * Time.deltaTime;
        }

        //rotate player
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        //set isWalking based on whether moveDirection is zero
        isWalking = moveDirection != Vector3.zero;
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}