using UnityEngine;
using System;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager Instance;
    public int CurrentDay = 1;
    public int TotalDays = 3;
    public bool HasWon { get; set; }

    private void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    public void EndDay()
    {
        if (DayManager.Instance != null)
            RevenueManager.Instance?.ResetDayRevenue();
        CurrentDay++;
        if (CurrentDay > TotalDays)
        {
            HasWon = RevenueManager.Instance != null && RevenueManager.Instance.HasPaidLoan();
#if UNITY_EDITOR
            Debug.Log("[Loop] Day 3 ended. Won=" + HasWon);
#endif
        }
        else
        {
            if (DayManager.Instance != null) DayManager.Instance.StartDay();
            SaveData.SaveDay(CurrentDay, RevenueManager.Instance != null ? RevenueManager.Instance.TodayRevenue : 0, RevenueManager.Instance != null ? RevenueManager.Instance.TotalRevenue : 0);
        }
    }

    public void Restart()
    {
        CurrentDay = 1; HasWon = false;
        if (DayManager.Instance != null) DayManager.Instance.StartDay();
        if (RevenueManager.Instance != null) RevenueManager.Instance.ResetDayRevenue();
        if (MailManager.Instance != null) MailManager.Instance.ResetTodayDelivered();
        if (MailManager.Instance != null) MailManager.Instance.ActiveMail = null;
        if (MailManager.Instance != null) MailManager.Instance.ResetTodayDelivered();
    }
}
