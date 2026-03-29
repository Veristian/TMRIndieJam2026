using System.Collections.Generic;
using UnityEngine;

public class MajorReef : SafeZone
{
    private Transform savePoint;
    
    private List<ReefActivationPoints> activationPoints = new List<ReefActivationPoints>();
    [SerializeField] private float activationThreshold = 3f;
    private float activationProgress = 0f;

    private void OnValidate()
    {
        CreateActivationPoints();
    }
    private void Start()
    {
        SetSafeZoneActive(false);
        if (savePoint == null) savePoint = transform;
        CreateActivationPoints();
    }

    private void CreateActivationPoints()
    {
        if (activationPoints.Count > 0) return; // Avoid creating points multiple times
        if (activationThreshold <= 0) return; 
        if (safeZoneCollider == null) return;
        for (int i = 0; i < activationThreshold; i++)
        {
            GameObject pointObj = new GameObject($"ActivationPoint_{i + 1}");
            pointObj.transform.SetParent(transform);
            pointObj.transform.localPosition = new Vector3(Random.Range(-safeZoneCollider.radius, safeZoneCollider.radius), 0, Random.Range(-safeZoneCollider.radius, safeZoneCollider.radius));
            ReefActivationPoints activationPoint = pointObj.AddComponent<ReefActivationPoints>();
            activationPoint.ParentReef = this;
            activationPoints.Add(activationPoint);
        }
    }

    [Button("Activate Major Reef")]
    public void ActivateMajorReef()
    {
        SetSafeZoneActive(true);
        SavePointManager.Instance.AddSavePoint(savePoint);
    }

    [Button("Detect Activation Progress")]
    public void DetectActivationProgress()
    {
        activationProgress = 0f;
        foreach (var point in activationPoints)
        {
            if (!point.gameObject.activeInHierarchy)
                activationProgress += 1f;
        }
        if (activationProgress >= activationThreshold)
        {
            ActivateMajorReef();
        }
    }


}
