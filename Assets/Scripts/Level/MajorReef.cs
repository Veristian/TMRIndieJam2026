using UnityEngine;

public class MajorReef : SafeZone
{
    private Transform savePoint;
    private void Start()
    {
        SetSafeZoneActive(false);
        if (savePoint == null) savePoint = transform;
    }

    [Button("Activate Major Reef")]
    private void ActivateMajorReef()
    {
        SetSafeZoneActive(true);
        SavePointManager.Instance.AddSavePoint(savePoint);
    }


}
