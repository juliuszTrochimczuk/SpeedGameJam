using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerSpinning : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerStatesHandler statesHandler;

        private Vector3 direction;
        private Coroutine slowingDownCoroutine;
        private Coroutine speedingUpCoroutine;

        private float strength;
        public float Strength => strength;

        [SerializeField] private float slowingDownTime;
        [SerializeField] private float baseStrength = 1;
        

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            statesHandler = GetComponent<PlayerStatesHandler>();
        }

        private void FixedUpdate()
        {
            if (statesHandler.CurrentState != PlayerStatesHandler.PlayerState.Spinning)
                return;

            if (!Physics.Raycast(transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground")))
                return;

            if (speedingUpCoroutine == null)
            {
                Debug.Log(speedingUpCoroutine);
            }
            


            if (speedingUpCoroutine == null && slowingDownCoroutine == null)
                slowingDownCoroutine = StartCoroutine(LerpingStrength(baseStrength, slowingDownTime, slowingDownCoroutine));
            else if (speedingUpCoroutine == null && slowingDownTime != 0)
                StopCoroutine(slowingDownCoroutine);

                transform.Translate(movement.Speed * Time.fixedDeltaTime * direction * strength, Space.World);
        }
         
        public void Spinning(InputAction.CallbackContext context)
        {
            if (statesHandler.CurrentState == PlayerStatesHandler.PlayerState.Not_In_Move)
                return;

            context.action.performed += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 1);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Spinning;
                direction = transform.forward;
            };
            context.action.canceled += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 3);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Moving;
            };
        }

        public void ChangeDirection(Vector3 direction) => this.direction = direction;

        public void ChangeStrength(float addition, float duration)
        {
            if (speedingUpCoroutine != null)
                StopCoroutine(speedingUpCoroutine);
            speedingUpCoroutine = StartCoroutine(LerpingStrength(strength + addition, duration, speedingUpCoroutine));
        }

        private IEnumerator LerpingStrength(float targetStrength, float duration, Coroutine coroutineToNull)
        {
            float time = 0.0f;
            float oldStrength = strength;
            Debug.Log("target strenght "+targetStrength);
            while (time < duration)
            {
                strength = Mathf.Lerp(oldStrength, targetStrength, time / duration);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            strength = targetStrength;
            coroutineToNull = null;
        }
    }
}