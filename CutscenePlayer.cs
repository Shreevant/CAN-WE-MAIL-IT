using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{
    public static CutscenePlayer Instance;
    public GameObject videoPanel;      // Canvas / RawImage overlay (assign in Inspector)
    public VideoPlayer videoPlayer;
    public AudioSource menuMusic;

    void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    public void PlayAndLoadCity()
    {
        if (videoPanel != null) videoPanel.SetActive(true);
        if (menuMusic != null) menuMusic.Stop();
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
        // Hide menu buttons via listener, or just rely on MenuController disabling them
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("city");
    }

    void OnDestroy()
    {
        if (videoPlayer != null) videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
