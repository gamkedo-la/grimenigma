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
    Vector2 lookDelta;

    PlayerInput input;

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Awake()
    {
        input = new PlayerInput();
        input.Player.Camera.performed += RotateCamera;
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
        
        //if(input.Player.Camera.inProgress){ RotateCamera(); }
        transform.position = new Vector3(player.position.x, (player.position.y + cameraHeight), player.position.z);
    }

    public void RotateCamera(InputAction.CallbackContext obj)
    {
        // Had to do combine these vector to get the mouse delta working while also listing for controller inputs. A problem to fix later.
        /*
        lookDelta = new Vector2(
                                (input.Player.Camera.ReadValue<Vector2>().x * stickVerticalSense) + (Mouse.current.delta.ReadValue().x * mouseVerticalSense),
                                (input.Player.Camera.ReadValue<Vector2>().y * stickHorizontalSense) + (Mouse.current.delta.ReadValue().y * mouseHorizontalSense)
                               );
        */

        Debug.Log(obj.control.parent.displayName);

        if(obj.control.parent.displayName == "Mouse"){ 
            horizontalSenseToUse = mouseHorizontalSense;
            verticalSenseToUse = mouseVerticalSense;
        }
        else if(obj.control.parent.displayName == "whatever string gamepads have"){ 
            horizontalSenseToUse = stickHorizontalSense;
             verticalSenseToUse = stickVerticalSense;
        }
        // Else statement can be removed after gamepad string is added
        else{
            horizontalSenseToUse = stickHorizontalSense;
            verticalSenseToUse = stickVerticalSense;
        }

        lookDelta = new Vector2(
                                (input.Player.Camera.ReadValue<Vector2>().x * verticalSenseToUse),
                                (input.Player.Camera.ReadValue<Vector2>().y * horizontalSenseToUse)
                                );
                               
        //Debug.Log(lookDelta);
        mouseX = lookDelta.x * Time.deltaTime;
        mouseY = lookDelta.y * Time.deltaTime;

        yRotation += mouseX;
        xRotation = Mathf.Clamp((xRotation-mouseY), -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
