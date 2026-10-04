using UnityEngine;

public class PlayerDeliveryInteraction : MonoBehaviour
{
    public Transform playerTransform;
    private void Awake() { if (playerTransform == null) playerTransform = transform; }

    private void Update()
    {
#if UNITY_EDITOR
        if (MailManager.Instance == null) { Debug.Log("[AutoDel] MailManager is NULL"); return; }
#endif
#if UNITY_EDITOR
        if (MailManager.Instance.ActiveMail == null) { Debug.Log("[AutoDel] ActiveMail is NULL (pick mail first)"); return; }
#endif
#if UNITY_EDITOR
        if (DeliveryManager.Instance == null) { Debug.Log("[AutoDel] DeliveryManager is NULL"); return; }
#endif
#if UNITY_EDITOR
        if (DeliveryManager.Instance.CurrentDestination == null) { Debug.Log("[AutoDel] CurrentDestination is NULL"); return; }
#endif

        bool inside = DeliveryManager.Instance.CurrentDestination.playerInside;
#if UNITY_EDITOR
        Debug.Log("[AutoDel] CurrentPoint=" + DeliveryManager.Instance.CurrentDestination.name + " inside=" + inside);
#endif

                if (inside && MailManager.Instance != null && MailManager.Instance.ActiveMail != null && MailManager.Instance.ActiveMail.isPickedUp)
        {
#if UNITY_EDITOR
            Debug.Log("[AutoDel] DELIVERING at " + DeliveryManager.Instance.CurrentDestination.name);
#endif
            MailManager.Instance.DeliverActiveMail();
        }
    }
}
