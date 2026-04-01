using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HealingPickups : MonoBehaviour
{
    public static float healingAmount = 20f; // Amount of health to restore to the player
    private void Awake()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttribute.RestoreHealth(healingAmount);
            Debug.Log("Player collected healing pickup! Health: " + PlayerAttribute.PlayerHealth);
            DestroySelf();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider != null)
            Gizmos.DrawWireSphere(transform.position, collider.radius * transform.localScale.x);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
