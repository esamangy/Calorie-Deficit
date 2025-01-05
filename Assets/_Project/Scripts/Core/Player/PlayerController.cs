using System;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineVirtualCamera virtualVCam;
    [SerializeField] private InputReader input;
    [SerializeField] private Transform cameraRoot;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float smoothTime = .2f;

    private const float ZeroF = 0f;
    private const float TopClamp = 90.0f;
    private const float BottomClamp = -90.0f;
    private Transform mainCam;

    private float currentSpeed;
    private float velocity;

    private float _cinemachineTargetPitch;
    private float _rotationVelocity;
    
    private void Awake() {
        mainCam = Camera.main.transform;

        virtualVCam.Follow = cameraRoot;
        virtualVCam.LookAt = cameraRoot;
        virtualVCam.OnTargetObjectWarped(transform, transform.position - virtualVCam.transform.position - Vector3.forward);
    }

    private void OnEnable() {
        input.Look += OnLook;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnLook(Vector2 arg0, bool arg1) {
        //Don't multiply mouse input by Time.deltaTime
		float deltaTimeMultiplier = false ? 1.0f : Time.deltaTime;

        _cinemachineTargetPitch += arg0.y * rotationSpeed * deltaTimeMultiplier;
        _rotationVelocity = arg0.x * rotationSpeed * deltaTimeMultiplier;

        // clamp our pitch rotation
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Update Cinemachine camera target pitch
        cameraRoot.transform.localRotation = Quaternion.Euler(-_cinemachineTargetPitch, 0.0f, 0.0f);

        // rotate the player left and right
        transform.Rotate(Vector3.up * _rotationVelocity);
    }

    private void Update() {
        HandleMovement();
        // HandleAnimator();
    }

    private void HandleMovement() {
        Vector3 movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
        Vector3 adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;

        if(adjustedDirection.magnitude > ZeroF) {
            //HandleRotation(adjustedDirection);
            HandleCharacterController(adjustedDirection);

            SmoothSpeed(adjustedDirection.magnitude);
        } else {
            SmoothSpeed(ZeroF);
        }
    }
    private void HandleCharacterController(Vector3 adjustedDirection) {
        //move the player
        Vector3 adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
        controller.Move(adjustedMovement);
    }
    private void HandleRotation(Vector3 adjustedDirection) {
        //adjust rotation to match movement direction
        Quaternion targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SmoothSpeed(float value) {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax) {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
