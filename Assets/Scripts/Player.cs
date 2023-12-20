using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 20f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float playerRadius = .7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float reachDistance = 1.2f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask kitchenUnitLayerMask;

    private bool isWalking;
    private Vector3 facingDirection;

    private void Start()
    {
        //subscribe to the new onInteraction event...
        gameInput.OnInteractAction.AddListener(OnInteractAction);
    }

    private void Update()
    {
        HandleMovement();
    }

    private void OnInteractAction()
    {
        //This is neat - this event is triggered by the Gameinput script, meaning its not called each frame like movement and collisions. 
        //new raycast direction that isnt amended if you crash, like the movement one
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 movementDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        //keep track of last interaction to keep detection even when movDirection is zero 
        if (movementDirection != Vector3.zero)
        {
            facingDirection = movementDirection;
        }

        //Check if getting anything with a variant of raycast that returns more than a bool. it defines a new variable of RaycastHit called raycastHit and giving it directly to the function immediately to use is afterwards...The raycasthit will be filled with data from what you detect! Last thing is the layermask we check. 
        if (Physics.Raycast(transform.position, facingDirection, out RaycastHit raycastHit, reachDistance, kitchenUnitLayerMask))
        {
            //Check if its a clearcounter and interact if so
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
    }

    private void HandleMovement()
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
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            //if you CAN move on X, change the direction to the one that was accepted and do it!
            if (canMove)
            {
                moveDirection = moveDirectionX;
            }

            else
            { //attempt to move along the Z instead
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
                else
                {
                    Debug.Log("youre truly stuck");
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