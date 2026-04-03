using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected SphereCollider col;
    protected Transform sprite;
    protected bool softEnabled = false;
    protected bool softDisabled = false;
    public float distanceToSoftEnable = 15f;
    public float distanceToSoftDisable = 30f;
    [SerializeField] protected bool alwaysSoftEnable = true;
    [SerializeField] protected bool alwaysSoftDisable = true;

    protected bool isDisabled;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        if (sprite == null) sprite = transform.GetComponentInChildren<SpriteRenderer>().transform;
        if (alwaysSoftDisable) softDisabled = true;
        if (alwaysSoftEnable)
        {
            softEnabled = true;
            DisableAgent();
        } 
    }

    protected void MoveTo(Vector3 destination)
    {
        if (agent != null)
        {
            try
            {
                agent.SetDestination(destination);
            }
            catch
            {
                Debug.LogWarning("Set Destination Failed");
            }
        }
    }

    [ContextMenu("Disable Agent")]
    public virtual void DisableAgent()
    {
        if (agent != null) agent.enabled = false;
        if (col != null) col.enabled = false;
        if (sprite != null) sprite.gameObject.SetActive(false);
        softDisabled = false;
        if (alwaysSoftEnable) softEnabled = true;
        isDisabled = true;
    }
    [ContextMenu("Enable Agent")]
    public virtual void EnableAgent()
    {
        if (agent != null) agent.enabled = true;
        if (col != null) col.enabled = true;
        if (sprite != null) sprite.gameObject.SetActive(true);
        softEnabled = false;
        if (alwaysSoftDisable) softDisabled = true;
        isDisabled = false;
    }

    //ready to enable when player is close enough, but not yet active
    [ContextMenu("Soft Enable Agent")]
    public virtual void SoftEnableAgent()
    {
        if (!softEnabled)
        {
            softEnabled = true;
        }
    }
    [ContextMenu("Soft Disable Agent")]
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
