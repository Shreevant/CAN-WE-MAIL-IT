using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class EndVideoPlayer : MonoBehaviour
{
    public static EndVideoPlayer Instance;
    public GameObject videoPanel;
    public VideoPlayer videoPlayer;
    public VideoClip winClip;
    public VideoClip loseClip;
    public AudioSource menuMusic;

    private bool isPlayingEnd = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (videoPanel != null) videoPanel.SetActive(false);
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    public void PlayEndVideo(bool win)
    {
        isPlayingEnd = true;
        if (videoPanel != null) videoPanel.SetActive(true);
        if (menuMusic != null) menuMusic.Stop();
        if (videoPlayer != null)
        {
            videoPlayer.clip = win ? winClip : loseClip;
#if UNITY_EDITOR
            Debug.Log("[EndVideo] Clip: " + (videoPlayer.clip ? videoPlayer.clip.name : "NULL"));
#endif
            videoPlayer.Play();
#if UNITY_EDITOR
            Debug.Log("[EndVideo] Playing: " + (win ? "WIN" : "LOSE"));
#endif
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
#if UNITY_EDITOR
        Debug.Log("[EndVideo] Finished");
#endif
        if (isPlayingEnd)
        {
            isPlayingEnd = false;
            if (videoPanel != null) videoPanel.SetActive(false);
#if UNITY_EDITOR
            Debug.Log("[EndVideo] Showing EndGameUI");
#endif
            EndGameUI.Instance?.ShowEndScreen();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("[EndVideo] Not playing end - ignoring");
#endif
        }
    }

    void Update()
    {
        if (isPlayingEnd && videoPlayer != null && videoPlayer.isPlaying == false && videoPlayer.time > 0)
        {
            OnVideoFinished(videoPlayer);
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null) videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
