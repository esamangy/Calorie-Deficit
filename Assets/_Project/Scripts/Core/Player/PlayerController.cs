using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using static Timer;

public class PlayerController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundChecker groundChecker;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform virtualVCam;
    [SerializeField] private InputReader input;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private CapsuleCollider thisCollider;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float sprintSpeed = 600f;
    [SerializeField] private float cameraSpeed = 15f;
    [SerializeField] private float smoothTime = .2f;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpDuration = .5f;
    [SerializeField] float jumpCooldown = .2f;
    [SerializeField] float gravityMultiplier = 1f;

    public static PlayerController Instance;
    private const float ZeroF = 0f;
    private Transform mainCam;

    private float currentSpeed;
    private float velocity;
    float jumpVelocity;

    private Vector3 movement;
    List<Timer> timers;
    CountdownTimer jumpTimer;
    CountdownTimer jumpCooldownTimer;
    private StateMachine stateMachine;
    private bool frozen = false;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
        mainCam = Camera.main.transform;

        virtualVCam.GetComponent<CinemachineCamera>().Follow = cameraRoot;

        rb.freezeRotation = true;

        //Setup timers
        jumpTimer = new CountdownTimer(jumpDuration);
        jumpCooldownTimer = new CountdownTimer(jumpCooldown);
        timers = new List<Timer>(2) { jumpTimer, jumpCooldownTimer};

        jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

        // State Machine
        stateMachine = new StateMachine();

        //Declare States
        var locomationState = new LocomotionState(this);
        var jumpState = new JumpState(this);

        //define transitions;
        At(locomationState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
        At(jumpState, locomationState, new FuncPredicate(() => groundChecker.IsGrounded && !jumpTimer.IsRunning));

        stateMachine.SetState(locomationState);
    }

    void At(IState from, IState to, IPredicate condition) {
        stateMachine.AddTransition(from, to, condition);
    }
    void Any(IState to, IPredicate condition) {
        stateMachine.AddAnyTransition(to, condition);
    }

    private void OnEnable() {
        input.Jump += OnJump;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnablePlayer() {
        frozen = false;
    }

    public void DisablePlayer() {
        frozen = true;
    }

    private void OnDisable() {
        input.Jump -= OnJump;

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnJump(bool performed) {
        if(frozen) return;
        if(performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded) {
            jumpTimer.Start();
            GameManager.Instance.RegisterJump();
        } else if (!performed && jumpTimer.IsRunning){
            jumpTimer.Stop();
        }
    }

    private void Update() {
        if(frozen) return;
        movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
        cameraRoot.rotation = virtualVCam.rotation;

        stateMachine.Update();
        HandleTimers();
        // HandleAnimator();
    }

    private void HandleTimers() {
        foreach (Timer timer in timers) {
            timer.Tick(Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        stateMachine.FixedUpdate();
    }

    public void HandleJump() {
        if(frozen) return;
        //if not jumping and grounded, keep jump velocity at 0
        if(!jumpTimer.IsRunning && groundChecker.IsGrounded) {
            jumpVelocity = ZeroF;
            jumpTimer.Stop();
            return;
        }

        if(!jumpTimer.IsRunning) {
            //Gravity takes over
            jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }

        //apply vlocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
    }

    public void HandleMovement() {
        if(frozen) return;
        Vector3 adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;

        if(adjustedDirection.magnitude > ZeroF) {
            //HandleRotation(adjustedDirection);
            HandleHorizontalMovement(adjustedDirection);

            SmoothSpeed(adjustedDirection.magnitude);
        } else {
            SmoothSpeed(ZeroF);

            //Reset horizonl velocity for a snappy stop
            rb.linearVelocity = new Vector3(ZeroF, rb.linearVelocity.y, ZeroF);
        }
    }
    private void HandleHorizontalMovement(Vector3 adjustedDirection) {
        //move the player
        float speed = input.IsSprinting ? sprintSpeed : moveSpeed;
        Vector3 velocity = adjustedDirection * speed * Time.fixedDeltaTime;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }
    private void HandleRotation(Vector3 adjustedDirection) {
        //adjust rotation to match movement direction
        Quaternion targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, cameraSpeed * Time.deltaTime);
    }

    private void SmoothSpeed(float value) {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax) {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SetMoveSpeed(float value) {
        moveSpeed = value;
    }

    public void SetSprintSpeed(float value) {
        sprintSpeed = value;
    }

    public float GetRadius() {
        return thisCollider.radius;
    }
}
