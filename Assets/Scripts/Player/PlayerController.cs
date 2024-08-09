using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // audio source
    [SerializeField] private AudioSource runningSoundEffect;
    [SerializeField] private AudioSource punchSoundEffect;

    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();

    [Header("Player")]
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;
    [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
    public float SpeedChangeRate = 10.0f;
    public float Sensitivity = 1.0f;

    [Space(10)]
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;
    
    [Space(10)]
    public float JumpTimeout = 0.50f;
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;

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
    
    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private Animator _animator;
    private CharacterController _controller;
    private PlayerInputs _input;
    private InputAction move;
    private GameObject _mainCamera;
    private PlayerAttack attack;

    private Vector3 mouseWorldPosition = Vector3.zero;

    private const float _threshold = 0.01f;

    private bool IsCurrentDeviceMouse = true;
    private bool IsStanceFire = true;
    private bool IsAttacking = false;
    private bool MoveDisabled = false;

    [SerializeField] private GameObject eyebrows;
    [SerializeField] private GameObject eyelashes;
    [SerializeField] private GameObject hair;
    [SerializeField] private GameObject beard;
    [SerializeField] private GameObject moustache;
    [SerializeField] private GameObject weapon;
    
    private Renderer eyebrowsRenderer;
    private Renderer eyelashesRenderer;
    private Renderer hairRenderer;
    private Renderer beardRenderer;
    private Renderer moustacheRenderer;
    private Renderer weaponRenderer;
    private List<Renderer> renderList;

    public float dashSpeed;

    private void Awake()
    {
        // Gets reference to the Main Camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start() 
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputs>();
        _animator = GetComponent<Animator>();
        attack = GetComponentInChildren<PlayerAttack>();

        eyebrowsRenderer = eyebrows.GetComponent<Renderer>();
        eyelashesRenderer = eyelashes.GetComponent<Renderer>();
        hairRenderer = hair.GetComponent<Renderer>();
        beardRenderer = beard.GetComponent<Renderer>();
        moustacheRenderer = moustache.GetComponent<Renderer>();
        weaponRenderer = weapon.GetComponent<Renderer>();

        renderList = new List<Renderer> {eyebrowsRenderer, eyelashesRenderer, hairRenderer, beardRenderer, moustacheRenderer, weaponRenderer};

        foreach (Renderer r in renderList)
        {
            r.material.color = Color.red;
        }
        
        // Reset timeouts on Start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
        Swap();
        Attack();
        Dash();
        AnimCheck();

    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void RaycastUpdate()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999.0f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        } 
    }

    private void GroundedCheck()
    {
        // Sets sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
        
        if (Grounded)
        {
            _animator.SetBool("isGrounded", true);
        }
        else
        {
            _animator.SetBool("isGrounded", false);
        }
    }

    private void CameraRotation()
    {
        // If there is an input
        if (_input.look.sqrMagnitude >= _threshold)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * Sensitivity;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * Sensitivity;
        }

        // Clamps rotations so values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {
        float targetSpeed = MoveSpeed;

        if (_input.move == Vector2.zero || MoveDisabled) targetSpeed = 0.0f;

        // A reference to the player's current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        _animator.SetFloat("speed", currentHorizontalSpeed);
        _animator.SetFloat("movex", _input.move.x);
        _animator.SetFloat("movey", _input.move.y);

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // Accelerates or decelerates to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // Creates curved result rather than a linear one, giving a more organic speed change
                
            // Note: T in Lerp is clamped, so speed does not need to be clamped

            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // Rounds speed to three decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;

        }
        else
        {
            _speed = targetSpeed;
        }

        // Normalizes input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        // Note: Vector2's != operator uses approximation so it is not floating point error prone, and it is cheaper than magnitude
            
        // If there is a move input, rotate the player when the player is moving
        if (_input.move != Vector2.zero)
        {
            runningSoundEffect.mute = false;

            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // Rotates player to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            runningSoundEffect.mute = true;
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // Moves the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;

            if(_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                _animator.SetTrigger("jumpTrigger");
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = JumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }

            _input.jump = false;
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }

        _input.jump = false;
    }

    private void Swap()
    {
        if (_input.swap && IsStanceFire)
        {
            foreach (Renderer r in renderList)
            {
                r.material.color = Color.blue;
            }
            IsStanceFire = false;
        }
        else if (_input.swap)
        {
            foreach (Renderer r in renderList)
            {
                r.material.color = Color.red;
            }
            IsStanceFire = true;
        }
        _input.swap = false;
    }

    private void Attack()
    {
        attack.setAttackType(IsStanceFire);

        if (_input.lightAttack && _input.move == Vector2.zero) 
        {
            punchSoundEffect.Play();
            _animator.SetTrigger("lightAttackTrigger");
            _input.lightAttack = false;
            attack.lightAttack();
        }
        if (Input.GetButtonDown("Heavy Attack") && !IsAttacking) 
        {
            _animator.Play("Heavy Attack 1");
            _input.heavyAttack = false;
            attack.heavyAttack();
        }
    }

    private void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // Moves the player
        _controller.Move(targetDirection.normalized * (dashSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }

    private void AnimCheck()
    {
        var animState = _animator.GetCurrentAnimatorStateInfo(0);
        if (animState.IsName("Heavy Attack 1") || animState.IsName("Charging") || animState.IsName("Heavy Attack 2"))
        {
            _input.lightAttack = false;
            IsAttacking = true;
            MoveDisabled = true;
        }
        else
        {
            IsAttacking = false;
            MoveDisabled = false;
        }

        if (animState.IsName("Charging") && !Input.GetButton("Heavy Attack"))
        {
            _animator.SetBool("anim_Charging", false);
        }
    }

    public void ChargeAnimation()
    {
        if (Input.GetButton("Heavy Attack"))
        {
            _animator.SetBool("anim_Charging", true);
        }
    }
}

