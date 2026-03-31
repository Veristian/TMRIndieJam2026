using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ToxicWater : MonoBehaviour
{
    [SerializeField] private float damageRate = 5f; // Damage per second when inside toxic water

    private void Awake()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttribute.SetInsideToxicArea(true, damageRate);
            Debug.Log("Player entered toxic water!");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttribute.SetInsideToxicArea(true, damageRate);
            Debug.Log("Player is in toxic water!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttribute.SetInsideToxicArea(false, damageRate);
            Debug.Log("Player left toxic water!");
        }
    }
}
