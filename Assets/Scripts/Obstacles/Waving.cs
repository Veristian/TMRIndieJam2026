using UnityEngine;

public class Waving : MonoBehaviour
{
    [Header("Wave Settings")]
    public float amplitudeX = 0.5f;
    public float amplitudeZ = 0.5f;

    public float frequencyX = 1f;
    public float frequencyZ = 1.2f;

    public float speed = 1f;
    public float rotationSpeed = 30f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float time = Time.time * speed;

        float offsetX = Mathf.Sin(time * frequencyX) * amplitudeX;
        float offsetZ = Mathf.Cos(time * frequencyZ) * amplitudeZ;

        transform.position = new Vector3(
            startPos.x + offsetX,
            startPos.y,
            startPos.z + offsetZ
        );

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

    }
}