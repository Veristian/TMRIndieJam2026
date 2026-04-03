using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.GlobalIllumination;
public class PlayerAttribute : MonoBehaviour
{
    public static event System.Action OnPlayerDamageTaken;
    public static event System.Action OnPlayerHealed;
    public static event System.Action OnPlayerAteFood;
    private static bool isChasedByPredators = false;
    public static bool IsChasedByPredators
    {
        get { return isChasedByPredators; }
        set { isChasedByPredators = value; }
    }
    private static bool isHiddenFromPredators = false;
    public static bool IsHiddenFromPredators
    {
        get { return isHiddenFromPredators; }
        set { isHiddenFromPredators = value; }
    }

    public static float invincibilityDuration = 2f; // Duration of invincibility after taking damage
    private static bool isInvincible = false;
    private static float invincibilityTimer = 0f;

    private static float playerHealth = 100;
    public static float PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = Mathf.Clamp(value, 0, 100); }
    }

    private static Transform playerTransform;
    public static Transform PlayerTransform
    {
        get { return playerTransform; }
        set { playerTransform = value; }
    }
    private SpriteRenderer playerSpriteRenderer;
    private Light playerLight;
    private void Awake()
    {
        playerTransform = transform;
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerLight = GetComponentInChildren<Light>();
    }
    private void Reset()
    {
        playerTransform = transform;
    }
    public static bool isMoving = false;
    public static bool isSprinting = false;
    private static bool insideToxicArea = false;
    private static bool wasMoving = false;
    private static bool wasSprinting = false;
    private static bool wasInsideToxicArea = false;

    public static bool InsideToxicArea
    {
        get { return insideToxicArea; }
    }

    private static float toxicDamageRate = 5f; // Damage per second when inside toxic area
    public static void SetInsideToxicArea(bool inside, float damageRate = 5f)
    {
        insideToxicArea = inside;
        toxicDamageRate = damageRate;
    }
    private AudioSource swimSource;
    private AudioSource sprintSource;
    private AudioSource toxicSource;
    private AudioSource activeAmbience;
    bool ambienceActive;
    bool predatorActive;
    bool healingActive;
    float ambienceTime;
    float predatorTime;
    float healingTime;

    float maxAmbienceTime;
    float maxPredatorTime;
    float maxHealingTime;
    private void Start()
    {
        // activeAmbience = AudioManager.Instance.PlayContinously(SFX.DeepSeaAmbience);
        maxAmbienceTime = AudioManager.Instance.GetAudioLength(SFX.DeepSeaAmbience);
        maxPredatorTime = AudioManager.Instance.GetAudioLength(SFX.PredatorMusic);
        maxHealingTime = AudioManager.Instance.GetAudioLength(SFX.Healing);

    }

    private void UpdatePlayerAudio()
    {
        //bgm sound
        if (isHiddenFromPredators && !isChasedByPredators && !healingActive)
        {
            Debug.Log("1");
            activeAmbience = AudioManager.Instance.CrossFade(activeAmbience, SFX.Healing, healingTime);
            healingActive = true;
            predatorActive = false;
            ambienceActive = false;
        }
        else if (isChasedByPredators && !isHiddenFromPredators && !predatorActive)
        {
            Debug.Log("2");
            activeAmbience = AudioManager.Instance.CrossFade(activeAmbience, SFX.PredatorMusic, predatorTime);
            healingActive = false;
            predatorActive = true;
            ambienceActive = false;
        }
        else if (!ambienceActive && !isChasedByPredators && !isHiddenFromPredators)
        {
            Debug.Log("3");
            activeAmbience = AudioManager.Instance.CrossFade(activeAmbience, SFX.DeepSeaAmbience, ambienceTime);
            healingActive = false;
            predatorActive = false;
            ambienceActive = true;
        }

        if (ambienceActive) 
        {
            ambienceTime += Time.deltaTime;
            if (ambienceTime > maxAmbienceTime) ambienceTime -= maxAmbienceTime;
        }
        if (healingActive) 
        {
            healingTime += Time.deltaTime;
            if (healingTime > maxHealingTime) healingTime -= maxHealingTime;
        }
        if (predatorActive) 
        {
            predatorTime += Time.deltaTime;
            if (predatorTime > maxPredatorTime) predatorTime -= maxPredatorTime;
        }


        //player sound
        if (isMoving && !wasMoving)
        {
            swimSource = AudioManager.Instance.PlayContinously(SFX.Bubbles);
        }
        else if (!isMoving && wasMoving)
        {
            AudioManager.Instance.StopContinous(swimSource);
            swimSource = null;
        }

        if (isSprinting && !wasSprinting)
        {
            sprintSource = AudioManager.Instance.PlayContinously(SFX.Swim);
        }
        else if (!isSprinting && wasSprinting)
        {
            AudioManager.Instance.StopContinous(sprintSource);
            sprintSource = null;
        }

        if (insideToxicArea && !wasInsideToxicArea)
        {
            toxicSource = AudioManager.Instance.PlayContinously(SFX.InToxicWater);
        }
        else if (!insideToxicArea && wasInsideToxicArea)
        {
            AudioManager.Instance.StopContinous(toxicSource);
            toxicSource = null;
        }

        wasMoving = isMoving;
        wasSprinting = isSprinting;
        wasInsideToxicArea = insideToxicArea;
    }
    private void Update()
    {
        if (insideToxicArea)
        {
            TakeDamageBypass(toxicDamageRate * Time.deltaTime); // Example damage over time in toxic area
            playerSpriteRenderer.color = Color.Lerp(playerSpriteRenderer.color, Color.darkGreen, 1f * Time.deltaTime * 10); // Change color to indicate toxic damage
        }
        else
        {
            playerSpriteRenderer.color = Color.Lerp(playerSpriteRenderer.color, Color.white, 1f * Time.deltaTime * 10); // Reset color when not in toxic area
        }
        if (isInvincible)
        {
            playerSpriteRenderer.color = Color.Lerp(playerSpriteRenderer.color, Mathf.PingPong(Time.time * 5, 1) > 0.5f ? new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0f) : new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 1f), 2f * Time.deltaTime * 10); // Flashing effect
            playerLight.intensity = Mathf.PingPong(Time.time * 5, 2f);
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                playerSpriteRenderer.color = Color.white; // Reset color when invincibility ends
                playerLight.intensity = 2f; // Reset light intensity
            }
        }
        UpdatePlayerAudio();
    }

    
    public static void TakeDamage(float damageAmount)
    {
        if (isInvincible) return;
        PlayerHealth -= damageAmount;
        invincibilityTimer = invincibilityDuration; // Start invincibility timer after taking damage
        isInvincible = true;
        AudioManager.Instance.PlayOneShot(SFX.TakingDamage);
        if (OnPlayerDamageTaken != null)
        {
            OnPlayerDamageTaken.Invoke();
            Debug.Log("Player took damage and is now invincible for " + invincibilityDuration + " seconds!");
        }
        if (PlayerHealth <= 0)
        {
            playerHealth = 0;
            SavePointManager.Instance.ReturnToLastSavePoint();
            Debug.Log("Player has died!");
        }
    }
    
    public static void TakeDamageBypass(float damageAmount)
    {
        PlayerHealth -= damageAmount;
        if (PlayerHealth <= 0)
        {
            playerHealth = 0;
            SavePointManager.Instance.ReturnToLastSavePoint();
            Debug.Log("Player has died!");
        }
    }



    public static void RestoreHealth(float healAmount)
    {
        PlayerHealth += healAmount;
        if (OnPlayerHealed != null)
            OnPlayerHealed.Invoke();
        if (PlayerHealth > 100)
        {
            playerHealth = 100;
        }
    }
    public static void EatFood(float healAmount)
    {
        PlayerHealth += healAmount;
        if (OnPlayerHealed != null)
            OnPlayerHealed.Invoke();
        if (PlayerHealth > 100)
        {
            playerHealth = 100;
        }
        AudioManager.Instance.PlayOneShot(SFX.EatFish);
        if (OnPlayerAteFood != null)
            OnPlayerAteFood.Invoke();
    }


    [Button("Toggle Hide From Predators")]
    public void ToggleHideFromPredators()
    {
        IsHiddenFromPredators = !IsHiddenFromPredators;
        Debug.Log("Player is now " + (IsHiddenFromPredators ? "hidden from predators." : "visible to predators."));
    }

    [Button("Reset Player Health")]
    public void ResetPlayerHealth()
    {
        PlayerHealth = 100;
    }
    
    [Button("Kill Player")]
    public void KillPlayer()
    {
        PlayerHealth = 0;
        SavePointManager.Instance.ReturnToLastSavePoint();
        Debug.Log("Player has been killed!");
    }

    [Button("Debug Player Attributes")]
    public void DebugPlayerAttributes()
    {
        Debug.Log("Player Health: " + PlayerHealth);
        Debug.Log("Is Hidden From Predators: " + IsHiddenFromPredators);
    }

}
