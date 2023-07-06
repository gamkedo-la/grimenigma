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

    [Header("Game Object Dependencies")]
    [SerializeField] Transform player;

    float xRotation, yRotation, mouseX, mouseY, horizontalSenseToUse, verticalSenseToUse;
    Vector2 cameraInput, lookDelta;
    string controlType;

    PlayerInput input;

    void OnEnable()
    {
        input.Enable();
        input.Player.Camera.performed += SetCameraVector;
        input.Player.Camera.canceled += SetCameraVector;
    }

    void OnDisable()
    {
        input.Disable();
        input.Player.Camera.performed -= SetCameraVector;
        input.Player.Camera.canceled -= SetCameraVector;
    }

    void Awake()
    {
        input = new PlayerInput();
    }
    
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
        
        RotateCamera();
        transform.position = new Vector3(player.position.x, (player.position.y + cameraHeight), player.position.z);
    }

    public void RotateCamera()
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
                                (cameraInput.x * verticalSenseToUse),
                                (cameraInput.y * horizontalSenseToUse)
                                );
                               
        //Debug.Log(lookDelta);
        mouseX = lookDelta.x * Time.deltaTime;
        mouseY = lookDelta.y * Time.deltaTime;

        yRotation += mouseX;
        xRotation = Mathf.Clamp((xRotation-mouseY), -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void SetCameraVector(InputAction.CallbackContext obj)
    {
        cameraInput = obj.ReadValue<Vector2>();
        controlType = obj.control.parent.displayName;
    }
}
