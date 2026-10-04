using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingMenuManager : MonoBehaviour
{
    [Header("References")]
    public TemporaryBindingManager BindingManager;
    private PlayerInputManager playerInputManager;

        [Header("UI")]
    public GameObject journalPanel;   // root "Journal" object
    public GameObject bindingPage;  // child "Bindingmenu"
    public GameObject mapPage;      // child "map"

    [Header("Sound")]
    public AudioSource soundSource;

    [Header("Binding Rows")]
    public List<BindingRowUI> bindingRows =
        new List<BindingRowUI>();

    private bool menuOpen;
    private int currentPage = 0; // 0 = Binding, 1 = Map

    private readonly string[] actionNames =
    {
        "Move Forward",
        "Move Left",
        "Move Backward",
        "Move Right",
        "Jump",
        "Sprint",
        "Crouch"
    };

    private void Start()
    {
        // Find Binding Manager if it was not assigned.
        if (BindingManager == null)
        {
            BindingManager =
                FindFirstObjectByType<TemporaryBindingManager>();
        }

        // Get Player Input Manager from the same Player.
        if (BindingManager != null)
        {
            playerInputManager =
                BindingManager.GetComponent<PlayerInputManager>();
        }

                menuOpen = false;

        if (journalPanel != null)
            journalPanel.SetActive(false);

        // Make sure both pages start hidden until menu opens
        if (bindingPage != null) bindingPage.SetActive(false);
        if (mapPage != null) mapPage.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetupRows();
        RefreshMenu();
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            bool videoPlaying = (CutscenePlayer.Instance != null && CutscenePlayer.Instance.videoPanel != null && CutscenePlayer.Instance.videoPanel.activeSelf) || (EndVideoPlayer.Instance != null && EndVideoPlayer.Instance.videoPanel != null && EndVideoPlayer.Instance.videoPanel.activeSelf);
            if (!videoPlaying) ToggleMenu();
        }
    }

    private void SetupRows()
    {
        int count =
            Mathf.Min(
                actionNames.Length,
                bindingRows.Count
            );

        for (int i = 0; i < count; i++)
        {
            bindingRows[i].Setup(
                actionNames[i],
                this
            );
        }
    }

        public void ToggleMenu()
    {
        if (soundSource != null && soundSource.clip != null)
            soundSource.Play();
        if (menuOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    private void OpenMenu()
    {
        menuOpen = true;

        if (playerInputManager != null)
        {
            playerInputManager.SetGameplayInputEnabled(false);
        }

                journalPanel.SetActive(true);
        SwitchToPage(currentPage); // keep last page, or force 0 if you prefer

        Time.timeScale = 0f;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        RefreshMenu();
    }

    private void CloseMenu()
    {
        menuOpen = false;

        if (playerInputManager != null)
        {
            playerInputManager.SetGameplayInputEnabled(true);
        }

        journalPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    public void RefreshMenu()
    {
        foreach (BindingRowUI row in bindingRows)
        {
            if (row != null)
                row.Refresh();
        }
    }

        public void SwitchToPage(int index)
    {
        currentPage = Mathf.Clamp(index, 0, 1);
        if (bindingPage != null) bindingPage.SetActive(currentPage == 0);
        if (mapPage != null) mapPage.SetActive(currentPage == 1);
        if (currentPage == 0) RefreshMenu();
    }

    // Called by UI buttons to flip pages directly
    public void ShowBindingPage() => SwitchToPage(0);
    public void ShowMapPage() => SwitchToPage(1);

    // Called by a single "Cycle / Flip" button
    public void CyclePage()
    {
        SwitchToPage(currentPage == 0 ? 1 : 0);
    }

    private void OnDestroy()
    {
    }
}
