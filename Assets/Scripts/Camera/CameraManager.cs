using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : Singleton<CameraManager>
{
    private CinemachineCamera cinemachineCamera;
    [SerializeField]
    private GameObject vignetteLv1;
    [SerializeField]
    private GameObject vignetteLv2;
    [SerializeField]
    private GameObject vignetteLv3;

    protected override void Awake()
    {
        base.Awake();
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
        IncrementVignetteLevel();

    }

    public void IncrementVignetteLevel()
    {
        if (vignetteLv1 != null && !vignetteLv1.activeSelf)
        {
            vignetteLv1.SetActive(true);
        }
        else if (vignetteLv2 != null && !vignetteLv2.activeSelf)
        {
            vignetteLv2.SetActive(true);
        }
        else if (vignetteLv3 != null && !vignetteLv3.activeSelf)
        {
            vignetteLv3.SetActive(true);
        }
    }

    
}
