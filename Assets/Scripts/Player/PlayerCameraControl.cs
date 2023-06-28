using UnityEngine;

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
        transform.position = new Vector3(player.position.x, (player.position.y + cameraHeight), player.position.z);
    }

    public void RotateCamera(Vector2 cameraInput){
        mouseX = cameraInput.x * Time.deltaTime * verticalSense;
        mouseY = cameraInput.y * Time.deltaTime * horizontalSense;

        yRotation += mouseX;
        xRotation = Mathf.Clamp((xRotation-mouseY), -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
