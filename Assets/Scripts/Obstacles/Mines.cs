using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class Mines : MonoBehaviour
{
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private float timeToExplode = 2f; // Time in seconds before the mine explodes after being triggered
    private bool isTriggered = false;
    private bool playerInRange = false;
    private ParticleSystem explosionEffect;
    private SpriteRenderer spriteRenderer;
    private SphereCollider col;
    private CinemachineImpulseSource cinemachineImpulseSource;
    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        explosionEffect = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ExplodeAfterDelay(other.gameObject));
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

    IEnumerator ExplodeAfterDelay(GameObject player)
    {
        yield return new WaitForSeconds(timeToExplode);
        if (isTriggered)
        {
            Vector3 explosionDirection = (player.transform.position - transform.position).normalized;
            if (playerInRange)
            {
                PlayerAttribute.TakeDamage(damageAmount);
                player.GetComponent<Rigidbody>().AddForce(explosionDirection * 20f, ForceMode.Impulse); // Example explosion force
                cinemachineImpulseSource.GenerateImpulseWithVelocity(explosionDirection * 2f); // Example impulse direction and magnitude

            }
            cinemachineImpulseSource.GenerateImpulseWithVelocity(explosionDirection * .5f); // Example impulse direction and magnitude

            Debug.Log("Mine exploded! Player health: " + PlayerAttribute.PlayerHealth);
            DestroyMine();
        }
    }

    private void DestroyMine()
    {
        explosionEffect.Play();
        spriteRenderer.enabled = false; // Hide the mine's sprite
        Destroy(gameObject, explosionEffect.main.duration); // Destroy the mine after the explosion effect finishes
    }
    
}
