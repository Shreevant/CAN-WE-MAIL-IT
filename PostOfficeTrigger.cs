using UnityEngine;

public class PostOfficeTrigger : MonoBehaviour
{
    public PickMailUI pickUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MailManager.Instance != null && MailManager.Instance.HasActiveMail()) return;
            if (DayManager.Instance != null && !DayManager.Instance.IsDayActive) return;
            if (pickUI != null) pickUI.OpenSelection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickUI != null) pickUI.CloseSelection();
        }
    }
}
