using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHanlder : MonoBehaviour
{
    [SerializeField] PlayerData pData;
    [SerializeField] PlayerMovement pBody;
    [SerializeField] PlayerCameraControl pCamera;
    [SerializeField] PlayerWeaponHandeling pHandeling;
    [SerializeField] HealthController pHealth;
    [SerializeField] EquipmentHandler pEquipment;
    [SerializeField] PlayerSelectHandAnimations pHandAnims;

    PlayerInput input;

    bool isSliding;
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

        pBody.moveInput = moveInput;
        pCamera.UpdateCameraRotation(cameraInput, controlType, moveInput);

        if(input.Player.Dash.IsInProgress()){ pBody.Dash(moveInput); }
        // TO DO: Touch up slide check logic.
        if(input.Player.Slide.IsInProgress()){
            pBody.isSliding = input.Player.Slide.IsInProgress();
            pCamera.isSliding = input.Player.Slide.IsInProgress();
            pBody.Slide(moveInput, input.Player.Slide.WasReleasedThisFrame());
            
        }
        else if(input.Player.Slide.WasReleasedThisFrame()){
            pBody.isSliding = input.Player.Slide.IsInProgress();
            pCamera.isSliding = input.Player.Slide.IsInProgress();
            pBody.Slide(moveInput, input.Player.Slide.WasReleasedThisFrame());
        }
        if(input.Player.LeftAttack.IsInProgress()){
            pData.leftItem?.GetComponent<AttackController>().Attack();
            pHandAnims.PlayAttackAnim(Hand.Left);
        }
        if(input.Player.RightAttack.IsInProgress()){
            pData.rightItem?.GetComponent<AttackController>().Attack();
            pHandAnims.PlayAttackAnim(Hand.Right);
        }
        pBody.JumpHandler(input.Player.Jump.IsPressed(), input.Player.Jump.WasPressedThisFrame());
        if(input.Player.Camera.IsInProgress() || input.Player.Movement.IsInProgress()){ pHandeling.WeaponSway(cameraInput.normalized, moveInput); }
        else{ pHandeling.IdleAroundOrigin(); }

        if(input.Player.SwitchToNextWeapon.WasPressedThisFrame()){
            pHandAnims.PickHandPosition(Hand.Left, pData.leftItem.GetComponent<ItemIDController>().id, false);
            pEquipment.SelectNextEquipment(Hand.Left);
            pHandAnims.PickHandPosition(Hand.Left, pData.leftItem.GetComponent<ItemIDController>().id, true);
            }

        if(input.Player.PauseMenu.IsPressed()){
            Application.Quit();
            Debug.Log("Quit button clicked.");
        }

        if(Input.GetKeyDown(KeyCode.K)){ pHealth.Damage(1); }
        if(Input.GetKeyDown(KeyCode.H)){ pHealth.Heal(1); }
    }

    void GetCameraInput(InputAction.CallbackContext obj)
    {
        controlType = obj.control.parent.displayName;
    }
}
