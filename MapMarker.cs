using UnityEngine;

public class MapMarker : MonoBehaviour
{
    [Header("UI")]
    public RectTransform mapRect;
    public RectTransform markerRect;

    [Header("Orthographic Map Camera")]
    public Camera orthoMapCamera;

    [Header("Post Office Fallback")]
    public Transform postOfficeTarget;

        [Header("Home / End of Day")]
    public Transform homeTarget;

    [Header("Minimap")]
    public Camera miniCam;
    public RectTransform miniMapRect;
    public RectTransform miniLocationRect;

        private HouseTrigger houseTrigger;

    private void Awake()
    {
        houseTrigger = FindFirstObjectByType<HouseTrigger>();
    }

    private void Update()
    {
        Vector3 vp; float localX, localY;
        bool dayFinished = (DayManager.Instance != null && !DayManager.Instance.IsDayActive);

        if (dayFinished)
        {
            if (homeTarget == null)
            {
                                if (houseTrigger != null) homeTarget = houseTrigger.transform;
            }
            Transform target = homeTarget != null ? homeTarget : postOfficeTarget;

            if (target == null || markerRect == null || mapRect == null || orthoMapCamera == null)
            {
                if (markerRect != null) markerRect.gameObject.SetActive(false);
                return;
            }

            markerRect.gameObject.SetActive(true);
            vp = orthoMapCamera.WorldToViewportPoint(target.position);
            localX = Mathf.Lerp(mapRect.rect.xMin, mapRect.rect.xMax, vp.x);
            localY = Mathf.Lerp(mapRect.rect.yMin, mapRect.rect.yMax, vp.y);
            markerRect.anchoredPosition = new Vector2(localX, localY);

            // Minimark
            UpdateMinimapMarker(target);
            return;
        }

        DeliveryPoint dest = (DeliveryManager.Instance != null) ? DeliveryManager.Instance.CurrentDestination : null;
        Transform targetNormal = (dest != null) ? dest.transform : postOfficeTarget;

        if (targetNormal == null || markerRect == null || mapRect == null || orthoMapCamera == null)
        {
            if (markerRect != null) markerRect.gameObject.SetActive(false);
            return;
        }

        markerRect.gameObject.SetActive(true);
        vp = orthoMapCamera.WorldToViewportPoint(targetNormal.position);
        localX = Mathf.Lerp(mapRect.rect.xMin, mapRect.rect.xMax, vp.x);
        localY = Mathf.Lerp(mapRect.rect.yMin, mapRect.rect.yMax, vp.y);
        markerRect.anchoredPosition = new Vector2(localX, localY);

                // Minimark
        UpdateMinimapMarker(targetNormal);
    }

    private void UpdateMinimapMarker(Transform target)
    {
        if (target == null || miniLocationRect == null || miniMapRect == null || miniCam == null)
        {
            if (miniLocationRect != null) miniLocationRect.gameObject.SetActive(false);
            return;
        }

        miniLocationRect.gameObject.SetActive(true);
        Vector3 vp = miniCam.WorldToViewportPoint(target.position);
        float w = miniMapRect.rect.width;
        float h = miniMapRect.rect.height;

        // Centered mapped position (player-centered minimap)
        float cx = (vp.x - 0.5f) * w;
        float cy = (vp.y - 0.5f) * h;

        // If inside visible area, place exactly
        if (vp.z > 0f && vp.x >= 0f && vp.x <= 1f && vp.y >= 0f && vp.y <= 1f)
        {
            miniLocationRect.anchoredPosition = new Vector2(cx, cy);
            return;
        }

        // Outside: stick to edge in direction of target
        Vector2 dir = new Vector2(cx, cy);
        if (dir.sqrMagnitude < 0.0001f)
            dir = Vector2.right;
        dir.Normalize();

                float radius = Mathf.Min(w, h) * 0.5f;
        miniLocationRect.anchoredPosition = dir * radius;
    }
}




