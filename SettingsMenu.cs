using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu Instance;

    [Header("UI")]
    public GameObject settingsPanel;
    public Button backBtn;
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    [Header("Music")]
    public AudioSource menuMusic;

    [Header("Target")]
    public PlayerLook playerLook;

    [Header("Sensitivity Range")]
    public float sensitivityMin = 50f;
    public float sensitivityMax = 300f;

    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (backBtn != null) backBtn.onClick.AddListener(OnBack);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = sensitivityMin;
            sensitivitySlider.maxValue = sensitivityMax;
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
            if (playerLook != null)
                sensitivitySlider.value = Mathf.Clamp(playerLook.sensitivity, sensitivityMin, sensitivityMax);
        }

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            if (menuMusic != null)
                volumeSlider.value = menuMusic.volume;
        }
    }

    public void Open()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (PauseMenu.Instance != null && PauseMenu.Instance.pausePanel != null)
            PauseMenu.Instance.pausePanel.SetActive(false);
    }

    void OnBack()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (PauseMenu.Instance != null && PauseMenu.Instance.pausePanel != null)
            PauseMenu.Instance.pausePanel.SetActive(true);
    }

    void OnSensitivityChanged(float value)
    {
        if (playerLook != null) playerLook.sensitivity = value;
    }

    void OnVolumeChanged(float value)
    {
        if (menuMusic != null) menuMusic.volume = value;
    }

    void OnDestroy()
    {
        if (backBtn != null) backBtn.onClick.RemoveListener(OnBack);
        if (sensitivitySlider != null) sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
        if (volumeSlider != null) volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }
}
