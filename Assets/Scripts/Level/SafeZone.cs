using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SafeZone : MonoBehaviour
{
    //Safe zone heals players and allow then to hide from predators
    [Tooltip("Amount of health restored per second")]
    public float healRate = 5f; // Amount of health restored per second

    protected bool isActive = false;
    protected SphereCollider safeZoneCollider;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            // Heal the player or provide safety
            Debug.Log("Player entered the safe zone");
            PlayerAttribute.IsHiddenFromPredators = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            // Player leaves the safe zone
            Debug.Log("Player left the safe zone");
            PlayerAttribute.IsHiddenFromPredators = false;
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            HealPlayer();
            PlayerAttribute.IsHiddenFromPredators = true;
        }
    }

    private void HealPlayer()
    {
        PlayerAttribute.RestoreHealth(healRate * Time.deltaTime);
    }

    protected void SetSafeZoneActive(bool active)
    {
        isActive = active;
    }


    private void OnDrawGizmos()
    {
        if (isActive)
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.red;
        if (safeZoneCollider != null)
            Gizmos.DrawWireSphere(transform.position, safeZoneCollider.radius * transform.localScale.x);
    }
    
    protected virtual void Awake()
    {
        safeZoneCollider = GetComponent<SphereCollider>();
        safeZoneCollider.isTrigger = true;
    }
    private void Reset()
    {
        safeZoneCollider = GetComponent<SphereCollider>();
        safeZoneCollider.isTrigger = true;
    }
}
