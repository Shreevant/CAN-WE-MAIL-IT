using UnityEngine;

public class LoadSaveOnStart : MonoBehaviour
{
    void Start()
    {
        if (Application.isEditor) return; // don't load saved data in Unity Editor
        int day = 1, todayRev = 0, totalRev = 0;
        SaveData.LoadDay(out day, out todayRev, out totalRev);

        if (GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.CurrentDay = day;
            GameLoopManager.Instance.HasWon = false;
        }

        if (RevenueManager.Instance != null)
        {
            RevenueManager.Instance.TotalRevenue = totalRev;
            RevenueManager.Instance.TodayRevenue = todayRev;
            RevenueManager.Instance.TriggerRevenueUpdated();
        }

        if (DayManager.Instance != null)
        {
            DayManager.Instance.StartDay();
        }

        if (MailManager.Instance != null)
        {
            MailManager.Instance.ActiveMail = null;
        }
    }
}

