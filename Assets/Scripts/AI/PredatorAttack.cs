using UnityEngine;
using Unity.Cinemachine;

public class PredatorAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damageAmount = 20f;
    public LayerMask playerMask;

    [Header("Hitbox")]
    public BoxCollider attackCollider;

    [Header("Knockback")]
    public float knockbackForce = 5f;

    private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void ChompSound()
    {
        AudioManager.Instance.PlayOneShot(SFX.PredatorAttack);
    }
    // Call this from animation event
    public void Chomp()
    {
        if (PlayerAttribute.IsHiddenFromPredators)
            return;
        Debug.Log("Chomp Attack!");
        Vector3 center = attackCollider.bounds.center;
        Vector3 halfExtents = attackCollider.bounds.extents;
        Quaternion rotation = transform.rotation;

        // Instant hit detection (better for melee)
        Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation, playerMask);

        if (hits.Length == 0)
        {
            Debug.Log("Attack missed!");
            return;
        }

        foreach (Collider col in hits)
        {
            Debug.Log("Hit: " + col.name);

            PlayerAttribute.TakeDamage(damageAmount);

            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                Vector3 forceDir = (col.transform.position - transform.position).normalized;
                rb.AddForce(forceDir * knockbackForce, ForceMode.Impulse);
            }

            if (impulseSource != null)
            {
                Vector3 impulseDir = (col.transform.position - transform.position).normalized;
                impulseSource.GenerateImpulseWithVelocity(impulseDir * 5f);
            }
        }
    }

    // 🟢 Debug visualization in editor
    private void OnDrawGizmosSelected()
    {
        if (attackCollider == null) return;

        Gizmos.color = Color.red;
        Gizmos.matrix = attackCollider.transform.localToWorldMatrix;

        Gizmos.DrawWireCube(
            attackCollider.transform.InverseTransformPoint(attackCollider.bounds.center),
            attackCollider.size
        );
    }
}