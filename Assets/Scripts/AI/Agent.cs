using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected SphereCollider col;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
    }

    protected void MoveTo(Vector3 destination)
    {
        if (agent != null)
        {
            agent.SetDestination(destination);
        }
    }


}
