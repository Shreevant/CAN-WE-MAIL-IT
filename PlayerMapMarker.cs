using UnityEngine;

public class PlayerMapMarker : MonoBehaviour
{
    [Header("UI")]
    public RectTransform mapRect;
    public RectTransform markerRect;

    [Header("Orthographic Map Camera")]
    public Camera orthoMapCamera;

        [Header("Player")]
    public Transform playerTransform;

    [Header("Minimap")]
    public Camera miniCam;
    public RectTransform miniMapRect;
    public RectTransform miniPlayerRect;

    private void Update()
    {
        if (playerTransform == null || markerRect == null || mapRect == null || orthoMapCamera == null)
        {
            if (markerRect != null) markerRect.gameObject.SetActive(false);
            return;
        }

        markerRect.gameObject.SetActive(true);
        Vector3 vp = orthoMapCamera.WorldToViewportPoint(playerTransform.position);
        float w = mapRect.rect.width;
        float h = mapRect.rect.height;
        float x = (vp.x - 0.5f) * w;
        float y = (vp.y - 0.5f) * h;
                markerRect.anchoredPosition = new Vector2(x, y);

        // Minimark circle clamp
        UpdateMinimapPlayer();
    }

    private void UpdateMinimapPlayer()
    {
        if (playerTransform == null || miniPlayerRect == null || miniMapRect == null || miniCam == null)
        {
            if (miniPlayerRect != null) miniPlayerRect.gameObject.SetActive(false);
            return;
        }
        miniPlayerRect.gameObject.SetActive(true);
        Vector3 vp = miniCam.WorldToViewportPoint(playerTransform.position);
        float w = miniMapRect.rect.width;
        float h = miniMapRect.rect.height;
        float cx = (vp.x - 0.5f) * w;
        float cy = (vp.y - 0.5f) * h;

        if (vp.z > 0f && vp.x >= 0f && vp.x <= 1f && vp.y >= 0f && vp.y <= 1f)
        {
            miniPlayerRect.anchoredPosition = new Vector2(cx, cy);
            return;
        }

        Vector2 dir = new Vector2(cx, cy);
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
        dir.Normalize();
        float radius = Mathf.Min(w, h) * 0.5f;
        miniPlayerRect.anchoredPosition = dir * radius;
    }
}
