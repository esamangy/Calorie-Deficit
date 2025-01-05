using System;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private InputReader input;
    [SerializeField] private CinemachineVirtualCamera virtualVCam;
    
    [Header("Settings")]
    [SerializeField] private float speedMultiplier;


    private void OnEnable() {
        input.Look += OnLook;
    }

    private void OnLook(Vector2 arg0, bool arg1) {
        throw new NotImplementedException();
    }
}