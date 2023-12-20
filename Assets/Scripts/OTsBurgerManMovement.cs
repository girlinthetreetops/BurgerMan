using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTsBurgerManMovement : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private Vector2 inputVector;
    [SerializeField] private float speed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        GetInputWithKeys();
        MovePlayer();
    }

    private void GetInputWithKeys()
    {
        Vector2 input = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            input.y = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input.x = +1;
        }
        inputVector = input.normalized;
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.position += moveDirection * speed * Time.deltaTime;

        //manipulating Z (forward) axis to avoid rotation management.
        //Slerp is made of a slurp between whatever is forward, and what is your movedirection!
        // slerp is linear for circular interpolation
         transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * speed);
    }
}
