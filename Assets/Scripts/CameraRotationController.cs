using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotationController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private InputActionReference rotationX;
    [SerializeField] private InputActionReference rotationSwitch;
    [SerializeField] private int sensitivity;
    [SerializeField] private int backRotationSpeed;
    private float _rotationX;
    private bool _rotating;
    
    
    private void Awake() {
        rotationX.action.performed += OnRotate;
        rotationSwitch.action.performed += OnEnableRotation;
        rotationSwitch.action.canceled += OnDisableRotation;
    }
    
    private void OnRotate(InputAction.CallbackContext context) {
        if (_rotating) {
            _rotationX = context.ReadValue<float>() * sensitivity;
        }
    }

    private void OnEnableRotation(InputAction.CallbackContext context)
    {
        _rotating = true;
    }

    private void OnDisableRotation(InputAction.CallbackContext context)
    {
        _rotating = false;
    }

    void Update() {
        if (_rotating) {
            target.Rotate(Vector3.up, _rotationX * Time.deltaTime);
        }else{
            target.Rotate(Vector3.up, (playerTransform.forward.x-target.forward.x)*backRotationSpeed * Time.deltaTime);
        }
        
    }
}

