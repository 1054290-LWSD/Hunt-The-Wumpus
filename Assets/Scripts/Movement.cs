using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    
    //Put camera in unity. should be at top of head.
    public Camera playerCamera;
    private Rigidbody rb;
    private float speed = 12f;

    //Gravity rate at which player is pulled down (25f and 30f is pretty good)
    private float gravity = 25f;
    private float jumpStrength = 30f;

    //Mouse Sensitivity
    private float lookSpeed = 1.5f;
    //The max you can look up or down in degrees, 90 is straight up and down.
    private float lookYLimit = 90f;
    
    public bool ground = false;
    private float ogCoyoteTime = 0.5f;
    private float coyoteTime = 0f;


    private float curSpeedX;
    private float curSpeedY;

    //The Dot Product [-1,1] the player is looking each of the directions.
    public float xLook = 0f;
    public float yLook = 0f;
    public float zLook = 0f;

    public float fps = 0f;
    //deltaTime is the time since last frame, used for physics calculations and other things
    private float deltaTime = 0f;

    private float rotationX = 0;
    
    //Might be useless, don't delete
    private CharacterController characterController;

    //The sphere used for checking if player is grounded and the layer in which the grounded state is
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public bool canMove = true;
    public PauseMenu isPaused;
    void Start()
    {
        //rb = Rigidbody, look at Rigidbody decleration for more info.
        rb = GetComponent<Rigidbody>();
        
        //Don't worry about or change these, not useful, don't delete it.
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {        
        //Assigns delta time and checks FPS
        deltaTime = Time.deltaTime;
        fps = 1 / deltaTime;

        //Calculates Coyote Time (More info at variable decleration)
        if (!ground)
        {
            coyoteTime -= deltaTime;
        }
        else
        {
            coyoteTime = ogCoyoteTime;
        }
        
        //currents Speed of x and y for the camera rotation
        curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
        curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;


        //Changes camera and gets camera angles Dot Product
        if (canMove)
        {
            //Moves Camera
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookYLimit, lookYLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        //Gets cameras Dot Product [-1,1]
        yLook = -1 * (float)Math.Sin((Math.PI) * (playerCamera.transform.rotation.eulerAngles.x < 0 ? playerCamera.transform.rotation.eulerAngles.x + 180 : playerCamera.transform.rotation.eulerAngles.x) / 180);
        zLook = (float)Math.Cos((Math.PI) * transform.rotation.eulerAngles.y / 180);
        xLook = (float)Math.Sin((Math.PI) * transform.rotation.eulerAngles.y / 180); 
    }
    void FixedUpdate()
    {
        //Assigns the time since last frame, (delta time)
        float fixedDeltaTime = deltaTime;
        if (fixedDeltaTime < Time.deltaTime) {
            fixedDeltaTime = Time.deltaTime;
        }

        //Velocity is used to help with movement, the current velocity in each dirrection
        Vector3 velocity = rb.velocity;

        //Used for seeing if isGrounded while playing in dev mode.
        ground = IsGrounded();

        //assigns controller/keyboards directions for horizontal and vertical axis's [-1,1]
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Applies gravity if mid air, and sets vertical velocity to jump velocity if grounded and jumps
        if (!IsGrounded())
        {
            //Makes gravity go faster if you are falling instead of rising, makes jump feel a lot better
            if (velocity.y < 1) velocity.y -= gravity * deltaTime * 1.5f;
            velocity.y -= gravity * deltaTime;
        }
        //Checks coyote time and jump, if true let's player jump
        if (Input.GetButton("Jump") && canMove && coyoteTime > 0)
        {
            coyoteTime = 0f;
            velocity.y = jumpStrength;
        }
        
        
        
        // Main Movement System:
        // If is less than movement speed let's you move up till running speed. If above, applies slight drag or extra drag if grounded
        float xMove = velocity.x;
        float zMove = velocity.z;
        //air Drag is also ground drag
        float airDrag = 0;
        
        //If grounded, has more airDrag
        if (IsGrounded())
        {
            //Log helps make the airDrag more consistent between different frame rates
            airDrag = (float)(-0.9 * Math.Log(fixedDeltaTime, 120));
        }
        else
        {
            //Log helps make the airDrag more consistent between different frame rates
            airDrag = (float)(-1.05 * Math.Log(fixedDeltaTime, 170));
        }

        //If velocity is above a little bit over max speed, it will not let you move, but you will still slow down
        if (Math.Abs(velocity.x) <= speed * 1.1)
        {
            //Movement from vertical (W and S or Up and Down on controller)
            xMove = xLook * vertical * speed;
            //Movement from horizontal (A and D or Left and Right on controller)
            xMove += zLook * horizontal * speed;

            ///If the movement added puts you above max speed, instead just puts you at max speed
            if (xMove > speed) xMove = speed;
            if (-1 * xMove > speed) xMove = -1 * speed; //Negative version of above
        }
        
        //Same as X valued ones but for Y
        if (Math.Abs(velocity.z) <= speed * 1.1)
        {
            zMove = zLook * vertical * speed;
            zMove += -1 * xLook * horizontal * speed;
            if (zMove > speed) zMove = speed;
            if (-1 * zMove > speed) zMove = -1 * speed;
        }
        //Applies airDrag
        xMove *= airDrag;
        zMove *= airDrag;
        //applies vertical airdrag if velocity is going up
        if (velocity.y > 0) velocity.y *= airDrag;
        //If velocity is to low it will zero out, prevents tiny permanent micro movements.
        if (Math.Abs(velocity.x) < 0.05) velocity.x = 0;
        if (Math.Abs(velocity.z) < 0.05) velocity.z = 0;

        //applies current velocity in game to the rigid body.
        rb.velocity = new Vector3(xMove, velocity.y, zMove);
    }

    //Checks if you are actually grounded
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.45f, groundLayer);
    }
}   
