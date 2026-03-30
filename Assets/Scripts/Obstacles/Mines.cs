using System.Collections;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class Mines : MonoBehaviour
{
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private float timeToExplode = 2f; // Time in seconds before the mine explodes after being triggered
    private bool isTriggered = false;
    private bool playerInRange = false;
    private void Awake()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ExplodeAfterDelay());
            isTriggered = true;
            playerInRange = true;
            Debug.Log("Player hit a mine and took " + damageAmount + " damage!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(timeToExplode);
        if (isTriggered)
        {
            if (playerInRange)
                PlayerAttribute.TakeDamage(damageAmount);
            Debug.Log("Mine exploded! Player health: " + PlayerAttribute.PlayerHealth);
            DestroyMine();
        }
    }

    private void DestroyMine()
    {
        Destroy(gameObject);
    }
    
}
