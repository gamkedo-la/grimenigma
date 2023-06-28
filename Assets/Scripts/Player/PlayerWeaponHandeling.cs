using UnityEngine;

public class PlayerWeaponHandeling : MonoBehaviour
{
    [Header("Position Sway")]
    [SerializeField] float step = 0.01f;
    [SerializeField] float maxPositionStep = 0.06f;
    [SerializeField] float smoothing = 10f;
    Vector3 swayPosition;
    float maxPositionX; 
    float maxPositionY;
    float maxPositonZ;

    [Header("Rotation Sway")]
    [SerializeField] float rotationStep = 4f;
    [SerializeField] float maxRotationStep = 10f;
    [SerializeField] float rotationSmoothing = 12f;
    Vector3 swayEulerRotation;
    float yRotation;

    [Header("Movement Sway")]
    [SerializeField] float moveDrag = 1f;

    [Header("Idle Sway")]
    [SerializeField] float idlePositionMax = 10f;
    [SerializeField] float idleRotationMax = 5f;

    Vector3 originPosition;
    Quaternion originRotation;

    void Awake(){
        originPosition = transform.localPosition;
        originRotation = transform.localRotation;
        maxPositionX = originPosition.x + maxPositionStep;
        maxPositionY = originPosition.y + maxPositionStep;
        maxPositonZ = originPosition.z + maxPositionStep;
    }

    public void WeaponSway(Vector2 cameraInput, Vector2 moveInput){
        swayPosition = new Vector3(
            transform.localPosition.x + (cameraInput.x * -step) + (moveInput.x * -moveDrag),
            transform.localPosition.y + (cameraInput.y * -step),
            transform.localPosition.z + (moveInput.y * -moveDrag)
            );

            /*
            Currently breaks swaying movement.
            Mathf.Clamp((transform.localPosition.x + (cameraInput.x * -step) + (moveInput.x * -moveDrag)), -maxPositionX, maxPositionX),
            Mathf.Clamp((transform.localPosition.y + (cameraInput.y * -step)), -maxPositionY, maxPositionY),
            Mathf.Clamp((transform.localPosition.z + (moveInput.y * -moveDrag)), -maxPositonZ, maxPositonZ));
            */

        yRotation = transform.localPosition.y + (cameraInput.x * -rotationStep);
        swayEulerRotation = new Vector3(
            transform.localPosition.x + (cameraInput.y * -rotationStep),
            yRotation,
            yRotation
            );
        
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPosition, Time.deltaTime * smoothing);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRotation), Time.deltaTime * rotationSmoothing);
    }

/*
OLD
    public void CameraSway(Vector2 cameraInput)
    {
        swayPosition = new Vector3(transform.localPosition.x + (cameraInput.x * -step),
                                   transform.localPosition.y + (cameraInput.y * -step),
                                   transform.localPosition.z);

        yRotation = transform.localPosition.y + (cameraInput.x * -rotationStep);
        swayEulerRotation = new Vector3(transform.localPosition.x + (cameraInput.y * -rotationStep),
                                        yRotation,
                                        yRotation);
        
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPosition, Time.deltaTime * smoothing);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRotation), Time.deltaTime * rotationSmoothing);
    }

    public void MovementSway(Vector2 moveInput){
        swayPosition = new Vector3(transform.localPosition.x + (moveInput.x * -moveDrag),
                                   transform.localPosition.y,
                                   transform.localPosition.z + (moveInput.y * -moveDrag));
        
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPosition, Time.deltaTime * smoothing);
    }
*/

public void IdleAroundOrigin(){
        // TO DO MAKE SWAY AROUND ORIGIN
        transform.localPosition = Vector3.Lerp(transform.localPosition, originPosition, Time.deltaTime * smoothing);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotation, Time.deltaTime * rotationSmoothing);
    }
}
