using UnityEngine;

public class HouseTrigger : MonoBehaviour
{
    [Header("Visual & Trigger")]
    public MeshRenderer meshRenderer;
    public Collider triggerCollider;

    private void Start()
    {
        SetActive(false);
        if (DayManager.Instance != null)
            DayManager.Instance.OnDayEnd += () => SetActive(true);
    }

    public void SetActive(bool active)
    {
        if (meshRenderer != null) meshRenderer.enabled = active;
        if (triggerCollider != null) triggerCollider.enabled = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && DaySummaryUI.Instance != null)
            DaySummaryUI.Instance.ShowSummary();
    }
}
