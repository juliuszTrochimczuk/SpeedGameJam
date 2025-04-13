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
    [SerializeField] private int sensitivity;
    [SerializeField] private int backRotationSpeed;
    private float _rotationX;
    private bool _rotating;
    
    
    private void Awake() {
        rotationX.action.performed += OnRotate;
    }
    
    private void OnRotate(InputAction.CallbackContext context) {
        _rotationX = context.ReadValue<float>() * sensitivity;
    }
    

    void Update() {
        target.Rotate(Vector3.up, _rotationX * Time.deltaTime);
        target.rotation = Quaternion.Euler(target.rotation.eulerAngles.x, target.rotation.eulerAngles.y, 0);
        
        
        
        
    }
}

