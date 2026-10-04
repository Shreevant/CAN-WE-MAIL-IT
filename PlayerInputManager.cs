using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }

    public PlayerInputActions.PlayerActions Player =>
        InputActions.Player;

    public bool GameplayInputEnabled { get; private set; } = true;

    private void Awake()
    {
        InputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        InputActions.Player.Enable();
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
    }

    public void SetGameplayInputEnabled(bool enabled)
    {
        GameplayInputEnabled = enabled;
    }

    private void OnDestroy()
    {
        InputActions.Dispose();
    }
}
