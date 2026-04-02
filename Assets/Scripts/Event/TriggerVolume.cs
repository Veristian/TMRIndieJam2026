using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerVolume : MonoBehaviour
{
    public string triggerTag = "Player";
    public bool triggerOnce = true;
    private bool hasBeenTriggered = false;
    public UnityEvent OnTriggerEnterEvent;

    private void Reset()
    {
        // Ensure the collider is set as a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }
    private void Awake()
    {
        // Ensure the collider is set as a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag) && (!triggerOnce || !hasBeenTriggered))
        {
            hasBeenTriggered = true;
            // Implement your trigger logic here, e.g., enable/disable agents, play sounds, etc.
            Debug.Log("Trigger activated by: " + other.name);
            OnTriggerEnterEvent.Invoke();
        }
    }
}
