using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovement pBody;
    [SerializeField] PlayerAttack pAttack;
    [SerializeField] PlayerCameraControl pCamera;
    [SerializeField] PlayerWeaponHandeling pHandeling;
    [SerializeField] HealthController pHealth;
    [SerializeField] EquipmentHandler pEquipment;

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
        if(input.Player.Attack.IsInProgress()){ pEquipment.currentEquipment.GetComponent<AttackController>().Attack(); }
        pBody.JumpHandler(input.Player.Jump.IsPressed(), input.Player.Jump.WasPressedThisFrame());
        if(input.Player.Camera.IsInProgress() || input.Player.Movement.IsInProgress()){ pHandeling.WeaponSway(input.Player.Camera.ReadValue<Vector2>().normalized, input.Player.Movement.ReadValue<Vector2>()); }
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
        pBody.MovePlayer(input.Player.Movement.ReadValue<Vector2>());
    }
}
