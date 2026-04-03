using System.Collections.Generic;
using UnityEngine;

public class MajorReef : SafeZone
{
    private Transform savePoint;
    
    [SerializeField] private List<ReefActivationPoints> activationPoints = new List<ReefActivationPoints>();
    [SerializeField] private float activationThreshold = 3f;

    [SerializeField] private GameObject oldReefVisuals;
    [SerializeField] private GameObject newReefVisuals;
    [SerializeField] private ParticleSystem activationEffect;
    private float activationProgress = 0f;


    private void Start()
    {
        SetSafeZoneActive(false);
        if (savePoint == null) savePoint = transform;
    }

    [Button("Create Activation Points")]
    public void CreateActivationPoints()
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
        if (SavePointManager.Instance == null) return;
        SetSafeZoneActive(true);
        SavePointManager.Instance.AddSavePoint(savePoint);
        Debug.Log("Major Reef Activated! Safe zone is now active and save point added.");

        if (oldReefVisuals != null) oldReefVisuals.SetActive(false);
        if (newReefVisuals != null) newReefVisuals.SetActive(true);
        if (activationEffect != null) activationEffect.Play();
        AudioManager.Instance.PlayOneShot(SFX.SavePointGot);
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
