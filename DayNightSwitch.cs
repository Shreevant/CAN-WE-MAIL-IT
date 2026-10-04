using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DayNightSwitch : MonoBehaviour
{
    public static DayNightSwitch Instance;

    [Header("Skybox Materials")]
    public Material daySkybox;
    public Material nightSkybox;

    [Header("Post-Process (Stack v2)")]
    public PostProcessVolume postVolume;
    public PostProcessProfile dayProfile;
    public PostProcessProfile nightProfile;

    private void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    private void Start()
    {
        SwitchToDay();
        if (DayManager.Instance != null)
            DayManager.Instance.OnDayEnd += SwitchToNight;
    }

    void SwitchToNight()
    {
        if (nightSkybox != null)
        {
            RenderSettings.skybox = nightSkybox;
            DynamicGI.UpdateEnvironment();
        }

        if (postVolume != null && nightProfile != null)
            postVolume.profile = nightProfile;
    }

    public void SwitchToDay()
    {
        if (daySkybox != null)
        {
            RenderSettings.skybox = daySkybox;
            DynamicGI.UpdateEnvironment();
        }
        if (postVolume != null && dayProfile != null)
            postVolume.profile = dayProfile;
    }

    private void OnDestroy()
    {
        if (DayManager.Instance != null)
            DayManager.Instance.OnDayEnd -= SwitchToNight;
    }
}
