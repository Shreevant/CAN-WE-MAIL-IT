using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStamina : MonoBehaviour
{
    public static PlayerStamina Instance;
    public float current = 100f;
    public float max = 100f;
    public float drainRate = 20f;
    public float replenishRate = 15f;
    private TemporaryBindingManager bindingMgr;

    void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); bindingMgr = FindFirstObjectByType<TemporaryBindingManager>(); }

    void Update()
    {
        bool holdingSprint = false;
        if (bindingMgr != null)
        {
            UnityEngine.InputSystem.Key sprintKey = bindingMgr.GetCurrentKey("Sprint");
            if (sprintKey != UnityEngine.InputSystem.Key.None && Keyboard.current != null)
                holdingSprint = Keyboard.current[sprintKey].isPressed;
        }
        else if (Keyboard.current != null)
        {
            // Fallback when binding manager is not available
            holdingSprint = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        }

        if (holdingSprint)
        {
            if (current > 0f)
                current -= drainRate * Time.deltaTime;
        }
        else
        {
            current += replenishRate * Time.deltaTime;
        }

        current = Mathf.Clamp(current, 0f, max);
    }

    public bool CanSprint() => current > 0f;
}
