using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SavePointManager : Singleton<SavePointManager>
{
    [SerializeField] private List<Transform> savePoints;

    private void Start()
    {
        if (savePoints == null) savePoints = new List<Transform>();
        GameObject defaultSavePoint = new GameObject("Default Save Point");
        defaultSavePoint.transform.position = PlayerAttribute.PlayerTransform.position; // Set to player's starting position
        AddSavePoint(defaultSavePoint.transform);
    }


    public void AddSavePoint(Transform savePoint)
    {
        Debug.Log("Adding save point: " + savePoint);
        RemoveEmptySavePoints();
        if (!savePoints.Contains(savePoint))
        {
            savePoints.Add(savePoint);
        }
    }

    public Vector3 GetLastSavePoint()
    {
        RemoveEmptySavePoints();
        if (savePoints.Count > 0)
        {
            return savePoints[savePoints.Count - 1].position;
        }
        else
        {
            Debug.LogWarning("No save points available. Returning default position.");
            return Vector3.zero; // Return a default value if no save points are available
        }
    }

    [Button("Return To Last Save Point")]
    public void ReturnToLastSavePoint()
    {
        FadeManager.Instance.FadeInScene();
        Vector3 lastSavePoint = GetLastSavePoint();
        PlayerAttribute.PlayerTransform.position = lastSavePoint;
        PlayerAttribute.PlayerHealth = 100; // Restore health on respawn
    }

    // private void OnDrawGizmos()
    // {
    //     if (savePoints != null)
    //     {
    //         Gizmos.color = Color.green;
    //         foreach (var point in savePoints)
    //         {
    //             Gizmos.DrawSphere(point.position, 0.5f);
    //         }
    //     }
    // }

    private void RemoveEmptySavePoints()
    {
        if (savePoints != null)
        {
            savePoints.RemoveAll(point => point == null);
        }
    }

    [Button("Debug Save Points")]
    public void DebugSavePoints()
    {
        if (savePoints == null || savePoints.Count == 0)
        {
            Debug.Log("No save points available.");
            return;
        }
        foreach (var point in savePoints)
        {
            Debug.Log("Save Point: " + point);
        }
    }




}
