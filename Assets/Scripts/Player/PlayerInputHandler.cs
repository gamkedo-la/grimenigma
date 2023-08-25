using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHanlder : MonoBehaviour
{
    [SerializeField] PlayerMovement pBody;
    [SerializeField] PlayerCameraControl pCamera;
    [SerializeField] PlayerWeaponHandeling pHandeling;
    [SerializeField] HealthController pHealth;
    [SerializeField] EquipmentHandler pEquipment;

    PlayerInput input;

    Vector2 moveInput, cameraInput;
    string controlType;

    void OnEnable()
    {
        input.Enable();
        input.Player.Camera.performed += GetCameraInput;
        input.Player.Camera.canceled += GetCameraInput;
    }

    void OnDisable()
    {
        input.Disable();
        input.Player.Camera.performed -= GetCameraInput;
        input.Player.Camera.canceled -= GetCameraInput;
    }

    void Awake()
    {
        input = new PlayerInput();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = input.Player.Movement.ReadValue<Vector2>();
        cameraInput = input.Player.Camera.ReadValue<Vector2>();

        pCamera.UpdateCameraRotation(cameraInput, controlType, moveInput);

        if(input.Player.Dash.IsInProgress()){ pBody.Dash(moveInput); }
        if(input.Player.Attack.IsInProgress()){ pEquipment.currentEquipment?.GetComponent<AttackController>().Attack(); }
        pBody.JumpHandler(input.Player.Jump.IsPressed(), input.Player.Jump.WasPressedThisFrame());
        if(input.Player.Camera.IsInProgress() || input.Player.Movement.IsInProgress()){ pHandeling.WeaponSway(cameraInput.normalized, moveInput); }
        else{ pHandeling.IdleAroundOrigin(); }

        if(input.Player.SwitchToNextWeapon.IsPressed()){ pEquipment.SelectNextEquipment(); }

        if(input.Player.PauseMenu.IsPressed()){
            Application.Quit();
            Debug.Log("Quit button clicked.");
        }

        if(Input.GetKeyDown(KeyCode.K)){ pHealth.Damage(1); }
        if(Input.GetKeyDown(KeyCode.H)){ pHealth.Heal(1); }
    }

    void FixedUpdate()
    {
        pBody.MovePlayer(moveInput);
    }

    void GetCameraInput(InputAction.CallbackContext obj)
    {
        controlType = obj.control.parent.displayName;
    }
}
