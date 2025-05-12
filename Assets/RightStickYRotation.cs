using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RightStickControlledRotation : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float maxYawAngle = 45f;
    public float resetSpeed = 5f;

    private float currentYaw = 0f;
    private Vector2 rightStickInput = Vector2.zero;

    public void OnLook(InputAction.CallbackContext context)
    {
        rightStickInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        float horizontalInput = rightStickInput.x;

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            currentYaw += horizontalInput * rotationSpeed * Time.deltaTime;
            currentYaw = Mathf.Clamp(currentYaw, -maxYawAngle, maxYawAngle);
        }
        else
        {
            currentYaw = Mathf.Lerp(currentYaw, 0f, resetSpeed * Time.deltaTime);
        }

        transform.localRotation = Quaternion.Euler(0f, currentYaw, 0f);
    }
}
