using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationFreeze : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        
        transform.rotation = Quaternion.Euler(
            rb.constraints == RigidbodyConstraints.FreezeRotationX ? 0.0f : transform.rotation.eulerAngles.x, 
            rb.constraints == RigidbodyConstraints.FreezeRotationY ? 0.0f: transform.rotation.eulerAngles.y, 
            0.0f
        );
    }
}
