using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject pausePanel;
    public Button menuBtn;
    public Button resumeBtn;
    public Button restartBtn;
    public Button settingsBtn;

    private bool isPaused = false;

    void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (menuBtn != null) menuBtn.onClick.AddListener(BackToMenu);
        if (resumeBtn != null) resumeBtn.onClick.AddListener(Resume);
        if (restartBtn != null) restartBtn.onClick.AddListener(Restart);
        if (settingsBtn != null) settingsBtn.onClick.AddListener(OpenSettings);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

        void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool videoPlaying = (CutscenePlayer.Instance != null && CutscenePlayer.Instance.videoPanel != null && CutscenePlayer.Instance.videoPanel.activeSelf) || (EndVideoPlayer.Instance != null && EndVideoPlayer.Instance.videoPanel != null && EndVideoPlayer.Instance.videoPanel.activeSelf);
            if (videoPlaying || AnyOtherPanelActive()) return;
            TogglePause();
        }
    }

        bool AnyOtherPanelActive()
    {
        if (DaySummaryUI.Instance != null && DaySummaryUI.Instance.panel != null && DaySummaryUI.Instance.panel.activeSelf) return true;
        if (EndGameUI.Instance != null && EndGameUI.Instance.panel != null && EndGameUI.Instance.panel.activeSelf) return true;
        if (EndVideoPlayer.Instance != null && EndVideoPlayer.Instance.videoPanel != null && EndVideoPlayer.Instance.videoPanel.activeSelf) return true;
        if (PickMailUI.Instance != null && PickMailUI.Instance.panel != null && PickMailUI.Instance.panel.activeSelf) return true;
        if (SettingsMenu.Instance != null && SettingsMenu.Instance.settingsPanel != null && SettingsMenu.Instance.settingsPanel.activeSelf) return true;
        return false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (pausePanel != null) pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        if (isPaused) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        else { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    }

    void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }

    public void OpenSettings()
    {
        if (SettingsMenu.Instance != null) SettingsMenu.Instance.Open();
    }

    void Resume()
    {
        TogglePause();
    }

    void Restart()
    {
        Time.timeScale = 1f;
        if (GameLoopManager.Instance != null) GameLoopManager.Instance.Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        if (menuBtn != null) menuBtn.onClick.RemoveListener(BackToMenu);
        if (resumeBtn != null) resumeBtn.onClick.RemoveListener(Resume);
        if (restartBtn != null) restartBtn.onClick.RemoveListener(Restart);
        if (settingsBtn != null) settingsBtn.onClick.RemoveListener(OpenSettings);
    }
}
