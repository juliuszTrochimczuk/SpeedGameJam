using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PowerUps
{
    public class PowerUpUiController : MonoBehaviour
    {
        [SerializeField] private Image image;

        [SerializeField] private float updateSpeed;

        public void UpdateImage(Sprite sprite) {
            image.sprite = sprite;
            StartCoroutine(LerpImage(0, 1));
        }

        private IEnumerator LerpImage(float from, float to) {
            float alpha = from;
            while (alpha < to)
            {
                this.image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                alpha += Time.deltaTime * updateSpeed * (to - from);
                yield return new WaitForEndOfFrame();
            }
        }
        
    }
}