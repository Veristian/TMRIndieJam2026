using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class Mines : MonoBehaviour
{
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private float timeToExplode = 2f; // Time in seconds before the mine explodes after being triggered
    [SerializeField] private LayerMask playerMask;
    private bool isTriggered = false;
    private bool playerInRange = false;
    private ParticleSystem explosionEffect;
    private SpriteRenderer spriteRenderer;
    private Light mineLight;
    private SphereCollider col;
    private CinemachineImpulseSource cinemachineImpulseSource;
    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        explosionEffect = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        mineLight = GetComponentInChildren<Light>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
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
        yield return new WaitForSeconds(timeToExplode*3.5f/5f);
        AudioManager.Instance.PlayOneShotFree(SFX.UnderwaterExplosion);
        yield return new WaitForSeconds(timeToExplode*1.5f/5f);
        if (isTriggered)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, col.radius * col.transform.localScale.x, playerMask);
            if (hits.Length == 0)
            {
                Debug.Log("Mine missed!");
            }
            else
            {
                foreach (Collider col in hits)
                {
                    Debug.Log("Hit: " + col.name);
                    PlayerAttribute.TakeDamage(damageAmount);

                    Rigidbody rb = col.attachedRigidbody;
                    if (rb != null)
                    {
                        Vector3 forceDir = (col.transform.position - transform.position).normalized;
                        rb.AddForce(forceDir * 20f, ForceMode.Impulse);
                    }

                    if (cinemachineImpulseSource != null)
                    {
                        Vector3 impulseDir = (col.transform.position - transform.position).normalized;
                        cinemachineImpulseSource.GenerateImpulseWithVelocity(impulseDir * 5f);
                    }
                }
            }

            cinemachineImpulseSource.GenerateImpulseWithVelocity(Vector3.up); // Example impulse direction and magnitude

            Debug.Log("Mine exploded! Player health: " + PlayerAttribute.PlayerHealth);
            DestroyMine();
        }
    }

    private void DestroyMine()
    {
        explosionEffect.Play();
        mineLight.enabled = true; // Enable the explosion light
        spriteRenderer.enabled = false; // Hide the mine's sprite
        Destroy(gameObject, explosionEffect.main.duration); // Destroy the mine after the explosion effect finishes
    }
    
}
