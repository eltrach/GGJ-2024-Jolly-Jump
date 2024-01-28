using System;
using UnityEngine;

namespace VTemplate.Controller
{
    public class ThirdPersonController : IHuman
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        [SerializeField] private float moveSpeed = 2.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        [SerializeField] private float sprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float rotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float speedChangeRate = 10.0f;
        // Camera
        [SerializeField] private float CameraSensitivity = 1f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        [SerializeField] private float jumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        [SerializeField] private float gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private float jumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField] private float fallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField] private bool grounded = true;

        [Tooltip("Useful for rough ground")]
        [SerializeField] private float groundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField] private float groundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        [SerializeField] private LayerMask groundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        [SerializeField] private GameObject cinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        [SerializeField] private float topClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        [SerializeField] private float bottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        [SerializeField] private float cameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        [SerializeField] private bool lockCameraPosition = false;


        public bool canMove = true;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private bool _rotateOnMove = true;
        private float targetSpeed;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDLocomotion;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDIsDead;

        private Animator _animator;
        private CharacterController _controller;
        private ThirdPersonInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;
        private bool _hasAnimator;

        public float TargetSpeed { get => targetSpeed; set => targetSpeed = value; }
        public float runSpeed { get => moveSpeed; set => moveSpeed = value; }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }
        private void Start()
        {
            _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<ThirdPersonInputs>();
            AssignAnimationIDs();
            // reset our timeouts on start
            _jumpTimeoutDelta = jumpTimeout;
            _fallTimeoutDelta = fallTimeout;
        }
        private void Update()
        {

            JumpAndGravity();
            GroundedCheck();
            Move();

        }
        private void LateUpdate()
        {
            CameraRotation();
        }
        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            bool sprint = _input.Move.magnitude > 0.7f;
            //targetSpeed = sprint ? sprintSpeed : walkSpeed;
            targetSpeed = _input.Sprint ? sprintSpeed : runSpeed;

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.Move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;
            inputDirection *= 1.5f;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.Move != Vector2.zero)
            {
                _targetRotation = (Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg) +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    rotationSmoothTime);

                // rotate to face input direction relative to camera position
                if (_rotateOnMove) transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move((targetDirection.normalized * (_speed * Time.deltaTime)) + (new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime));

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDLocomotion, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, 1);
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDLocomotion = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDIsDead = Animator.StringToHash("isDead");
        }
        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new(transform.position.x, transform.position.y - groundedOffset,
                transform.position.z);
            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, grounded);
            }
        }
        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.Look.sqrMagnitude >= _threshold && !lockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                _cinemachineTargetYaw += _input.Look.x * CameraSensitivity;
                _cinemachineTargetPitch -= _input.Look.y * CameraSensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        private void JumpAndGravity()
        {
            if (grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = fallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                        AudioManager.Play("Jump");
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = jumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.Jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }
        }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        public void SetSensitivity(float newSensitivity)
        {
            CameraSensitivity = newSensitivity;
        }
        public void SetRotateOnMove(bool newRotateOnMove)
        {
            _rotateOnMove = newRotateOnMove;
        }

        public void Die()
        {
            _animator.GetLayerIndex("Base Layer");
            _animator.SetBool(_animIDIsDead, true);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            float weight = animationEvent.animatorClipInfo.weight;
            if (weight > 0.25f)
            {
                if (AudioManager.instance)
                    AudioManager.instance.PlayFoot(_animationBlend);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

            if (grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z),
                groundedRadius);
        }
    }
}