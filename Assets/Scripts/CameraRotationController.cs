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
    [SerializeField] private InputActionReference rotationX;
    [SerializeField] private int sensitivity;
    private float _rotationX;
    
    public void OnRotate(InputAction.CallbackContext context) {
        _rotationX = context.ReadValue<float>() * sensitivity;
    }
    

    void Update() {
        target.Rotate(Vector3.up, _rotationX * Time.deltaTime);
    }
}

