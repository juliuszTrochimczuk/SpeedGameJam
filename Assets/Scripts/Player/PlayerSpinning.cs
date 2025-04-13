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
        
        private Animator anim;
        

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            statesHandler = GetComponent<PlayerStatesHandler>();
            anim = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate()
        {
            if (statesHandler.CurrentState != PlayerStatesHandler.PlayerState.Spinning)
                return;

            if (!Physics.Raycast(transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground")))
                return;
            
            if (speedingUpCoroutine == null && slowingDownCoroutine == null)
                slowingDownCoroutine = StartCoroutine(LerpingStrengthDown(baseStrength, slowingDownTime));
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
                anim.SetBool("Spin", true);
            };
            context.action.canceled += _ =>
            {
                transform.localScale = new Vector3(1, 0.5f, 3);
                statesHandler.CurrentState = PlayerStatesHandler.PlayerState.Moving;
                anim.SetBool("Spin", false);
            };
        }

        public void ChangeDirection(Vector3 direction) => this.direction = direction;

        public void ChangeStrength(float addition, float duration)
        {
            if (speedingUpCoroutine != null)
                StopCoroutine(speedingUpCoroutine);
            speedingUpCoroutine = StartCoroutine(LerpingStrengthUp(strength + addition, duration));
        }

        private IEnumerator LerpingStrengthUp(float targetStrength, float duration)
        {
            float time = 0.0f;
            float oldStrength = strength;
            while (time < duration)
            {
                strength = Mathf.Lerp(oldStrength, targetStrength, time / duration);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            strength = targetStrength;
            speedingUpCoroutine = null;
        }
        
        private IEnumerator LerpingStrengthDown(float targetStrength, float duration)
        {
            float time = 0.0f;
            float oldStrength = strength;
            while (time < duration)
            {
                strength = Mathf.Lerp(oldStrength, targetStrength, time / duration);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            strength = targetStrength;
            slowingDownCoroutine = null;
        }
    }
}