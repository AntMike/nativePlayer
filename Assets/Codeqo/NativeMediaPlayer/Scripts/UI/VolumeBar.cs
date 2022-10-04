using UnityEngine;
using UnityEngine.UI;
using Codeqo.NativeMediaPlayer;
using UnityEngine.Events;

public class VolumeBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] bool _mute;
    public bool mute { get { return _mute; } set { Toggle(); } }
    private UnityAction onInitAction;

    void OnEnable()
    {
        if (slider == null) return;

        onInitAction = () => {
            MediaEvents.OnPrepared.AddListener(() => { slider.value = MediaPlayer.Volume; });
            slider.onValueChanged.AddListener(SetVolume);
            slider.value = MediaPlayer.Volume;
            if (_mute) Toggle();
        };

        MediaEventListener.AddOnInitListener(onInitAction);
    }

    void OnDestroy()
    {
        MediaEvents.OnInit.RemoveListener(onInitAction);
    }

    private void SetVolume(float value)
    {
        MediaPlayer.Volume = value;
    }

    void Toggle()
    {
        if (_mute)
        {
            slider.value = 1;
            slider.interactable = true;
            Debug.Log("Audio Unmuted");
        }
        else
        {
            slider.value = 0;
            slider.interactable = false;
            Debug.Log("Audio Muted");
        }
    }
}

