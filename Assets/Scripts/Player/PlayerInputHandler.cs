using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHanlder : MonoBehaviour
{
    [SerializeField] MainMenu pauseMenu;
    [SerializeField] PlayerMovement pBody;
    [SerializeField] PlayerCameraControl pCamera;
    [SerializeField] PlayerWeaponHandeling pHandeling;
    [SerializeField] HealthController pHealth;
    [SerializeField] EquipmentHandler pLeftEquipment;
    [SerializeField] EquipmentHandler pRightEquipment;

    public event System.Action<int> onWeaponSwap;

    PlayerInput input;
    Vector2 moveInput, cameraInput;

    string controlType;
    bool isPaused;

    void Awake()
    {
        input = new PlayerInput();
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Camera.performed += GetCameraInput;
        input.Player.Camera.canceled += GetCameraInput;
        input.Player.PauseMenu.performed += PauseGame;
    }

    void Start()
    {
        isPaused = false;
    }

    void OnDisable()
    {
        input.Player.Camera.performed -= GetCameraInput;
        input.Player.Camera.canceled -= GetCameraInput;
        input.Player.PauseMenu.performed -= PauseGame;
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaused){

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
                pLeftEquipment.currentEquipment.GetComponent<AttackController>().Attack();
            }
            if(input.Player.RightAttack.IsInProgress()){
                pRightEquipment.currentEquipment.GetComponent<AttackController>().Attack();
            }
            pBody.JumpHandler(input.Player.Jump.IsPressed(), input.Player.Jump.WasPressedThisFrame());
            if(input.Player.Camera.IsInProgress() || input.Player.Movement.IsInProgress()){ pHandeling.WeaponSway(cameraInput.normalized, moveInput); }
            else{ pHandeling.IdleAroundOrigin(); }

            if(input.Player.SwitchToNextWeapon.WasPressedThisFrame()){
                pLeftEquipment.SelectNextEquipment();
                onWeaponSwap?.Invoke(0);
            }

            if(Input.GetKeyDown(KeyCode.K)){ pHealth.Damage(1, gameObject); }
            if(Input.GetKeyDown(KeyCode.H)){ pHealth.Heal(1); }

        }
    }

    void GetCameraInput(InputAction.CallbackContext obj)
    {
        controlType = obj.control.parent.displayName;
    }

    void PauseGame(InputAction.CallbackContext obj)
    {
        if(obj.action.WasPerformedThisFrame()){
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeInHierarchy);
            isPaused = pauseMenu.gameObject.activeInHierarchy;
            //Debug.Log("Pause button clicked.");
        }
    }
}
