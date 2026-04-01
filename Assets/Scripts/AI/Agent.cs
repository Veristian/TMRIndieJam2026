using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected SphereCollider col;
    protected Transform sprite;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        if (sprite == null) sprite = transform.GetComponentInChildren<SpriteRenderer>().transform;

    }

    protected void MoveTo(Vector3 destination)
    {
        if (agent != null)
        {
            agent.SetDestination(destination);
        }
    }


}
