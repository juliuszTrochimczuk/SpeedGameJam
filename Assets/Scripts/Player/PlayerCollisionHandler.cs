using Cinemachine;
using ObjectsOnMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerSpinning spinning;
        private PlayerStatesHandler statesHandler;
        private CinemachineVirtualCamera virtualCamera;

            
        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            spinning = GetComponent<PlayerSpinning>();
            statesHandler = GetComponent<PlayerStatesHandler>();
            virtualCamera = transform.root.GetComponentInChildren<CinemachineVirtualCamera>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Static_Obstacles")
            {
                Audio.Instance.PlaySound("Bump");
                if (statesHandler.CurrentState == PlayerStatesHandler.PlayerState.Spinning)
                {
                    Vector3 spinDirection = Vector3.Reflect(movement.transform.forward, collision.contacts[0].normal);
                    spinning.ChangeDirection(spinDirection);
                    StartCoroutine(CameraDelay());
                    transform.LookAt(transform.position + spinDirection);
                }
            }
            else if (collision.collider.tag == "Dynamic_Obstacles")
            {
                Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
                rb.AddForce(movement.Speed * ((collision.collider.transform.position - movement.transform.position).normalized + Vector3.up), ForceMode.VelocityChange);
                Audio.Instance.PlaySound("Bump");
            }
            else if (collision.collider.tag == "Bouncing_Obstacles")
            {
                Audio.Instance.PlaySound("Bump");
                if (statesHandler.CurrentState == PlayerStatesHandler.PlayerState.Spinning)
                {
                    Vector3 spinDirection = Vector3.Reflect(movement.transform.forward, collision.contacts[0].normal);
                    spinning.ChangeDirection(spinDirection);
                    Bouncer bouncer = collision.collider.gameObject.GetComponent<Bouncer>();
                    spinning.ChangeStrength(bouncer.spinStrength, bouncer.spinDuration);
                    StartCoroutine(CameraDelay());
                    transform.LookAt(transform.position + spinDirection);
                }
            }
            else if (collision.collider.tag == "Ground")
            {
                Audio.Instance.PlaySound("Ground");
            }
        }

        private IEnumerator CameraDelay()
        {
            virtualCamera.Follow = null;
            yield return new WaitForSeconds(0.5f);
            virtualCamera.Follow = gameObject.transform;
        }
    }
}