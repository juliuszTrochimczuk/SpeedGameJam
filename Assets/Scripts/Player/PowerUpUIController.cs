using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PowerUpUIController : MonoBehaviour {
     private Image image;
     
    [SerializeField] private float transitionTime = 1.0f;
    [SerializeField] private float sustainTime = 1.0f;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void UpdateImage(Sprite sprite) {
        image.sprite = sprite;
        StartCoroutine(ImageEffect());
    }

    private IEnumerator ImageEffect()
    {
        yield return StartCoroutine(LerpImage(0, 1));
        yield return new WaitForSeconds(sustainTime);
        yield return StartCoroutine(LerpImage(1, 0));
    }
    
    private IEnumerator LerpImage(float from, float to) 
    {
        float time = 0.0f;
        while (time < transitionTime)
        {
            image.color = Color.Lerp(new Color(1, 1, 1, from), new Color(1, 1, 1, to), time / transitionTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        image.color = new Color(1, 1, 1, to);
    }
        
}
