using UnityEngine;

public class FullScreenIndicatorManagers : MonoBehaviour
{
    public float minLowHealthVignette = 1.2f;
    public float maxLowHealthVignette = 1.6f;
    public float minInToxicWaterVignette = 0;
    public float maxInToxicWaterVignette = 1.56f;
    public float minInToxicWaterTransparency = 0;
    public float maxInToxicWaterTransparency = 0.14f;

    public Material lowHealthMaterial;
    public Material inToxicWaterMaterial;

    private float toxicWaterExposure;

    private void Reset()
    {
        minLowHealthVignette = 1.2f;
        maxLowHealthVignette = 1.6f;
        minInToxicWaterVignette = 0;
        maxInToxicWaterVignette = 1.56f;
        minInToxicWaterTransparency = 0;
        maxInToxicWaterTransparency = 0.14f;
    }

    private void Start()
    {
        if (lowHealthMaterial == null)
        {
            Debug.LogError("Low Health Material is not assigned in the inspector.");
        }

        if (inToxicWaterMaterial == null)
        {
            Debug.LogError("In Toxic Water Material is not assigned in the inspector.");
        }
    }

    private void Update()
    {   
        if (PlayerAttribute.PlayerHealth < 50)
        {
            float playerHealthPercentage = (PlayerAttribute.PlayerHealth/100)*(maxLowHealthVignette - minLowHealthVignette) + minLowHealthVignette;
            UpdateLowHealthVignette(playerHealthPercentage);
        }
        else
        {
            UpdateLowHealthVignette(0f);
        }
        print("Inside Toxic Area: " + PlayerAttribute.InsideToxicArea);
        toxicWaterExposure = Mathf.Lerp(toxicWaterExposure, PlayerAttribute.InsideToxicArea ? 1f : 0f, 0.1f);
        print ("Toxic Water Exposure: " + toxicWaterExposure);
        float playerToxicWaterPercentage = toxicWaterExposure*(maxInToxicWaterVignette - minInToxicWaterVignette) + minInToxicWaterVignette;
        float playerToxicWaterTransparency = toxicWaterExposure*(maxInToxicWaterTransparency - minInToxicWaterTransparency) + minInToxicWaterTransparency;
        print("Player Toxic Water Percentage: " + playerToxicWaterPercentage);
        print("Player Toxic Water Transparency: " + playerToxicWaterTransparency);
        UpdateInToxicWaterEffect(playerToxicWaterPercentage, playerToxicWaterTransparency);
    }

    private void UpdateLowHealthVignette(float healthPercentage)
    {
        if (lowHealthMaterial != null)
        {
            float vignetteIntensity = healthPercentage;
            lowHealthMaterial.SetFloat("_VignetteIntensity", vignetteIntensity);
        }
    }

    private void UpdateInToxicWaterEffect(float toxicWaterPercentage, float transparency)
    {
        if (inToxicWaterMaterial != null)
        {
            float vignetteIntensity = toxicWaterPercentage;
            inToxicWaterMaterial.SetFloat("_VignetteIntensity", vignetteIntensity);
            inToxicWaterMaterial.SetFloat("_Alpha", transparency);
        }
    }

    private void OnEnable()
    {
        UpdateLowHealthVignette(0f);
        UpdateInToxicWaterEffect(0f, 0f);
    }

    private void OnDisable()
    {
        UpdateLowHealthVignette(0f);
        UpdateInToxicWaterEffect(0f, 0f);
    }



}
