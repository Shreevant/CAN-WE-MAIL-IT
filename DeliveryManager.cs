using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;
    public DeliveryPoint CurrentDestination { get; private set; }
    [Header("Manual Drop Points")]
    public DeliveryPoint[] dropPoints;
    public event Action<string> OnDestinationChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetDestination(DeliveryPoint point)
    {
        DeliveryPoint[] all = (dropPoints != null && dropPoints.Length > 0) ? dropPoints : FindObjectsByType<DeliveryPoint>(FindObjectsSortMode.None);
        foreach (var dp in all)
        {
            if (dp != null) dp.gameObject.SetActive(dp == point);
        }
        CurrentDestination = point;
        OnDestinationChanged?.Invoke(point ? "DropPoint" : "None");
#if UNITY_EDITOR
        Debug.Log("[Delivery] Destination set: " + (point ? "DropPoint" : "None"));
#endif
    }

    public void SetRandomDestination()
    {
        DeliveryPoint[] pool = (dropPoints != null && dropPoints.Length > 0) ? dropPoints : FindObjectsByType<DeliveryPoint>(FindObjectsSortMode.None);
        if (pool.Length > 0)
            SetDestination(pool[UnityEngine.Random.Range(0, pool.Length)]);
    }

    public bool IsAtDestination(Vector3 playerPos)
    {
        return CurrentDestination != null && CurrentDestination.IsAtPoint(playerPos);
    }
}
