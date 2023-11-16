using UnityEngine;

public class RotateAndBobObject : MonoBehaviour
{
    public float rotationSpeed = 10f; // Set your desired rotation speed here
    public float bobbingSpeed = 0.5f; // Set your desired bobbing speed here
    public float bobbingAmount = 0.5f; // Set your desired bobbing amount here
    public Vector3 rotationAxis = Vector3.forward; // Set your desired rotation axis here
    
    private float bobbingOffset;
    private Vector3[] initialChildPositions;
    private Quaternion[] initialRotations;

    private void OnEnable()
    {
        // Store initial positions and rotations of child objects
        initialChildPositions = new Vector3[transform.childCount];
        initialRotations = new Quaternion[transform.childCount];
        bobbingOffset = Random.Range(0f, 2f * Mathf.PI); // Random offset between 0 and 2π
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            initialChildPositions[i] = child.position;
            initialRotations[i] = child.rotation;
        }
    }

    private void Update()
    {
        // Rotate and bob each child object
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float newY = initialChildPositions[i].y + Mathf.Sin(Time.time * bobbingSpeed + bobbingOffset) * bobbingAmount;
            child.position = new Vector3(child.position.x, newY, child.position.z);
            child.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }
}
