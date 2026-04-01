using UnityEngine;
using System.Collections;
public class PlayerAttribute : MonoBehaviour
{
    private static bool isHiddenFromPredators = false;
    public static bool IsHiddenFromPredators
    {
        get { return isHiddenFromPredators; }
        set { isHiddenFromPredators = value; }
    }

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

    private void Awake()
    {
        playerTransform = transform;
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
            TakeDamage(toxicDamageRate * Time.deltaTime); // Example damage over time in toxic area
        }
    }

    public static void TakeDamage(float damageAmount)
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
