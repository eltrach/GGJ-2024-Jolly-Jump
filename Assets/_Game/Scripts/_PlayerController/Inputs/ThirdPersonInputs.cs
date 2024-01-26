using Sirenix.OdinInspector;
using TouchControlsKit;
using UnityEngine;



public class ThirdPersonInputs : MonoBehaviour
{
    [SerializeField] private bool debug;
    [Header(" Is Mobile : ")]
    [SerializeField] private bool _isMobile = true;
    [Space(10)]
    public bool HoldToAim;
    //movements
    [Space(10)]
    [SerializeField, ShowIf("debug")] private float _horizontal;
    [SerializeField, ShowIf("debug")] private float _vertical;
    //camera
    [SerializeField, ShowIf("debug")] private float _MouseX;
    [SerializeField, ShowIf("debug")] private float _MouseY;

    public KeyCode actionBtnKeyCode = KeyCode.E;

    //buttons
    [ShowInInspector] public float InputMag => _inputDirection.magnitude;
    [Header("Debug"), ShowIf("debug")]
    [SerializeField, ShowIf("debug")] private bool _isJumping;
    [SerializeField, ShowIf("debug")] private bool _isSprinting;
    [SerializeField, ShowIf("debug")] private bool _shotButton;
    [SerializeField, ShowIf("debug")] private bool _aimButton;
    [SerializeField, ShowIf("debug")] private bool _actionButton;
    [SerializeField, ShowIf("debug")] private Vector2 _inputDirection;
    [SerializeField, ShowIf("debug")] private Vector2 _cameraDirection;

    // 
    public ThirdPersonShooter _shooter;

    // public
    public Vector2 Move { get => _inputDirection; set => _inputDirection = value; }
    public Vector2 Look { get => _cameraDirection; set => _cameraDirection = value; }
    public bool Sprint { get => _isSprinting; set => _isSprinting = value; }
    public bool Jump { get => _isJumping; set => _isJumping = value; }
    public bool Aim { get => _aimButton; set => _aimButton = value; }
    public bool Shot { get => _shotButton; set => _shotButton = value; }
    public bool ActionButton { get => _actionButton; set => _actionButton = value; }

    private void Start()
    {
        if (!_isMobile) Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (_isMobile) Mobile();
        else MouseKeyboard();
    }
    private void Mobile()
    {
        Move = TCKInput.GetAxis("Joystick");
        Look = TCKInput.GetAxis("Touchpad");
        HandleAimConditions();
        HandleShoot();
        _isJumping = TCKInput.GetAction("jumpBtn", EActionEvent.Down);
        _actionButton = TCKInput.GetAction("actionBtn", EActionEvent.Down);
    }
    private void MouseKeyboard()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _MouseX = Input.GetAxis("Mouse X");
        _MouseY = Input.GetAxis("Mouse Y");

        HandleAimConditions();
        HandleShoot();

        Move = new Vector2(_horizontal, _vertical);
        Look = new Vector2(_MouseX, _MouseY);

        _isJumping = Input.GetButtonDown("Jump");
        _isSprinting = Input.GetKey(KeyCode.LeftShift);
        _actionButton = Input.GetKeyDown(actionBtnKeyCode);
    }
    private void HandleAimConditions()
    {
        if (_isMobile)
        {
            if (HoldToAim) _aimButton = TCKInput.GetAction("aimBtn", EActionEvent.Press);
            else _aimButton = TCKInput.GetAction("aimBtn", EActionEvent.Down);
        }
        else
        {
            if (HoldToAim) _aimButton = Input.GetMouseButton(1);
            else _aimButton = Input.GetMouseButtonDown(1);
        }
    }
    public void HandleShoot()
    {
        if (_shooter.GetEquippedWeapon() != null)
        {
            Weapon EquippedWeapon = _shooter.GetEquippedWeapon();
            if (EquippedWeapon.automaticWeapon)
            {
                if (_isMobile) _shotButton = TCKInput.GetAction("fireBtn", EActionEvent.Press);
                else _shotButton = Input.GetMouseButton(0);
            }
            else
            {
                if (_isMobile) _shotButton = TCKInput.GetAction("fireBtn", EActionEvent.Down);
                else _shotButton = Input.GetMouseButtonDown(0);
            }
        }
        else // if he's not holding any weapon:
        {
            if (_isMobile) _shotButton = TCKInput.GetAction("fireBtn", EActionEvent.Press);
            else _shotButton = Input.GetMouseButton(0);
        }
    }
}
