using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovement pBody;
    [SerializeField] PlayerCameraControl pCamera;
    [SerializeField] PlayerWeaponHandeling pHandeling;
    [SerializeField] HealthController pHealth;

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
    }

    // Update is called once per frame
    void Update()
    {
        pBody.JumpHandler(input.Player.Jump.IsPressed(), input.Player.Jump.WasPressedThisFrame());
        if(input.Player.Camera.IsInProgress() || input.Player.Movement.IsInProgress()){
            pCamera.RotateCamera(input.Player.Camera.ReadValue<Vector2>());
            pHandeling.WeaponSway(input.Player.Camera.ReadValue<Vector2>(), input.Player.Movement.ReadValue<Vector2>());
        }
        else{
            pHandeling.IdleAroundOrigin();
        }

        if(Input.GetKeyDown(KeyCode.K)){ pHealth.Damage(1); }
        if(Input.GetKeyDown(KeyCode.H)){ pHealth.Heal(1); }
    }

    void FixedUpdate()
    {
        pBody.MovePlayer(input.Player.Movement.ReadValue<Vector2>());
    }
}
