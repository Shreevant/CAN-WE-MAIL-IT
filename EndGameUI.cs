using UnityEngine;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    public static EndGameUI Instance;
    public GameObject panel;
    public TMP_Text resultText;
    public TMP_Text totalText;
    public TMP_Text loanText;

    [Header("Scene")]
    public string menuSceneName = "menu";

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (panel != null) panel.SetActive(false);
        if (GameLoopManager.Instance != null) GameLoopManager.Instance.Restart();
        UnityEngine.SceneManagement.SceneManager.LoadScene("city");
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }

    private void Awake() 
    { 
        if (Instance == null) Instance = this; 
        else Destroy(gameObject); 
        if (panel != null) panel.SetActive(false);
    }

    public void ShowEndScreen()
    {
        if (panel != null) panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int total = (RevenueManager.Instance != null) ? RevenueManager.Instance.TotalRevenue : 0;
        int loan = (RevenueManager.Instance != null) ? RevenueManager.Instance.LoanAmount : 50;

        if (totalText != null) totalText.text = "Total Revenue: $" + total;
        if (loanText != null) loanText.text = "Loan Needed: $" + loan;
        if (resultText != null)
            resultText.text = (total > loan) ? "GAME COMPLETE" : "FAILED";
    }
}
