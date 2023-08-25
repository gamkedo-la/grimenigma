/*
SOME NOTES:
    - To get snappy movement, modifying the players velocity directly OnMove() was decided on.
    - Checking GetKey and GetKeyDown in Jump() allows the player to hold the jump button for grounded jumps, but requires an additonal jump input to trigger the air jump.
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpeedController))]

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Vector3 moveInput;
    [HideInInspector] public bool isSliding;

    [Header("Movement")]
    [SerializeField] SpeedController movement;
    [Range(0.1f,1f)][SerializeField] float crouchModifier;
    [Range(0f,8f)][SerializeField] float rigidBodyDrag;

    [Header("Jump")]
    [Range(50f,150f)][SerializeField] float jumpForce;
    [Range(0f,5f)][SerializeField] float jumpCooldown;
    
    [Header("Dash")]
    [Range(100f,500f)][SerializeField] float dashForce;
    [Range(0f,5f)][SerializeField] float dashCooldown;

    [Header("Slide")]
    [Range(0f,10f)][SerializeField] float slideForce;
    [Range(0f, 3f)][SerializeField] float allowedSlideTime;
    [Range(0f,5f)][SerializeField] float slideCooldown;

    [Header("Bodge Fixes")]
    [Range(0f,300f)][SerializeField] float extraGravity;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] float raycastPadding;
    [SerializeField] LayerMask whatIsGround;

    Rigidbody rb;

    bool canJump, canDash, slideAvailable, airJumpAvailable;
    float speed, airMoveSpeed, slideTime, maxDistance;
    bool grounded;
    float horizontalInput, verticalInput;

    public void Dash(Vector2 moveInput)
    {
        if(canDash){
            //Debug.Log("Dash:" + canDash);
            Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
            rb.AddForce(moveDirection * dashForce, ForceMode.Impulse);
            StartCoroutine(DashTimer());
        }
    }

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

    
    public void MovePlayer()
    {
        if(isSliding){speed = movement.speed * crouchModifier; }
        else{ speed = movement.speed; }

        Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
        rb.AddForce(moveDirection * speed, ForceMode.Acceleration);
        //Debug.Log("Player velocity:" + rb.velocity);
    }

    public void Slide(Vector2 moveInput, bool wasSlideReleased){
        ApplyExtraGravity();

        if(wasSlideReleased){
            slideTime = 0f;
            //Debug.Log("Slide Time:" + slideTime);
            StartCoroutine(SlideTimer());
        }
        else if(slideAvailable){
            if(slideTime <= allowedSlideTime){
                //Debug.Log("Slide Time:" + slideTime);
                Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
                rb.AddForce(moveDirection * slideForce, ForceMode.Impulse);
                //Debug.Log("Sliding!");

                slideTime += Time.deltaTime;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Component handling
        rb = GetComponent<Rigidbody>();
        rb.drag = rigidBodyDrag;

        // Calculate constants on instatiation.
        maxDistance = (playerHeight * 0.5f) + raycastPadding;
        rb.freezeRotation = true;
    }

    void OnEnable()
    {
        canDash = true;
        canJump = true;
        slideAvailable = true;
    }

    void FixedUpdate()
    {
        GroundCheck();
        ApplyExtraGravity();
        MovePlayer();
    }

    private void ApplyExtraGravity()
    {
        if(!grounded){ rb.AddForce(new Vector3(0, -extraGravity, 0)); }
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

    IEnumerator DashTimer()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator JumpTimer()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    IEnumerator SlideTimer()
    {
        slideAvailable = false;
        yield return new WaitForSeconds(slideCooldown);
        slideAvailable = true;
    }

}
