using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    //Movement Components
    private Animator animator;
    private CharacterController controller;

    private float moveSpeed = 4f;

    [Header("Movement system")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

    //Interaction components
    PlayerInteraction playerInteraction;
    void Start()
    {
        //Get movement components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //Get interaction component
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
    }

    private void FixedUpdate()
    {
        //Runs the function that handles all movement
        Move();

        //Run the function that handles all interaction
        Interact();

        //Debugging purposes only
        //Skip the time when the right square bracket is press
        if (Input.GetKey(KeyCode.RightBracket))
        {
            TimeManager.Instance.Tick();
        }
    }

    public void Interact()
    {
        //Tool interaction
        if (Input.GetButtonDown("Fire1"))
        {
            //Interact
            playerInteraction.Interact();
        }

        //Item interaction
        if (Input.GetButtonDown("Fire2"))
        {
            playerInteraction.ItemInteract();
        }
    }

    public void Move()
    {
        //Get the horizontal and vertical inputs as a number
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Direction in a nomalised vector
        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * dir;

        //Is the sprint key pressed down?
        if (Input.GetButton("Sprint"))
        {
            //Set the animation run aand increase our movespeed
            moveSpeed = runSpeed;
            animator.SetBool("Running", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("Running", false);
        }

        //Check if there is movement
        if (dir.magnitude >= 0.1f)
        {
            //Look towards that direction
            transform.rotation = Quaternion.LookRotation(dir);

            //Move
            controller.Move(velocity);
        }

        animator.SetFloat("Speed", velocity.magnitude);
    }
}
