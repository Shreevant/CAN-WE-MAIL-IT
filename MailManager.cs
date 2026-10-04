using UnityEngine;
using System;

public class MailManager : MonoBehaviour
{
    public static MailManager Instance;
    public MailPickup ActiveMail { get; set; }
    public event Action<MailPickup> OnMailDelivered;
    public static int TotalDelivered = 0;
    public static int TodayDelivered = 0;

        [Header("Yellow Mail Tuning")]
    [SerializeField] private float yellowChangeInterval = 120f;

        [Header("Mail Fixed Values")]
    public int blueValue = 12;
    public int redValue = 8;
    public int yellowValue = 15;

        private TemporaryBindingManager binder;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        binder = FindFirstObjectByType<TemporaryBindingManager>();
    }

        public void CancelActiveMail()
    {
        if (ActiveMail == null) return;
#if UNITY_EDITOR
        Debug.Log("[Mail] Cancelled due to day end");
#endif
        ActiveMail.isPickedUp = false;

                if (binder != null) binder.ResetToDefaults();

        ActiveMail = null;
        if (DeliveryManager.Instance != null) DeliveryManager.Instance.SetDestination(null);
    }

    public void ResetTodayDelivered() => TodayDelivered = 0;

    public bool HasActiveMail() => ActiveMail != null && ActiveMail.isPickedUp;

        public void DeliverActiveMail()
    {
        if (ActiveMail == null || !ActiveMail.isPickedUp) return;
#if UNITY_EDITOR
        Debug.Log("[Mail] Delivered: " + ActiveMail.mailType + " Value=" + ActiveMail.hiddenValue);
#endif
        OnMailDelivered?.Invoke(ActiveMail);
                if (ActiveMail.mailType == MailType.Blue)
        {
            if (binder != null) binder.ResetToDefaults();
        }
        if (RevenueManager.Instance != null && ActiveMail.hiddenValue > 0)
            RevenueManager.Instance.AddRevenue(ActiveMail.hiddenValue);

        ActiveMail.isPickedUp = false;
        TotalDelivered++;
        TodayDelivered++;
        ActiveMail = null;

        if (DeliveryManager.Instance != null)
            DeliveryManager.Instance.SetDestination(null);
    }

    public void SelectMail(MailType type)
    {
        ActiveMail = new MailPickup();
        ActiveMail.mailType = type;
        ActiveMail.isPickedUp = true;
        ActiveMail.hiddenValue = RandomHiddenValue(type);
        ActiveMail.deliveryPointName = "Random";
#if UNITY_EDITOR
        Debug.Log("[Mail] Selected: " + type + " Value=" + ActiveMail.hiddenValue);
#endif
        if (DeliveryManager.Instance != null)
            DeliveryManager.Instance.SetRandomDestination();
                if (type == MailType.Blue)
        {
            if (binder != null) binder.StartRandomTimer();
        }
        if (type == MailType.Yellow)
            StartCoroutine(YellowDestinationLoop());
    }

        private int RandomHiddenValue(MailType type)
        {
            if (type == MailType.Blue) return blueValue;
            if (type == MailType.Red) return redValue;
            return yellowValue;
        }

    private System.Collections.IEnumerator YellowDestinationLoop()
    {
        while (ActiveMail != null && ActiveMail.mailType == MailType.Yellow)
        {
            yield return new WaitForSeconds(yellowChangeInterval);
            if (ActiveMail != null && ActiveMail.mailType == MailType.Yellow && DeliveryManager.Instance != null)
                DeliveryManager.Instance.SetRandomDestination();
        }
    }
}
