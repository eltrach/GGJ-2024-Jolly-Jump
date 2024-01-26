using System.Collections;
using TMPro;
using UnityEngine;
using VTemplate.Input;
public class FirstPersonController : MonoBehaviour
{
    public PlayerSettings settings = null;
    [SerializeField] private bool canDebug = true;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float speedChangeRate = 1.0f; // Time in seconds for the transition
    private float transitionTimer = 0.0f;

    // private Unserialized
    private CharacterController _cc;
    private ThirdPersonInputs _input;
    //vars
    private bool _canMove = true;
    private Vector3 _moveDirection;
    private float _targetSpeed;
    private float _walkSpeed;
    private float _runSpeed;
    private float _jumpHeight;
    private float _gravity;
    private float _speedMultiplier;
    private float _speed;

    Vector3 _velocity;
    bool _isGrounded;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _curSpeed_Text;
    void Start()
    {
        Init();
    }
    private void Init()
    {
        _targetSpeed = _walkSpeed;
        _cc = GetComponent<CharacterController>();
        _input = GetComponent<ThirdPersonInputs>();
        // get vars
        if (!settings) Debug.LogError("Please Assign the Player Settings");
        _walkSpeed = settings.WalkSpeed;
        _runSpeed = settings.RunSpeed;
        _jumpHeight = settings.JumpForce;
        _gravity = settings.Gravity;
        _speedMultiplier = settings.SpeedMultiplier;
    }
    void Update()
    {
        IsGrounded();
        if (_canMove)
        {
            Move();
            JumpAndGravity();
        }
    }
    private void Move()
    {
        _targetSpeed = _input.Sprint ? _runSpeed : _walkSpeed;

        _moveDirection = (transform.right * _input.Move.x) + (transform.forward * _input.Move.y);

        if (canDebug) _curSpeed_Text.text = " Current Speed : " + _targetSpeed.ToString();

        _cc.Move(_moveDirection.normalized * _targetSpeed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;

        _cc.Move(_velocity * Time.deltaTime);
    }
    private void JumpAndGravity()
    {

        bool jump = _input.Jump;
        if (IsGrounded() && _velocity.y < 0.0f)
        {
            _velocity.y = -2f;
        }
        if (jump && IsGrounded())
        {
            if (canDebug) Debug.Log(" JumpAndGravity()");
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
    private bool IsGrounded()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        return _isGrounded;
    }
    IEnumerator TransitionSpeed(float startValue, float endValue)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < speedChangeRate)
        {
            _targetSpeed += (endValue - startValue) * (Time.deltaTime / speedChangeRate);
            Debug.Log(_targetSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _targetSpeed = endValue;
    }
    private void OnDrawGizmos()
    {
        if (groundCheck) Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
    public void Die()
    {
        Debug.Log(" Player.Die() ");
    }
}
