using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    private static bool isHiddenFromPredators = false;
    public static bool IsHiddenFromPredators
    {
        get { return isHiddenFromPredators; }
        set { isHiddenFromPredators = value; }
    }

    private static float playerHealth = 100;
    public static float PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = Mathf.Clamp(value, 0, 100); }
    }

    private static Transform playerTransform;
    public static Transform PlayerTransform
    {
        get { return playerTransform; }
        set { playerTransform = value; }
    }

    private void Awake()
    {
        playerTransform = transform;
    }
    private void Reset()
    {
        playerTransform = transform;
    }
}
