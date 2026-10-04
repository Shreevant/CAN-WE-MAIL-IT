using UnityEngine;
public class SaveData : MonoBehaviour
{
    public static void SaveDay(int currentDay, int todayRevenue, int totalRevenue)
    {
        if (Application.isEditor) return;
        PlayerPrefs.SetInt("Day_Current", currentDay);
        PlayerPrefs.SetInt("Day_TodayRev", todayRevenue);
        PlayerPrefs.SetInt("Day_TotalRev", totalRevenue);
        PlayerPrefs.Save();
    }
    public static void LoadDay(out int currentDay, out int todayRevenue, out int totalRevenue)
    {
        currentDay = PlayerPrefs.GetInt("Day_Current", 1);
        todayRevenue = PlayerPrefs.GetInt("Day_TodayRev", 0);
        totalRevenue = PlayerPrefs.GetInt("Day_TotalRev", 0);
    }
}
