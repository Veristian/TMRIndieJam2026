using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.GlobalIllumination;
public class PlayerAttribute : MonoBehaviour
{
    public static event System.Action OnPlayerDamageTaken;
    public static event System.Action OnPlayerHealed;
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
    private PlayerEffects playerEffects;
    private void Awake()
    {
        playerTransform = transform;
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerLight = GetComponentInChildren<Light>();
        playerEffects = GetComponent<PlayerEffects>();
    }
    private void Reset()
    {
        playerTransform = transform;
    }


    private static bool insideToxicArea = false;

    private static float toxicDamageRate = 5f; // Damage per second when inside toxic area
    public static void SetInsideToxicArea(bool inside, float damageRate = 5f)
    {
        insideToxicArea = inside;
        toxicDamageRate = damageRate;

    }
    private void Update()
    {
        if (insideToxicArea)
        {
            TakeDamageBypass(toxicDamageRate * Time.deltaTime); // Example damage over time in toxic area
            playerSpriteRenderer.color = Color.Lerp(playerSpriteRenderer.color, Color.darkGreen, 0.1f); // Change color to indicate toxic damage
        }
        else
        {
            playerSpriteRenderer.color = Color.Lerp(playerSpriteRenderer.color, Color.white, 0.1f); // Reset color when not in toxic area
        }
        if (isInvincible)
        {
            playerSpriteRenderer.color = Color.Lerp(playerSpriteRenderer.color, Mathf.PingPong(Time.time * 5, 1) > 0.5f ? new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0f) : new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 1f), 0.2f); // Flashing effect
            playerLight.intensity = Mathf.PingPong(Time.time * 5, 100f);
            Debug.Log(Mathf.PingPong(Time.time * 10, 1) > 0.5f);
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                playerSpriteRenderer.color = Color.white; // Reset color when invincibility ends
                playerLight.intensity = 100f; // Reset light intensity
            }
        }
    }

    
    public static void TakeDamage(float damageAmount)
    {
        if (isInvincible) return;
        PlayerHealth -= damageAmount;
        invincibilityTimer = invincibilityDuration; // Start invincibility timer after taking damage
        isInvincible = true;
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
