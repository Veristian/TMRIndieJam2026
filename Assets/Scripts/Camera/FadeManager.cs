using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Material vignetteMaterial;
    [SerializeField] private float defaultVignetteIntensity = 1.79f;


    [Header("Color Gradient")]
    [SerializeField] private Gradient fadeInGradient;
    [SerializeField] private Gradient vignetteInMaterialGradient;
    [SerializeField] private Gradient fadeOutGradient;
    [SerializeField] private Gradient vignetteOutMaterialGradient;

    private void Start()
    {
        vignetteMaterial.SetFloat("_VignetteIntensity", defaultVignetteIntensity);
        StartCoroutine(FadeIn());
    }

    [Button("Fade Out and Load Game")]
    public void FadeOutAndLoadGame()
    {
        StartCoroutine(FadeOut("GameScene"));
    }
    public void FadeOutAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    public void FadeInScene()
    {
        StartCoroutine(FadeIn());
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
                fadeImage.color = fadeInGradient.Evaluate(t);
            if (vignetteMaterial != null)
                vignetteMaterial.SetColor("_FCColor", vignetteInMaterialGradient.Evaluate(t));

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
                fadeImage.color = fadeOutGradient.Evaluate(t);
            if (vignetteMaterial != null)
                vignetteMaterial.SetColor("_FCColor", vignetteOutMaterialGradient.Evaluate(t));

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        SceneManager.Instance.LoadScene(sceneName);
    }

    private void OnDisable()
    {
        vignetteMaterial.SetColor("_FCColor", Color.black);
        vignetteMaterial.SetFloat("_VignetteIntensity", 0);
    }
}