/*
SOME NOTES:
    - To get snappy movement, modifying the players velocity directly OnMove() was decided on.
    - Checking GetKey and GetKeyDown in Jump() allows the player to hold the jump button for grounded jumps, but requires an additonal jump input to trigger the air jump.
*/

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpeedController))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] SpeedController movement;

    [Header("Jump Properties")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] float raycastPadding;
    [SerializeField] LayerMask whatIsGround;

    Rigidbody rb;

    Vector2 moveDirection;

    bool canJump = true;
    bool airJumpAvailable;
    float airMoveSpeed;

    bool grounded;
    float maxDistance;

    float horizontalInput;
    float verticalInput;

    public void JumpHandler(bool jumpIsPressed, bool jumpWasPressThisFrame)
    {
        if(jumpIsPressed){
            if(grounded && canJump){ Jump(); }
            else if (airJumpAvailable && jumpWasPressThisFrame){
                airJumpAvailable = false;
                Jump();
            }
            //else if(grounded && !canJump && input.Player.Jump.WasPressedThisFrame()){ Jump(); }
        }
        //Debug.Log("Jump input:" + input.Player.Jump.IsPressed());
    }

    
    public void MovePlayer(Vector2 moveDirection)
    {
        // Move the player without modifying the up/down velocity. Modifying velocty it requires
        rb.velocity =  (transform.right * moveDirection.x + transform.forward * moveDirection.y) * movement.speed + new Vector3(0f, rb.velocity.y);
        //Debug.Log("Player velocity:" + rb.velocity);
    }

    void Awake()
    {
        // Component handling
        rb = GetComponent<Rigidbody>();

        // Calculate constants on instatiation.
        maxDistance = (playerHeight * 0.5f) + raycastPadding;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
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

}
