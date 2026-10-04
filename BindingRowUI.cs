using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BindingRowUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text actionNameText;
    public TMP_Text originalBindingText;
    public TMP_Text currentBindingText;
    private string actionName;

    private BindingMenuManager menuManager;

    public void Setup(
        string action,
        BindingMenuManager manager)
    {
        actionName = action;
        menuManager = manager;

        actionNameText.text = actionName;

        Refresh();
    }

    public void Refresh()
    {
        if (menuManager == null)
            return;

        TemporaryBindingManager bindingManager =
            menuManager.BindingManager;

        TemporaryBindingManager.BindingData binding =
            bindingManager.GetBinding(actionName);

        if (binding == null)
            return;

        originalBindingText.text =
            FormatKey(binding.originalKey);

        currentBindingText.text =
            FormatKey(binding.currentKey);


    }

    private string FormatKey(
        UnityEngine.InputSystem.Key key)
    {
        switch (key)
        {
            case UnityEngine.InputSystem.Key.W:
                return "W";

            case UnityEngine.InputSystem.Key.A:
                return "A";

            case UnityEngine.InputSystem.Key.S:
                return "S";

            case UnityEngine.InputSystem.Key.D:
                return "D";

            case UnityEngine.InputSystem.Key.Space:
                return "SPACE";

            case UnityEngine.InputSystem.Key.LeftShift:
                return "LEFT SHIFT";

            case UnityEngine.InputSystem.Key.C:
                return "C";

            default:
                return key.ToString().ToUpper();
        }
    }
}
