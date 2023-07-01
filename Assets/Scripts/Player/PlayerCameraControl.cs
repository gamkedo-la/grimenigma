using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraControl : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] float verticalSense = 400;
    [SerializeField] float horizontalSense = 400;
    [SerializeField] float cameraHeight = 32;

    [Header("Game Object Dependencies")]
    [SerializeField] Transform player;

    float xRotation;
    float yRotation;
    Vector2 mouseDelta;
    float mouseX;
    float mouseY;
    
    // Start is called before the first frame update
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update(){
        // Move to player position.
        RotateCamera();
        transform.position = new Vector3(player.position.x, (player.position.y + cameraHeight), player.position.z);
    }

    public void RotateCamera()
    {
        mouseDelta = Mouse.current.delta.ReadValue();
        Debug.Log(mouseDelta);
        mouseX = mouseDelta.x * Time.deltaTime * verticalSense;
        mouseY = mouseDelta.y * Time.deltaTime * horizontalSense;

        yRotation += mouseX;
        xRotation = Mathf.Clamp((xRotation-mouseY), -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
