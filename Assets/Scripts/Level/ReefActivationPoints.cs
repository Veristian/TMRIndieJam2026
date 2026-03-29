using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ReefActivationPoints : MonoBehaviour
{
    private MajorReef parentReef;
    public MajorReef ParentReef
    {
        set { parentReef = value; }
    }
    private SphereCollider activationCollider;
    private float cleanedProgress = 0f;
    private float cleanedThreshold = 1f;
    private float cleaningRate = 0.1f; // Progress per click

    private void Awake()
    {
        activationCollider = GetComponent<SphereCollider>();
        activationCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InputManager.interactWasPressedThisFrame)
            {
                cleanedProgress += cleaningRate;
                if (cleanedProgress >= cleanedThreshold)
                {
                    Debug.Log("Activation point cleaned!");
                    gameObject.SetActive(false);
                    parentReef?.DetectActivationProgress();
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, activationCollider.radius);
    }

    
}
