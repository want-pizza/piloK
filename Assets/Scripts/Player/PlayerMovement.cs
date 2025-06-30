using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour, IMove
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float airAcceleration = 5f;
    [SerializeField] private float groundDeceleration = 20f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpTime = 0.35f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float jumpReleaseFactor = 0.5f;
    [SerializeField] private float wallJumpForce = 5f;

    [Header("Gravity")]
    [SerializeField] private float fallGravity = 15f;
    [SerializeField] private float maxFallSpeed = 20f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldownTime = 0.5f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float XScaling = 2f;
    [SerializeField] private float YScaling = 10f;

    [Header("Collision")]
    [SerializeField] private TriggerChecker groundChecker;
    [SerializeField] private TriggerChecker leftWallChecker;
    [SerializeField] private TriggerChecker rightWallChecker;
    [SerializeField] private TriggerChecker ceilingChecker;
    [SerializeField] private LayerMask groundLayer;


    private bool wasOnGroundAfterDash = true;
    private string dashEventName = "dashEndTimer";
    private string dashCooldownEventName = "dashTimer";
    private bool isDashCooldown = false;
    private bool isDashing = false;
    private bool wasDashInterrupted = false;
    private Vector2 dashDirection;

    private float _velocityX;
    private float _velocityY;
    public bool isGrounded;
    private bool isTouchingCeiling;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    private string coyoteEventName = "coyoteTimer";
    private bool isCanCoyoteJump;
    private string jumpBufferEventName = "bufferJump";
    public bool isJumpBufferTimming = false;
    private bool isJumpRelease;
    private bool isJumping;

    private PlayerAction _inputActions;
    private float _inputX = 0f;

    public float XVelocity { get => _velocityX; }
    public float YVelocity { get => _velocityY; }

    private void OnEnable()
    {
        _inputActions = InputManager.Instance.PlayerActions;
        _inputActions.Player.Move.performed += ctx => _inputX = ctx.ReadValue<Vector2>().x;
        _inputActions.Player.Move.canceled += ctx => _inputX = 0;
        _inputActions.Player.Jump.started += ctx => HandleJump();
        _inputActions.Player.Jump.canceled += ctx => ReleaseJump();
        _inputActions.Player.Dash.started += ctx => HandleDash();

        //GroundCheckers
        groundChecker.OnTriggeredStateChanged += OnSetIsGrounded;
        leftWallChecker.OnTriggeredStateChanged += triggered => isTouchingLeftWall = triggered;
        rightWallChecker.OnTriggeredStateChanged += triggered => isTouchingRightWall = triggered;
        ceilingChecker.OnTriggeredStateChanged += triggered => isTouchingCeiling = triggered;

        //EventBus
        TimerEventBus.Subscribe(coyoteEventName, DisableCoyoteTime);
        TimerEventBus.Subscribe(jumpBufferEventName, DisableJumpBufferTimming);
        TimerEventBus.Subscribe(dashCooldownEventName, DisableDashCooldown);
        TimerEventBus.Subscribe(dashEventName, DisableDashing);
    }

    private void Update()
    {
        if (InputManager.Instance.CurrentState != PlayerState.Normal)
            return;
        HandleMovement();
    }
    private void OnSetIsGrounded(bool triggered)
    {
        isGrounded = triggered;
        if (isGrounded)
        {
            isJumping = false;

            if (isJumpBufferTimming)
            {
                HandleJump();

                if(!isJumpRelease)
                    ReleaseJump();
            }
            if (!isDashing)
            {
                wasOnGroundAfterDash = true;
            }
        }
        else if (!isJumping)
            TimerManager.Instance.AddTimer(coyoteTime, coyoteEventName);
    }
    private void DisableCoyoteTime()
    {
        Debug.Log("CoyoteJump = false");
        isCanCoyoteJump = false;
    }
    private void DisableJumpBufferTimming()
    {
        Debug.Log("JumpBufferTimming = false");
        isJumpBufferTimming = false;
    }
    private void DisableDashCooldown()
    {
        isDashCooldown = false;
        if (isGrounded)
        {
            wasOnGroundAfterDash = true;
        }
    }
    private void DisableDashing()
    {
        //Debug.Log($"DisableDashing; wasDashInterrupted = {wasDashInterrupted}");
        if (!wasDashInterrupted)
        {
            isDashing = false;
            isDashCooldown = true;
            _velocityX /= XScaling;
            _velocityY /= YScaling;
            TimerManager.Instance.AddTimer(dashCooldownTime, dashCooldownEventName);
        }
    }
    public void ReleaseJump()
    {
        if (_velocityY > 0)
        {
            _velocityY *= jumpReleaseFactor;
            isJumpRelease = true;
        }
        else
        {
            isJumpRelease = false;
        }
    }
    private void HandleJump()
    {
        Debug.Log("HandleJump called");
        if (CanJump())
        {
            float jumpForce = Mathf.Sqrt(2 * Mathf.Abs(fallGravity) * jumpHeight);
            _velocityY = jumpForce;
            isJumping = true;
            isJumpBufferTimming = false;
            //Debug.Log($"Jump initiated: _velocityY = {_velocityY}");

            if (!isGrounded)
            {
                if (isTouchingLeftWall)
                {
                    _velocityX = wallJumpForce * Mathf.Sign(transform.localScale.x);
                }
                else if (isTouchingRightWall)
                {
                    _velocityX = wallJumpForce * -Mathf.Sign(transform.localScale.x);
                }
            }
        }
        else if (!isJumpBufferTimming && isJumping)
        {
            TimerManager.Instance.AddTimer(jumpBufferTime, jumpBufferEventName);
            isJumpBufferTimming = true;
        }
    }

    private bool CanJump()
    {
        return isGrounded || isCanCoyoteJump || isTouchingRightWall || isTouchingLeftWall;
    }

    private void HandleMovement()
    {
        if (isDashing)
        {
            HandleDashMovement();
            transform.position += new Vector3(_velocityX, _velocityY, 0) * Time.deltaTime;
            return;
        }

        float targetVelocityX = _inputX * moveSpeed;
        float accelerationRate = isGrounded ? acceleration : airAcceleration;
        float decelerationRate = isGrounded ? groundDeceleration : 0;

        if (Mathf.Abs(_inputX) > 0.1f) 
        {

            _velocityX = Mathf.MoveTowards(
                    _velocityX,
                    targetVelocityX,
                    accelerationRate * Time.deltaTime
                );
        }
        else
        {
            _velocityX = Mathf.MoveTowards(
                _velocityX,
                0,
                decelerationRate * Time.deltaTime
            );
        }

        if (IsHittingWall(_velocityX))
        {
            _velocityX = 0;
        }

        if (!isGrounded)
        {
            _velocityY = Mathf.MoveTowards(
                _velocityY,
                -maxFallSpeed,
                fallGravity * Time.deltaTime
            );
        }
        else if (_velocityY < 0)
        {
            _velocityY = 0;
        }

        if (isTouchingCeiling && _velocityY > 0)
        {
            _velocityY = 0;
        }

        transform.position += new Vector3(_velocityX, _velocityY, 0) * Time.deltaTime * 2;
    }

    private void HandleDashMovement()
    {
        Vector2 moveDir = dashDirection.normalized;
        _velocityX = moveDir.x * dashSpeed;
        _velocityY = moveDir.y * dashSpeed;

        if (IsHittingWall(moveDir.x))
        {
            if (isGrounded)
            {
                wasOnGroundAfterDash = true;
            }
            wasDashInterrupted = true;
            isDashing = false;
            _velocityX /= 2; //need rewrite
            _velocityY /= 5;
            TimerManager.Instance.AddTimer(dashCooldownTime, dashCooldownEventName);
            return;
        }
    }


    private void HandleDash()
    {
        //Debug.Log("Try dashing");
        if (CanDash())
        {
           // Debug.Log("Start dashing");
            isDashing = true;
            wasDashInterrupted = false;
            wasOnGroundAfterDash = false;

            Vector2 input = _inputActions.Player.Move.ReadValue<Vector2>();
            Debug.Log($"dashInput - {input.x}; {input.y}");
            if (input == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0).normalized;
            }
            else
            {
                dashDirection = input.normalized;
            }
            TimerManager.Instance.AddTimer(dashTime, dashEventName);
        }
    }

    private bool CanDash()
    {
        return !isDashing && !isDashCooldown && wasOnGroundAfterDash;
    }

    private bool IsHittingWall(float dir)
    {
        if (dir > 0 && isTouchingRightWall)
            return true;
        if (dir < 0 && isTouchingLeftWall)
            return true;

        return false;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= ctx => _inputX = ctx.ReadValue<Vector2>().x;
        _inputActions.Player.Move.canceled -= ctx => _inputX = 0;
        _inputActions.Player.Jump.performed -= ctx => HandleJump();
        _inputActions.Player.Jump.canceled -= ctx => _velocityY *= _velocityY > 0 ? 0.5f : 1f;
        _inputActions.Player.Disable();

        groundChecker.OnTriggeredStateChanged -= OnSetIsGrounded;
        leftWallChecker.OnTriggeredStateChanged -= triggered => isTouchingLeftWall = triggered;
        rightWallChecker.OnTriggeredStateChanged -= triggered => isTouchingRightWall = triggered;

        TimerEventBus.Unsubscribe(coyoteEventName, DisableCoyoteTime);
        TimerEventBus.Unsubscribe(jumpBufferEventName, DisableJumpBufferTimming);
        TimerEventBus.Unsubscribe(dashCooldownEventName, HandleDash);
        TimerEventBus.Unsubscribe(dashEventName, DisableDashing);
    }
}