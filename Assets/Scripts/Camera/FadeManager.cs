using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Color Gradient")]
    [SerializeField] private Gradient fadeGradient;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    [Button("Fade Out and Load Scene")]
    public void FadeOutAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            // Alpha fade
            fadeCanvasGroup.alpha = 1f - t;

            // 🎨 Gradient color (reverse for fade in)
            if (fadeImage != null)
                fadeImage.color = fadeGradient.Evaluate(1f - t);

            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            // Alpha fade
            fadeCanvasGroup.alpha = t;

            // 🎨 Gradient color
            if (fadeImage != null)
                fadeImage.color = fadeGradient.Evaluate(t);

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        SceneManager.Instance.LoadScene(sceneName);
    }
}