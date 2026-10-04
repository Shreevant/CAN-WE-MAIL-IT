using UnityEngine;
using TMPro;

public class DaySummaryUI : MonoBehaviour
{
    public static DaySummaryUI Instance;
    public GameObject panel;
    public TMP_Text dayText;
    public TMP_Text revenueText;
    public TMP_Text lettersText;
    public TMP_Text loanRemainingText;

    private HouseTrigger houseTrigger;

    private void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); houseTrigger = FindFirstObjectByType<HouseTrigger>(); }

    private void OnEnable()
    {
        if (DayManager.Instance != null)
            DayManager.Instance.OnDayEnd += ShowSummary;
    }

    private void OnDisable()
    {
        if (DayManager.Instance != null)
            DayManager.Instance.OnDayEnd -= ShowSummary;
    }

    public void ShowSummary()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Day completed shown below
        if (revenueText != null && RevenueManager.Instance != null) revenueText.text = "Revenue: $" + RevenueManager.Instance.TodayRevenue;
        if (dayText != null) dayText.text = "Day " + (GameLoopManager.Instance != null ? GameLoopManager.Instance.CurrentDay : 1) + " Completed";
        if (lettersText != null) lettersText.text = "Letters Delivered: " + MailManager.TotalDelivered;
                if (loanRemainingText != null && RevenueManager.Instance != null) loanRemainingText.text = "Loan Remaining: $" + System.Math.Max(0, RevenueManager.Instance.LoanAmount - RevenueManager.Instance.TotalRevenue);
    }

    public void ConfirmAndContinue()
    {
        int d = (GameLoopManager.Instance != null) ? GameLoopManager.Instance.CurrentDay : 1;
        int t = (RevenueManager.Instance != null) ? RevenueManager.Instance.TodayRevenue : 0;
        int tot = (RevenueManager.Instance != null) ? RevenueManager.Instance.TotalRevenue : 0;
        SaveData.SaveDay(d, t, tot);
        CloseSummary();
                if (GameLoopManager.Instance != null && GameLoopManager.Instance.CurrentDay >= 3)
                {
                    int total = (RevenueManager.Instance != null) ? RevenueManager.Instance.TotalRevenue : 0;
                    int loan = (RevenueManager.Instance != null) ? RevenueManager.Instance.LoanAmount : 50;
                    bool win = total > loan;
                    EndVideoPlayer.Instance?.PlayEndVideo(win);
                }
                else if (GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.EndDay();
            DayNightSwitch.Instance?.SwitchToDay();
        }
        // Hide house until next timer ends
                if (houseTrigger != null) houseTrigger.SetActive(false);
    }

    public void CloseSummary()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
