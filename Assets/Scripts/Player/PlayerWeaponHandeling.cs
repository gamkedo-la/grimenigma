using UnityEngine;

public class PlayerWeaponHandeling : MonoBehaviour
{
    [Header("Position Sway")]
    [SerializeField] float step = 0.01f;
    [SerializeField] float maxPositionStep = 0.3f;
    [SerializeField] float maxDistanaceFromCamera = 0.3f;
    [SerializeField] float smoothing = 10f;
    Vector3 swayPosition;
    float minX, minY, minZ, maxX, maxY, maxZ;

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
        minX = transform.localPosition.x - maxPositionStep;
        minY = transform.localPosition.y - maxPositionStep;
        minZ = transform.localPosition.z - maxDistanaceFromCamera;
        maxX = transform.localPosition.x + maxPositionStep;
        maxY = transform.localPosition.y + maxPositionStep;
        maxZ = (transform.localPosition.z + maxDistanaceFromCamera)*0.75f;

        //Debug.Log(new Vector3(minX, minY, minZ));
        //Debug.Log(new Vector3(maxX, maxY, maxZ));
    }

    public void WeaponSway(Vector2 cameraInput, Vector2 moveInput){

        swayPosition = new Vector3(
            Mathf.Clamp((originPosition.x + (cameraInput.x * -step) + (moveInput.x * -moveDrag)), minX, maxX),
            Mathf.Clamp((originPosition.y + (cameraInput.y * -step)), minY, maxY),
            Mathf.Clamp((originPosition.z + (moveInput.y * -moveDrag)), minZ,maxZ)
            );
            

        yRotation = transform.localPosition.y + (cameraInput.x * -rotationStep);
        swayEulerRotation = new Vector3(
            transform.localPosition.x + (cameraInput.y * -rotationStep),
            yRotation,
            yRotation
            );
        
        //Debug.Log(swayPosition);
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPosition, Time.deltaTime * smoothing);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRotation), Time.deltaTime * rotationSmoothing);
    }

    public void IdleAroundOrigin(){
            // TO DO MAKE SWAY AROUND ORIGIN
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPosition, Time.deltaTime * smoothing);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotation, Time.deltaTime * rotationSmoothing);
        }
}
