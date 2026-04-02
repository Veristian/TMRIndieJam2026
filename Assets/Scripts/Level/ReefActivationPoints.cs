using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ReefActivationPoints : MonoBehaviour
{
    [SerializeField] private MajorReef parentReef;
    public MajorReef ParentReef
    {
        set { parentReef = value; }
    }
    [SerializeField] private SphereCollider activationCollider;
    private float cleanedProgress = 0f;
    private float cleanedThreshold = 1f;
    private float cleaningRate = 0.1f; // Progress per click
    private ParticleSystem cleaningEffect;

    private bool playerInRange = false;
    private float timeTillDeactivation = 0.3f; // Time after which the point can be reactivated if not cleaned
    private float deactivationTimer = 0f;
    private void Awake()
    {
        activationCollider = GetComponent<SphereCollider>();
        activationCollider.isTrigger = true;
        deactivationTimer = timeTillDeactivation;
        cleaningEffect = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && InputManager.interactWasPressedThisFrame) // Left click to clean
        {
            cleanedProgress += cleaningRate;
        }
        if (cleanedProgress >= cleanedThreshold)
            {
                cleaningEffect?.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                deactivationTimer -= Time.deltaTime;
                if (deactivationTimer <= 0f)
                {
                    gameObject.SetActive(false); // Deactivate the point
                    parentReef.DetectActivationProgress(); // Notify parent reef to check progress
                }
            }
    }

    private void OnDisable()
    {
        if (parentReef != null)
            parentReef.DetectActivationProgress(); // Notify parent reef to check progress
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (activationCollider != null)
            Gizmos.DrawWireSphere(transform.position, activationCollider.radius * transform.localScale.x * transform.parent.localScale.x);
    }



}
