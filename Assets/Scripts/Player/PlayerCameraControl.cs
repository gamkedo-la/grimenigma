using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraControl : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] float mouseVerticalSense = 5;
    [SerializeField] float mouseHorizontalSense = 5;
    [SerializeField] float stickVerticalSense = 200;
    [SerializeField] float stickHorizontalSense = 200;
    [SerializeField] float cameraHeight = 32;

    [Header("Bob/Tilting")]
    [SerializeField] float tiltStrength;

    [Header("Game Object Dependencies")]
    [SerializeField] Transform player;

    float xRotation, yRotation, mouseX, mouseY, horizontalSenseToUse, verticalSenseToUse;
    Vector2 lookDelta, lastMoveVector;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Move to player position.
        transform.position = new Vector3(player.position.x, (player.position.y + cameraHeight), player.position.z);
    }

    public void UpdateCameraRotation(Vector2 cameraInput, string controlType, Vector2 moveInput)
    {
        Quaternion cameraInputRotation = new Quaternion();
        Quaternion moveInputRotation = new Quaternion();
        cameraInputRotation = RotateCamera(transform.localRotation, cameraInput, controlType);
        moveInputRotation = MovementTilt(transform.localRotation, moveInput);
        transform.localRotation = cameraInputRotation * moveInputRotation;

        lastMoveVector = moveInput;
    }
    
    private Quaternion MovementTilt(Quaternion rotation, Vector2 moveInput)
    {
        // Check if horizontal input has changed
        if(moveInput.x != lastMoveVector.x){ }

        return Quaternion.Euler(rotation.x, rotation.z, moveInput.x * tiltStrength);
    }

    private Quaternion RotateCamera(Quaternion rotation, Vector2 cameraInput, string controlType)
    {
        //Debug.Log(obj.control.parent.displayName);
        if(controlType == "Mouse"){ 
            horizontalSenseToUse = mouseHorizontalSense;
            verticalSenseToUse = mouseVerticalSense;
        }
        else if(controlType == "whatever string gamepads have"){ 
            horizontalSenseToUse = stickHorizontalSense;
            verticalSenseToUse = stickVerticalSense;
        }
        // Else statement can be removed after gamepad string is added
        else{
            horizontalSenseToUse = stickHorizontalSense;
            verticalSenseToUse = stickVerticalSense;
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
}
