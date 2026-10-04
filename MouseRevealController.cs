using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MouseRevealController : MonoBehaviour
{
    [Header("References")]
    public Image colorImage;

    [Header("Reveal Settings")]
    [Range(0.01f, 0.5f)]
    public float revealRadius = 0.15f;

    [Range(0f, 0.2f)]
    public float revealSoftness = 0.03f;

    private Material revealMaterial;

    private void Start()
    {
        if (colorImage == null)
        {
            colorImage = GetComponent<Image>();
        }

        revealMaterial = colorImage.material;

        if (revealMaterial == null)
        {
            Debug.LogError(
                "[MouseReveal] No material found on Color Image."
            );

            return;
        }

        UpdateRevealSettings();
    }

    private void Update()
    {
        if (revealMaterial == null)
            return;

        if (Mouse.current == null)
            return;

        // New Input System
        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        float x =
            mousePosition.x / Screen.width;

        float y =
            mousePosition.y / Screen.height;

        revealMaterial.SetVector(
            "_MousePosition",
            new Vector4(x, y, 0f, 0f)
        );

        revealMaterial.SetFloat(
            "_Radius",
            revealRadius
        );

        revealMaterial.SetFloat(
            "_Softness",
            revealSoftness
        );
    }

    private void UpdateRevealSettings()
    {
        if (revealMaterial == null)
            return;

        revealMaterial.SetFloat(
            "_Radius",
            revealRadius
        );

        revealMaterial.SetFloat(
            "_Softness",
            revealSoftness
        );
    }
}
