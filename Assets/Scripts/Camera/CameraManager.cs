using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found in the scene. Please ensure there is a GameObject with the tag 'Player'.");
            return;
        }
        cinemachineCamera.Target = new CameraTarget
        {
            TrackingTarget = player.transform,
            LookAtTarget = player.transform,
            CustomLookAtTarget = false
        };  
    }
}
