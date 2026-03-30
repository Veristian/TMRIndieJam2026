using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Trash : MonoBehaviour
{
    public static float damageAmount = 10f; // Amount of damage to deal to the player
    private void Awake()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttribute.TakeDamage(damageAmount);
            Debug.Log("Player hit trash! Health: " + PlayerAttribute.PlayerHealth);
        }
    }
}
