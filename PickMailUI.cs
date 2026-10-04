using UnityEngine;
using UnityEngine.UI;

public class PickMailUI : MonoBehaviour
{
    public static PickMailUI Instance;
    public GameObject panel;

    public void OpenSelection()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerInputManager != null)
            playerInputManager.SetGameplayInputEnabled(false);
    }

    public void CloseSelection()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerInputManager != null)
            playerInputManager.SetGameplayInputEnabled(true);
    }

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (panel != null) panel.SetActive(false);
        // Optional: find player input manager if not assigned
        var pm = FindFirstObjectByType<PlayerInputManager>();
        if (pm != null) playerInputManager = pm;
    }

    public void PickBlue() => SelectMailType(MailType.Blue);
    public void PickRed() => SelectMailType(MailType.Red);
    public void PickYellow() => SelectMailType(MailType.Yellow);

    private void SelectMailType(MailType type)
    {
        if (MailManager.Instance != null)
            MailManager.Instance.SelectMail(type);
        CloseSelection();
    }
}
