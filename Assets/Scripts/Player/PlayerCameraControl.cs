using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraControl : MonoBehaviour
{
    [HideInInspector] public bool isSliding, hasLanded;

    [Header("Required Scripts")]
    [SerializeField] PlayerData pData;

    [Header("Camera Settings")]
    [SerializeField] Camera pCamera;
    //[SerializeField] float mouseVerticalSense = 5;
    //[SerializeField] float mouseHorizontalSense = 5;
    //[SerializeField] float stickVerticalSense = 200;
    //[SerializeField] float stickHorizontalSense = 200;
    [SerializeField] float cameraHeight = 32;

    [Header("Bob/Tilting")]
    [Range(0f,6f)][SerializeField] float landingOffsetStrength;
    [Range(0f, 1f)][SerializeField] float landingOffsetSteps;
    [Range(0f,6f)][SerializeField] float hardLandingOffsetStrength;
    [Range(0f, 1f)][SerializeField] float hardLandingOffsetSteps;
    [Range(0f, 10f)][SerializeField] float tiltStrength;
    [Range(0f, 1f)][SerializeField] float tiltSteps;
    [Range(0f, 5f)][SerializeField] float slideOffsetStrength;
    [Range(0f, 1f)][SerializeField] float slideSteps;

    [Header("Game Object Dependencies")]
    [SerializeField] Transform player;

    bool isHandlingLanding;

    float hardLandingThreshold;

    float xRotation, yRotation, mouseX, mouseY, horizontalSenseToUse, verticalSenseToUse, slideOffset, landingOffset;
    Vector2 lookDelta, lastMoveVector;
    Quaternion cameraRotationThisFrame;

    float slideOffsetTarget, landingOffsetTarget;
    Vector2 moveInputLast = Vector2.zero;
    Quaternion moveInputRotation = new Quaternion();

    void OnEnable()
    {
        pData.onRefreshFOV += SetFOV;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pCamera.fieldOfView = PlayerPrefs.GetFloat("fov", PlayerPrefsDefault.Floats["fov"]);
        hardLandingThreshold = pData.hardLandingThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHandlingLanding();
        SetSlideOffset();
        SetLandingOffset();

        moveInputRotation = Quaternion.Slerp(moveInputRotation, MovementTilt(transform.localRotation, moveInputLast), tiltSteps);
        slideOffset = Mathf.Lerp(slideOffset, slideOffsetTarget, slideSteps);
        landingOffset = Mathf.Lerp(landingOffset, landingOffsetTarget, landingOffsetSteps);

        // Move to player position.
        transform.position = new Vector3(player.position.x, (player.position.y + cameraHeight - slideOffset - landingOffset), player.position.z);
    }

    void FixedUpdate()
    {
        //moveInputRotation = Quaternion.Slerp(moveInputRotation, MovementTilt(transform.localRotation, moveInputLast), tiltSteps);
        //slideOffset = Mathf.Lerp(slideOffset, slideOffsetTarget, slideSteps);
        //landingOffset = Mathf.Lerp(landingOffset, landingOffsetTarget, landingOffsetSteps);
    }

    public void UpdateCameraRotation(Vector2 cameraInput, string controlType, Vector2 moveInput)
    {
        Quaternion cameraInputRotation = new Quaternion();
        cameraInputRotation = RotateCamera(transform.localRotation, cameraInput, controlType);
        
        transform.localRotation = cameraInputRotation * moveInputRotation;
        moveInputLast = moveInput;
        lastMoveVector = moveInput;
    }

    void SetLandingOffset()
    {
        if(!isHandlingLanding){
            switch (pData.hasLandedThisCycle)
            {
                case true:
                    if(-pData.landingVelocity.y >= hardLandingThreshold){ landingOffsetTarget = hardLandingOffsetStrength; }
                    else{ landingOffsetTarget = landingOffsetStrength; }
                    isHandlingLanding = true;
                    break;
                default:
                    landingOffsetTarget = 0f;
                    break;
            }
        }
    }

    void SetSlideOffset()
    {
        switch (isSliding)
        {
            case true:
                slideOffsetTarget = slideOffsetStrength;
                break;
            default:
                slideOffsetTarget = 0f;
                break;
        }
    }

    void CheckHandlingLanding()
    {
        if(landingOffsetTarget - landingOffset <= 0.1f) { isHandlingLanding = false; }
    }
    
    private Quaternion MovementTilt(Quaternion rotation, Vector2 moveInput)
    {
        // Check if horizontal input has changed
        if(moveInput.x != lastMoveVector.x){ }

        return Quaternion.Euler(rotation.x, rotation.z, moveInput.x * tiltStrength);
    }

    private Quaternion RotateCamera(Quaternion rotation, Vector2 cameraInput, string controlType)
    {
        //Debug.Log(pData.mouseHorizontalSensativity + " " + pData.mouseVerticleSensativity);
        //Debug.Log(obj.control.parent.displayName);
        if(controlType == "Mouse"){ 
            horizontalSenseToUse = pData.mouseHorizontalSensativity;
            verticalSenseToUse = pData.mouseVerticleSensativity;
        }
        else if(controlType == "whatever string gamepads have"){ 
            horizontalSenseToUse = pData.gamepadHorizontalSensativity;
            verticalSenseToUse = pData.gamepadVerticleSensativity;
        }
        // Else statement can be removed after gamepad string is added
        else{
            horizontalSenseToUse = pData.gamepadHorizontalSensativity;
            verticalSenseToUse = pData.gamepadVerticleSensativity;
        }

        lookDelta = new Vector2(
                                cameraInput.x * verticalSenseToUse,
                                cameraInput.y * horizontalSenseToUse
                                );
                               
        //Debug.Log(lookDelta);
        mouseX = lookDelta.x * Time.deltaTime;
        mouseY = lookDelta.y * Time.deltaTime;

        yRotation += mouseX;
        xRotation = Mathf.Clamp((xRotation-mouseY), -90f, 90f);
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);
        return rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    void SetFOV(float fov)
    {
        pCamera.fieldOfView = fov;
    }
}
