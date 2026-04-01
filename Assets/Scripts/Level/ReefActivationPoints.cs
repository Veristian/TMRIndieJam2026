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

    private bool playerInRange = false;

    private void Awake()
    {
        activationCollider = GetComponent<SphereCollider>();
        activationCollider.isTrigger = true;
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
            if (cleanedProgress >= cleanedThreshold)
            {
                gameObject.SetActive(false); // Deactivate point when cleaned
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
