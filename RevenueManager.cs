using UnityEngine;
using System;

public class RevenueManager : MonoBehaviour
{
    public static RevenueManager Instance;
        public int TodayRevenue { get; set; }
    public int TotalRevenue { get; set; }
    public int LoanAmount = 50;
    public event Action OnRevenueUpdated;

    private void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    public void AddRevenue(int amount)
    {
        TodayRevenue += amount;
        TotalRevenue += amount;
        OnRevenueUpdated?.Invoke();
    }

    public void TriggerRevenueUpdated() => OnRevenueUpdated?.Invoke();

    public bool HasPaidLoan() => TotalRevenue >= LoanAmount;

    public void ResetDayRevenue() { TodayRevenue = 0; OnRevenueUpdated?.Invoke(); }
}
