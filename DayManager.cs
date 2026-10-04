using UnityEngine;
using System;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;
    [Header("Config")]
    public float dayDurationSeconds = 600f; // 10 minutes default
    public float redMailSpeedMultiplier = 2.0f; // faster drain when Red picked
    private float timer;
    public bool IsDayActive { get; private set; }
    public event Action OnDayEnd;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start() { timer = dayDurationSeconds; IsDayActive = true; }

    private void Update()
    {
        if (!IsDayActive) return;
        float mult = (MailManager.Instance != null && MailManager.Instance.ActiveMail != null && MailManager.Instance.ActiveMail.mailType == MailType.Red) ? redMailSpeedMultiplier : 1f;
        timer -= Time.deltaTime * mult;
        if (timer <= 0f)
        {
            timer = 0f;
            IsDayActive = false;
            if (MailManager.Instance != null) MailManager.Instance.CancelActiveMail();
            OnDayEnd?.Invoke();
        }
    }

    public void StartDay() { timer = dayDurationSeconds; IsDayActive = true; }
    public float GetRemainingSeconds() => timer;
}
