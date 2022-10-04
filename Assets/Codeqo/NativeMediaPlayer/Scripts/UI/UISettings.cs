using UnityEngine;
using Codeqo.NativeMediaPlayer.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] Color lightModeEnabledColor;
    [SerializeField] Color lightModeDisabledColor;
    [SerializeField] Color darkModeEnabledColor;
    [SerializeField] Color darkModeDisabledColor;

    private void Awake()
    {
        ButtonColor.Enabled.LightMode = lightModeEnabledColor;
        ButtonColor.Disabled.LightMode = lightModeDisabledColor;
        ButtonColor.Enabled.DarkMode = darkModeEnabledColor;
        ButtonColor.Disabled.DarkMode = darkModeDisabledColor;
    }
}

