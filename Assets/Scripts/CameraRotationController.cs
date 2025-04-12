using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotationController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private InputActionReference rotationX;
    [SerializeField] private InputActionReference rotationY;
    private float _rotationX, _rotationY;
    
    
    private void Awake() {
        rotationX.action.performed += OnRotate;
    }
    
    private void OnRotate(InputAction.CallbackContext context) {
        _rotationX = context.ReadValue<float>();
    }

    void Update() {
        target.Rotate(Vector3.up, _rotationX * Time.deltaTime);
    }
}

