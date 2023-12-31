/*
SOME NOTES:
    - To get snappy movement, modifying the players velocity directly OnMove() was decided on.
    - Checking GetKey and GetKeyDown in Jump() allows the player to hold the jump button for grounded jumps, but requires an additonal jump input to trigger the air jump.
*/

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovementcopy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;

    [Header("Jump Properties")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] float raycastPadding;
    [SerializeField] LayerMask whatIsGround;

    Rigidbody rb;
    PlayerInput input;

    Vector2 moveDirection;

    bool canJump = true;
    bool airJumpAvailable;
    float airMoveSpeed;

    bool grounded;
    float maxDistance;

    float horizontalInput;
    float verticalInput;


    void Awake()
    {
        // Component handling
        rb = GetComponent<Rigidbody>();
        input = new PlayerInput();

        // Calculate constants on instatiation.
        maxDistance = (playerHeight * 0.5f) + raycastPadding;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
    }
    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        GroundCheck();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        JumpStateHandler();
    }

    private void MovePlayer()
    {
        moveDirection = input.Player.Movement.ReadValue<Vector2>();

        // Move the player without modifying the up/down velocity. Modifying velocty it requires
        rb.velocity =  (transform.right * moveDirection.x + transform.forward * moveDirection.y) * moveSpeed + new Vector3(0f, rb.velocity.y);
        //Debug.Log("Player velocity:" + rb.velocity);
    }

    private void JumpStateHandler()
    {
        if(input.Player.Jump.IsPressed()){
            if(grounded && canJump){ Jump(); }
            else if (airJumpAvailable && input.Player.Jump.WasPressedThisFrame()){
                airJumpAvailable = false;
                Jump();
            }
            //else if(grounded && !canJump && input.Player.Jump.WasPressedThisFrame()){ Jump(); }
        }
        //Debug.Log("Jump input:" + input.Player.Jump.IsPressed());
    }

    private void Jump()
    {
        //Debug.Log("Jump!");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(JumpTimer());
    }

    IEnumerator JumpTimer()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void GroundCheck()
    {
        // Performs raycasts for 90, 45, and 135 degrees from player's facing position to check if player is gounded.
        // In theory, this should handle most slope cases, but the values may need tweeking.
        if(Physics.Raycast(transform.position, Vector3.down, maxDistance)){
            grounded = true;
            airJumpAvailable = true;
        }
        else{
            grounded = false;
        }
        //Debug.Log("Grounded:" + grounded);
    }
}
