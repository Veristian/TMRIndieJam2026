using UnityEngine;
using UnityEngine.AI;

public class Fish : Agent
{
    // public Transform player;

    [Header("Area Constraint")]
    public Vector3 areaCenter;
    public float areaRadius = 15f;

    // [Header("Flee Settings")]
    // public float updateRate = 0.5f;
    [Header("Detection Settings")]
    // public float detectionRate = 0.5f;
    public int detectionRadius = 5;
    // private NavMeshAgent agent;
    private float timer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (areaCenter == Vector3.zero)
            areaCenter = transform.position;
    }

    private void Update()
    {

        if (Vector3.Distance(transform.position, PlayerAttribute.PlayerTransform.position) <= detectionRadius)
        {
            Flee();
        }
        
    }

    void Flee()
    {
        
        Vector3 aiPos = transform.position;
        Vector3 playerPos = PlayerAttribute.PlayerTransform.position;

        // Direction away from player
        Vector3 forward = (aiPos - playerPos).normalized;
        if (agent.hasPath && agent.remainingDistance > 0.5f && Vector3.Dot(agent.destination - aiPos, forward) > 0)
            return; // Already fleeing
        // Perpendicular (right vector)
        Vector3 right = new Vector3(-forward.z, 0, forward.x);

        // --- SAMPLE HALF-CIRCLE ---
        
        // Angle in radians (-90° to +90°)
        float angle = Random.Range(-Mathf.PI / 2f, Mathf.PI / 2f);

        // Proper radius distribution (uniform over area)
        float radius = Mathf.Sqrt(Random.value) * areaRadius;

        // Local offset in half-circle
        Vector3 localOffset =
            forward * Mathf.Cos(angle) * radius +
            right   * Mathf.Sin(angle) * radius;

        // Convert to world position
        Vector3 targetPos = areaCenter + localOffset;

        // Snap to NavMesh
        NavMeshHit hit;
        Debug.Log($"Trying to flee to {targetPos} (angle: {angle * Mathf.Rad2Deg:F1}°, radius: {radius:F1})");
        if (NavMesh.SamplePosition(targetPos, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw area
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(areaCenter, areaRadius);

        if (PlayerAttribute.PlayerTransform != null)
        {
            Vector3 aiPos = transform.position;
            Vector3 playerPos = PlayerAttribute.PlayerTransform.position;
            Vector3 midpoint = (aiPos + playerPos) * 0.5f;

            Vector3 awayDir = (aiPos - playerPos).normalized;
            Vector3 perpendicular = new Vector3(-awayDir.z, 0, awayDir.x);

            // Draw dividing line
            Gizmos.color = Color.red;
            Gizmos.DrawLine(midpoint - perpendicular * areaRadius, midpoint + perpendicular * areaRadius);

            // Draw flee direction
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(midpoint, awayDir * 5f);
        }
    }
}