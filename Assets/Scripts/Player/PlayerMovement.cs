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
    [HideInInspector] public bool hasLandedThisCycle;

    [Header("Required Scripts")]
    [SerializeField] PlayerData pData;
    [SerializeField] MovementSoundController moveSounds;

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

    [Header("Audio")]
    [SerializeField] AudioSource sourceAudio;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip dashAvailableSound;
    [SerializeField] AudioClip landingSound;
    [SerializeField] AudioClip slidingSound;
    [SerializeField] AudioClip footstepSound;

    [Header("Bodge Fixes")]
    [Range(0f,300f)][SerializeField] float extraGravity;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] float raycastPadding;

    Rigidbody rb;
    [SerializeField] Transform cam;

    bool canJump, canDash, slideAvailable, airJumpAvailable, shouldPlaySlideSound;
    float speed, airMoveSpeed, slideTime, maxDistance;
    bool grounded;
    float horizontalInput, verticalInput, graceJumpCounter, footstepCounter;
    float graceJumpTime = 0.2f;
    

    public void Dash(Vector2 moveInput)
    {
        if(canDash){
            //Debug.Log("Dash:" + canDash);
            Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
            rb.AddForce(moveDirection * dashForce, ForceMode.Impulse);
            PlayAudioClip(dashSound);
            StartCoroutine(RunDashTimer());
        }
    }

    public void JumpHandler(bool jumpIsPressed, bool jumpWasPressThisFrame)
    {
        if(jumpIsPressed){
            if(graceJumpCounter > 0 && canJump){ 
                Jump(); 
                graceJumpCounter = 0;
            }
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

        if(grounded && (rb.velocity.x > 0.3f || rb.velocity.x < -0.3f || rb.velocity.z > 0.3f || rb.velocity.z < -0.3f)){
            if(footstepCounter <= 0){
                moveSounds.PlaySound(MovementStyle.Running);
                footstepCounter = 0.35f;
            } else { footstepCounter -= Time.deltaTime; }
        }
    }

    public void Slide(Vector2 moveInput, bool wasSlideReleased){
        ApplyExtraGravity();

        if(wasSlideReleased){
            slideTime = 0f;
            shouldPlaySlideSound = true;
            //Debug.Log("Slide Time:" + slideTime);
            StartCoroutine(RunSlideTimer());
        }
        else if(slideAvailable){
            if(slideTime <= allowedSlideTime){
                if(shouldPlaySlideSound){
                    PlayAudioClip(slidingSound);
                    shouldPlaySlideSound = false;
                }
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
        shouldPlaySlideSound = true;
    }

    void FixedUpdate()
    {
        GroundCheck();
        ApplyExtraGravity();
        
        if(!grounded) { pData.landingVelocity = rb.velocity; }
        MovePlayer();
    }

    void Update()
    {
        transform.rotation = cam.transform.rotation;
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
            if(!grounded){
                pData.hasLandedThisCycle = true;
                graceJumpCounter -= Time.deltaTime;
                if(-pData.landingVelocity.y > pData.hardLandingThreshold){  moveSounds.PlaySound(MovementStyle.Landing, 0.8f); }
                else{ moveSounds.PlaySound(MovementStyle.Landing); }
            }
            else{ pData.hasLandedThisCycle = false; }

            grounded = true;
            graceJumpCounter = graceJumpTime;
            airJumpAvailable = true;
        }
        else{
            grounded = false;
            pData.hasLandedThisCycle = false;
        }
        //Debug.Log("Grounded:" + grounded);
    }

    private void Jump()
    {
        //Debug.Log("Jump!");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(RunJumpTimer());
    }

    private void PlayAudioClip(AudioClip sound)
    {
        sourceAudio.pitch = Random.Range(0.9f, 1.1f);
        sourceAudio.PlayOneShot(sound);
    }

    private void PlayAudioClip(AudioClip sound, float pitch)
    {
        sourceAudio.pitch = pitch;
        sourceAudio.PlayOneShot(sound);
    }

    IEnumerator RunDashTimer()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        PlayAudioClip(dashAvailableSound);
        canDash = true;
    }

    IEnumerator RunJumpTimer()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    IEnumerator RunSlideTimer()
    {
        slideAvailable = false;
        yield return new WaitForSeconds(slideCooldown);
        slideAvailable = true;
    }

}
