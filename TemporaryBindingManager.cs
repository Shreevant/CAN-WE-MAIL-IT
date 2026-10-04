using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TemporaryBindingManager : MonoBehaviour
{
    [Serializable]
    public class BindingData
    {
        public string actionName;

        // The original key for this action.
        public Key originalKey;

        // The key currently assigned to this action.
        public Key currentKey;

        // (binding-lock removed)
    }

    [Header("Binding Data")]
    [SerializeField]
    private List<BindingData> bindings = new List<BindingData>();

        private PlayerInputManager inputManager;
        private System.Collections.IEnumerator timerCoroutine;

        [Header("Timer Tuning")]
        [SerializeField] private float shuffleInterval = 30f;

    // The ONLY keys allowed in the game.
    private readonly Key[] allowedKeys =
    {
        Key.W,
        Key.A,
        Key.S,
        Key.D,
        Key.Space,
        Key.LeftShift,
        Key.C
    };

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();

        CreateDefaultBindings();
    }

    private void Update()
    {
        // TEMPORARY TEST CONTROL
        // T randomizes the current bindings.
        //
        // There is intentionally NO timer yet.

        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            RandomizeBindings();
        }

    }

    // =========================================================
    // DEFAULT BINDINGS
    // =========================================================

    private void CreateDefaultBindings()
    {
        bindings.Clear();

        bindings.Add(new BindingData
        {
            actionName = "Move Forward",
            originalKey = Key.W,
            currentKey = Key.W,
        });

        bindings.Add(new BindingData
        {
            actionName = "Move Left",
            originalKey = Key.A,
            currentKey = Key.A,
        });

        bindings.Add(new BindingData
        {
            actionName = "Move Backward",
            originalKey = Key.S,
            currentKey = Key.S,
        });

        bindings.Add(new BindingData
        {
            actionName = "Move Right",
            originalKey = Key.D,
            currentKey = Key.D,
        });

        bindings.Add(new BindingData
        {
            actionName = "Jump",
            originalKey = Key.Space,
            currentKey = Key.Space,
        });

        bindings.Add(new BindingData
        {
            actionName = "Sprint",
            originalKey = Key.LeftShift,
            currentKey = Key.LeftShift,
        });

        bindings.Add(new BindingData
        {
            actionName = "Crouch",
            originalKey = Key.C,
            currentKey = Key.C,
        });
    }

    // =========================================================
    // RANDOMIZATION
    // =========================================================

    public void RandomizeBindings()
    {
        List<Key> availableKeys =
            new List<Key>(allowedKeys);

        // -----------------------------------------------------
        // Shuffle all allowed keys (no locks; full randomization).
        // -----------------------------------------------------

        Shuffle(availableKeys);

        int keyIndex = 0;

        foreach (BindingData binding in bindings)
        {
            binding.currentKey = availableKeys[keyIndex];

            keyIndex++;
        }

        ApplyAllBindings();

#if UNITY_EDITOR
        Debug.Log("[Binding] ===== RANDOMIZED =====");
#endif

        PrintBindings();
    }

    private void Shuffle(List<Key> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex =
                UnityEngine.Random.Range(0, i + 1);

            Key temp = list[i];

            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // =========================================================
    // APPLY BINDINGS TO INPUT SYSTEM
    // =========================================================

    private void ApplyAllBindings()
    {
        ApplyMovementBindings();

        ApplyButtonBinding(
            "Jump",
            GetBinding("Jump").currentKey
        );

        ApplyButtonBinding(
            "Sprint",
            GetBinding("Sprint").currentKey
        );

        ApplyButtonBinding(
            "Crouch",
            GetBinding("Crouch").currentKey
        );
    }

    private void ApplyMovementBindings()
    {
        InputAction moveAction =
            inputManager.Player.Move;

        for (int i = 0;
             i < moveAction.bindings.Count;
             i++)
        {
            InputBinding binding =
                moveAction.bindings[i];

            if (!binding.isPartOfComposite)
                continue;

            switch (binding.name)
            {
                case "up":

                    SetBindingPath(
                        moveAction,
                        i,
                        GetBinding("Move Forward").currentKey
                    );

                    break;

                case "down":

                    SetBindingPath(
                        moveAction,
                        i,
                        GetBinding("Move Backward").currentKey
                    );

                    break;

                case "left":

                    SetBindingPath(
                        moveAction,
                        i,
                        GetBinding("Move Left").currentKey
                    );

                    break;

                case "right":

                    SetBindingPath(
                        moveAction,
                        i,
                        GetBinding("Move Right").currentKey
                    );

                    break;
            }
        }
    }

    private void ApplyButtonBinding(
        string actionName,
        Key key)
    {
        InputAction action =
            GetAction(actionName);

        if (action == null)
            return;

        if (action.bindings.Count == 0)
            return;

        action.ApplyBindingOverride(
            0,
            $"<Keyboard>/{GetKeyName(key)}"
        );
    }

    private void SetBindingPath(
        InputAction action,
        int bindingIndex,
        Key key)
    {
        action.ApplyBindingOverride(
            bindingIndex,
            $"<Keyboard>/{GetKeyName(key)}"
        );
    }

    // =========================================================
    // ACTION LOOKUP
    // =========================================================

    private InputAction GetAction(
        string actionName)
    {
        switch (actionName)
        {
            case "Jump":
                return inputManager.Player.Jump;

            case "Sprint":
                return inputManager.Player.Sprint;

            case "Crouch":
                return inputManager.Player.Crouch;

            default:
                return null;
        }
    }

    // =========================================================
    // GETTERS
    // =========================================================

    public IReadOnlyList<BindingData>
        GetBindings()
    {
        return bindings;
    }

    public BindingData
        GetBinding(string actionName)
    {
        return bindings.Find(
            binding =>
                binding.actionName == actionName
        );
    }

    public Key
        GetCurrentKey(string actionName)
    {
        BindingData binding =
            GetBinding(actionName);

        if (binding == null)
            return Key.None;

        return binding.currentKey;
    }

    public Key
        GetOriginalKey(string actionName)
    {
        BindingData binding =
            GetBinding(actionName);

        if (binding == null)
            return Key.None;

        return binding.originalKey;
    }

    // =========================================================
    // DEBUG
    // =========================================================

    private void PrintBindings()
    {
        foreach (BindingData binding in bindings)
        {
            string lockText =
                false
                    ? " [LOCKED]"
                    : "";

            #if UNITY_EDITOR
            Debug.Log($"{binding.actionName} → {binding.currentKey}" + lockText);
            #endif
        }
    }

    private BindingData GetBindingUsingKey(Key key)
    {
        return bindings.Find(
            binding => binding.currentKey == key
        );
    }



    // =========================================================
    // TIMED RANDOMIZATION (Blue Mail)
    // =========================================================
        public void StartRandomTimer()
    {
        StopRandomTimer();
        timerCoroutine = TimerLoop(shuffleInterval);
        StartCoroutine(timerCoroutine);
    }

    public void StopRandomTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

        private System.Collections.IEnumerator TimerLoop(float interval)
    {
        while (true)
        {
            RandomizeBindings(); // shuffle immediately on pickup
            yield return new WaitForSeconds(interval);
        }
    }

    // =========================================================
    // RESET (for Blue Mail delivery / end of day)
    // =========================================================
        public void ResetToDefaults()
    {
        StopRandomTimer();
        foreach (BindingData binding in bindings)
        {
            binding.currentKey = binding.originalKey;
        }
        ApplyAllBindings();
#if UNITY_EDITOR
        Debug.Log("[Binding] ===== RESET TO DEFAULTS =====");
#endif
        PrintBindings();
    }

    // =========================================================
    // KEY  INPUT SYSTEM PATH
    // =========================================================

    private string GetKeyName(Key key)
    {
        switch (key)
        {
            case Key.W:
                return "w";

            case Key.A:
                return "a";

            case Key.S:
                return "s";

            case Key.D:
                return "d";

            case Key.Space:
                return "space";

            case Key.LeftShift:
                return "leftShift";

            case Key.C:
                return "c";

            default:
                return "";
        }
    }
}
