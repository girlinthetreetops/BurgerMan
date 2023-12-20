using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 20f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float playerRadius = .7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float reachDistance = 1.2f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask kitchenUnitLayerMask;

    //just for debugging really
    [SerializeField] public ClearCounter selectedCounter;
    [SerializeField] private bool isWalking;

    private Vector3 facingDirection;

    //events
    public UnityEvent OnSelectedCounterChanged;  

    private void Start()
    {
        //subscribe to the new onInteraction UnityEvent made in GameInput
        gameInput.OnInteractAction.AddListener(OnInteractAction);
    } 

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
 
    public void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 movementDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if (movementDirection != Vector3.zero) //keep track  of last interaction to keep detection even when movDirection is zero 
        {
            facingDirection = movementDirection;
        } 

        //Check for things on the kitchenUnitLayerMask (and get a data sheet)
        if (Physics.Raycast(transform.position, facingDirection, out RaycastHit raycastHit, reachDistance, kitchenUnitLayerMask))
        {
            //Check if its a ClearCounter, specifically
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //check if youve already made this your selected counter, and if not - select it!
                if (clearCounter != selectedCounter)
                {
                    setSelectedCounter(clearCounter);

                } else //if its been selected already, deselect it... ?
                {
                    setSelectedCounter(null);
                }
            } else //if its not a counter, we dont wat a selected counter
            {
                setSelectedCounter(null);
            }
        }
    }

    private void setSelectedCounter(ClearCounter passedCounter)
    {
        selectedCounter = passedCounter;
        OnSelectedCounterChanged.Invoke();
    }

    private void OnInteractAction()
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
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