using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class SaturationCycler : MonoBehaviour
{
    [Header("Profiles")]
    public PostProcessProfile dayProfile;
    public PostProcessProfile nightProfile;

    [Header("Day Saturation")]
    public float dayMin = -100f;
    public float dayMax = 75f;

    [Header("Night Saturation")]
    public float nightMin = -100f;
    public float nightMax = 15f;

    [Header("Timing")]
    public float transitionDur = 2f;
    public float holdAtMin = 3f; // B&W hold
    public float holdAtMax = 3f; // Color hold

    private void Start()
    {
        if (dayProfile != null)
        {
            SetSat(dayProfile, dayMax);
            StartCoroutine(CycleProfile(dayProfile, dayMin, dayMax));
        }
        if (nightProfile != null)
        {
            SetSat(nightProfile, nightMax);
            StartCoroutine(CycleProfile(nightProfile, nightMin, nightMax));
        }
    }

    private void SetSat(PostProcessProfile profile, float value)
    {
        ColorGrading grading = profile.GetSetting<ColorGrading>();
        if (grading != null) grading.saturation.value = value;
    }

    private IEnumerator CycleProfile(PostProcessProfile profile, float minSat, float maxSat)
    {
        while (true)
        {
            yield return new WaitForSeconds(holdAtMax);   // start colored, hold
            yield return Animate(profile, maxSat, minSat, transitionDur); // to B&W
            yield return new WaitForSeconds(holdAtMin);   // hold B&W
            yield return Animate(profile, minSat, maxSat, transitionDur); // back to color
        }
    }

    private IEnumerator Animate(PostProcessProfile profile, float from, float to, float duration)
    {
        ColorGrading grading = profile.GetSetting<ColorGrading>();
        if (grading == null) yield break;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            grading.saturation.value = Mathf.Lerp(from, to, t);
            yield return null;
        }
        grading.saturation.value = to;
    }
}
