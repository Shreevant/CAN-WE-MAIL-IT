using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Buttons")]
    public Button newGameBtn;
    public Button continueBtn;
    public Button creditsBtn;
    public Button quitBtn;

    [Header("Credits Panel")]
    public GameObject creditsPanel;
    public GameObject menuPanel;

    private void Start()
    {
        if (newGameBtn != null) newGameBtn.onClick.AddListener(OnNewGame);
        if (continueBtn != null) continueBtn.onClick.AddListener(OnContinue);
        if (creditsBtn != null) creditsBtn.onClick.AddListener(OnCredits);
        if (quitBtn != null) quitBtn.onClick.AddListener(OnQuit);

        if (creditsPanel != null) creditsPanel.SetActive(false);

        if (continueBtn != null)
            continueBtn.interactable = PlayerPrefs.HasKey("Day_Current");
    }

    void OnNewGame()
    {
        PlayerPrefs.DeleteKey("Day_Current");
        PlayerPrefs.DeleteKey("Day_TodayRev");
        PlayerPrefs.DeleteKey("Day_TotalRev");
        PlayerPrefs.Save();
        StartCutscene();
    }

        void OnContinue()
    {
        if (!PlayerPrefs.HasKey("Day_Current")) return;
        int day = 1, todayRev = 0, totalRev = 0;
        SaveData.LoadDay(out day, out todayRev, out totalRev);
        if (day <= 1 && totalRev <= 0 && !PlayerPrefs.HasKey("Day_Current"))
        {
            OnNewGame();
            return;
        }
        SceneManager.LoadScene("city");
    }

    void OnCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);
    }

    void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        if (newGameBtn != null) newGameBtn.onClick.RemoveListener(OnNewGame);
        if (continueBtn != null) continueBtn.onClick.RemoveListener(OnContinue);
        if (creditsBtn != null) creditsBtn.onClick.RemoveListener(OnCredits);
        if (quitBtn != null) quitBtn.onClick.RemoveListener(OnQuit);
    }

    void StartCutscene()
    {
        if (newGameBtn != null) newGameBtn.interactable = false;
        if (continueBtn != null) continueBtn.interactable = false;
        CutscenePlayer.Instance?.PlayAndLoadCity();
    }
}
