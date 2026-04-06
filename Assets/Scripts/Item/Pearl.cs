using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Pearl : MonoBehaviour
{
    private void Start()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayOneShot(SFX.SavePointGot);
            // Add code here to give the player the pearl or trigger any desired effect
            CameraManager.Instance.IncrementVignetteLevel(); // Increment the vignette level in the CameraManager
            Destroy(gameObject); // Destroy the pearl after collection
        }
    }
}
