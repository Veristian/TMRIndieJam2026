using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected SphereCollider col;
    protected Transform sprite;
    private bool softEnabled = false;
    private bool softDisabled = false;
    public float distanceToSoftEnable = 15f;
    public float distanceToSoftDisable = 30f;

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

    public virtual void DisableAgent()
    {
        if (agent != null) agent.enabled = false;
        if (col != null) col.enabled = false;
        if (sprite != null) sprite.gameObject.SetActive(false);
    }

    public virtual void EnableAgent()
    {
        if (agent != null) agent.enabled = true;
        if (col != null) col.enabled = true;
        if (sprite != null) sprite.gameObject.SetActive(true);
    }

    //ready to enable when player is close enough, but not yet active
    public virtual void SoftEnableAgent()
    {
        if (!softEnabled)
        {
            softEnabled = true;
        }
    }

    public virtual void SoftDisableAgent()
    {
        if (!softDisabled)
        {
            softDisabled = true;
        }
    }

    protected virtual void Update()
    {
        if (softEnabled && !agent.enabled && Vector3.Distance(transform.position, PlayerAttribute.PlayerTransform.position) < distanceToSoftEnable)
        {
            EnableAgent();
        }
        if (softDisabled && agent.enabled && Vector3.Distance(transform.position, PlayerAttribute.PlayerTransform.position) > distanceToSoftDisable)
        {
            DisableAgent();
        }
    }

    


}
