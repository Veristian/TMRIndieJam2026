using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    [SerializeField] private GameObject minimap;
    bool isOpen;

    private void Start()
    {
        CloseMinimap();
    }

    public void ToggleMap()
    {
        if (isOpen)
        {
            CloseMinimap();
        }
        else
        {
            OpenMinimap();
        }
    }
    
    public void OpenMinimap()
    {
        minimap.SetActive(true);
        isOpen = true;
    }
    public void CloseMinimap()
    {
        minimap.SetActive(false);
        isOpen = false;
    }

    private void Update()
    {
        if (InputManager.tabWasPressedThisFrame)
        {
            ToggleMap();
        }
    }
}
