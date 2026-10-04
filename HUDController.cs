using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("UI References")]
    [Header("Mail Sprite References")]
    public UnityEngine.UI.Image mailIcon;
    public Sprite blueMailSprite;
    public Sprite redMailSprite;
    public Sprite yellowMailSprite;
    public Sprite noMailSprite;
    public TMP_Text dayText;
    public TMP_Text timeText;
    public TMP_Text deliveredText;
    [Header("Stamina")]
    public Slider staminaSlider;

    private float dayStartTime = 8f; // 8:00
    private float dayEndTime = 17f;  // 17:00 (5 PM)
    private float duration = 600f;

    private void Update()
    {
        UpdateMailStatus();
        UpdateDay();
        UpdateTime();
    }

    void UpdateMailStatus()
    {
        if (mailIcon == null) return;
        if (MailManager.Instance != null && MailManager.Instance.ActiveMail != null)
        {
            mailIcon.enabled = true;
            switch (MailManager.Instance.ActiveMail.mailType)
            {
                case MailType.Blue: mailIcon.sprite = blueMailSprite; break;
                case MailType.Red: mailIcon.sprite = redMailSprite; break;
                case MailType.Yellow: mailIcon.sprite = yellowMailSprite; break;
                default: mailIcon.sprite = noMailSprite; break;
            }
        }
        else
        {
            mailIcon.enabled = true;
            mailIcon.sprite = noMailSprite;
        }
    }

    Color GetMailColor(MailType type)
    {
        switch (type)
        {
            case MailType.Blue: return Color.blue;
            case MailType.Red: return Color.red;
            case MailType.Yellow: return Color.yellow;
            default: return Color.white;
        }
    }

        void UpdateDay()
    {
        if (dayText == null) return;
        bool dayOver = (DayManager.Instance != null && !DayManager.Instance.IsDayActive);
        if (dayOver)
            dayText.text = "Day Over";
        else if (GameLoopManager.Instance != null)
            dayText.text = "Day " + GameLoopManager.Instance.CurrentDay;
        else
            dayText.text = "Day 1";
    }

        void UpdateTime()
    {
        if (timeText == null) return;
        float timer = (DayManager.Instance != null) ? DayManager.Instance.GetRemainingSeconds() : 600f;
        float duration = (DayManager.Instance != null) ? DayManager.Instance.dayDurationSeconds : 600f;
        if (duration <= 0f) duration = 600f;
        float progress = Mathf.Clamp01(1f - timer / duration);
        float hourFloat = dayStartTime + progress * (dayEndTime - dayStartTime);
        int hour = Mathf.FloorToInt(hourFloat) % 24;
        int minute = Mathf.FloorToInt((hourFloat - Mathf.Floor(hourFloat)) * 60f);
        timeText.text = string.Format("{0:D2}:{1:D2}", hour, minute);       
                bool redActive = (MailManager.Instance != null && MailManager.Instance.ActiveMail != null && MailManager.Instance.ActiveMail.mailType == MailType.Red);
        timeText.color = redActive ? Color.red : Color.yellow;

        if (deliveredText != null && MailManager.Instance != null)
            deliveredText.text = "Delivered: " + MailManager.TodayDelivered;

        if (PlayerStamina.Instance != null)
        {
                        if (staminaSlider != null) staminaSlider.value = PlayerStamina.Instance.current / PlayerStamina.Instance.max;
        }
    }
}




