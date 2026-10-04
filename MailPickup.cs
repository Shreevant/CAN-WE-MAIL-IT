using UnityEngine;

public class MailPickup
{
    [Header("Mail Identity")]
    public MailType mailType = MailType.Blue;
    public int hiddenValue = 0; // revealed only on delivery
    public string deliveryPointName = "Post Office";

    [Header("Pickup State")]
    public bool isPickedUp = false;

    // Called by player / interaction
    public bool TryPickup()
    {
        // Only allow pickup if no mail currently held (enforced by MailManager)
        if (MailManager.Instance != null && MailManager.Instance.HasActiveMail())
            return false;

        isPickedUp = true;
        return true;
    }
}
