using Codeqo.NativeMediaPlayer;
using Codeqo.NativeMediaPlayer.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonEvent
{
    public static bool GetDisableCondition(ButtonType buttonType)
    {
        switch (buttonType)
        {
            case ButtonType.PreviousButton:
                return !MediaPlayer.HasPreviousMediaItem();
            case ButtonType.NextButton:
                return !MediaPlayer.HasNextMediaItem();
            default: return true;
        }
    }

    public static void AddEvent(ButtonType buttonType, Image image, Sprite[] sprites, UnityEvent mediaEvent, UnityEvent onPauseEvent, UnityEvent onDestroyEvent, UnityAction toggleInteractable)
    {
        switch (buttonType)
        {
            case ButtonType.PlayPauseToggle:
                UnityAction isPlayingChangedTrue = () => { if (image != null && sprites[1] != null) image.sprite = sprites[1]; };
                UnityAction isPlayingChangedFalse = () => { if (image != null && sprites[0] != null) image.sprite = sprites[0]; };

                MediaEvents.OnIsPlayingChangedTrue.AddListener(isPlayingChangedTrue);
                MediaEvents.OnIsPlayingChangedFalse.AddListener(isPlayingChangedFalse);

                mediaEvent.AddListener(() =>
                {
                    if (MediaPlayer.isPlaying)
                    {
                        MediaPlayer.Pause();
                    }
                    else
                    {
                        MediaPlayer.Play();
                    }
                });

                onPauseEvent = new UnityEvent();
                onPauseEvent.AddListener(() => {
                    if (MediaPlayer.isPlaying)
                    {
                        image.sprite = sprites[1];
                    }
                    else
                    {
                        image.sprite = sprites[0];
                    }
                });

                onDestroyEvent.AddListener(() =>
                {
                    MediaEvents.OnIsPlayingChangedTrue.RemoveListener(isPlayingChangedTrue);
                    MediaEvents.OnIsBufferingChangedFalse.RemoveListener(isPlayingChangedFalse);
                });
                break;

            case ButtonType.PlayButton:
                mediaEvent.AddListener(MediaPlayer.Play);
                break;

            case ButtonType.PauseButton:
                mediaEvent.AddListener(MediaPlayer.Pause);
                break;

            case ButtonType.StopButton:
                mediaEvent.AddListener(MediaPlayer.Stop);
                break;

            case ButtonType.FastForwardButton:
                mediaEvent.AddListener(() => {
                    MediaPlayer.FastForward();
                    MediaEvents.OnIsBufferingChangedFalse.Invoke();
                });
                break;

            case ButtonType.RewindButton:
                mediaEvent.AddListener(() =>
                {
                    MediaPlayer.Rewind();
                    MediaEvents.OnIsBufferingChangedFalse.Invoke();
                });
                break;

            case ButtonType.NextButton:
                MediaEvents.OnPrepared.AddListener(toggleInteractable);
                RepeatMode.OnRepeatModeChanged(toggleInteractable);
                ShuffleMode.OnShuffleModeEnabledChanged(toggleInteractable);
                mediaEvent.AddListener(() =>
                {
                    MediaPlayer.Next();
                });
                onDestroyEvent.AddListener(() => {
                    MediaEvents.OnPrepared.RemoveListener(toggleInteractable);
                });
                break;

            case ButtonType.PreviousButton:
                MediaEvents.OnPrepared.AddListener(toggleInteractable);
                RepeatMode.OnRepeatModeChanged(toggleInteractable);
                ShuffleMode.OnShuffleModeEnabledChanged(toggleInteractable);
                mediaEvent.AddListener(() =>
                {
                    MediaPlayer.Previous();
                });
                onDestroyEvent.AddListener(() => {
                    MediaEvents.OnPrepared.RemoveListener(toggleInteractable);
                });
                break;

            case ButtonType.LoopOneAllToggle:
                UnityAction disabledEvent = () => {
                    if (image == null || sprites[0] == null) return;
                    image.sprite = sprites[0];
                    image.color = ButtonColor.Disabled.LightMode;
                };
                UnityAction repeatOneEvent = () => {
                    if (image == null || sprites[0] == null) return;
                    image.sprite = sprites[0];
                    image.color = ButtonColor.Enabled.LightMode;
                };
                UnityAction repeatAllEvent = () => {
                    if (image == null || sprites[1] == null) return;
                    image.sprite = sprites[1];
                    image.color = ButtonColor.Enabled.LightMode;
                };
                RepeatMode.DisabledEvent.AddListener(disabledEvent);
                RepeatMode.RepeatOneEvent.AddListener(repeatOneEvent);
                RepeatMode.RepeatAllEvent.AddListener(repeatAllEvent);
                mediaEvent.AddListener(() =>
                {
                    if (MediaPlayer.RepeatMode == RepeatMode.RepeatAll)
                    {
                        MediaPlayer.RepeatMode = RepeatMode.Disabled;
                    }
                    else if (MediaPlayer.RepeatMode == RepeatMode.Disabled)
                    {
                        MediaPlayer.RepeatMode = RepeatMode.RepeatOne;
                    }
                    else
                    {
                        MediaPlayer.RepeatMode = RepeatMode.RepeatAll;
                    }
                    RepeatMode.UpdateUI();
                });
                onPauseEvent.AddListener(() => {
                    RepeatMode.UpdateUI();
                });
                onDestroyEvent.AddListener(() => {
                    RepeatMode.DisabledEvent.RemoveListener(disabledEvent);
                    RepeatMode.RepeatOneEvent.RemoveListener(repeatOneEvent);
                    RepeatMode.RepeatAllEvent.RemoveListener(repeatAllEvent);
                });
                break;

            case ButtonType.LoopOneButton:
                disabledEvent = () => {
                    if (image != null) image.color = ButtonColor.Disabled.LightMode;
                };
                repeatOneEvent = () => {
                    if (image != null) image.color = ButtonColor.Enabled.LightMode;
                };
                repeatAllEvent = () => {
                    if (image != null) image.color = ButtonColor.Disabled.LightMode;
                };
                RepeatMode.DisabledEvent.AddListener(disabledEvent);
                RepeatMode.RepeatOneEvent.AddListener(repeatOneEvent);
                RepeatMode.RepeatAllEvent.AddListener(repeatAllEvent);
                mediaEvent.AddListener(() =>
                {
                    if (MediaPlayer.RepeatMode == RepeatMode.RepeatOne)
                    {
                        MediaPlayer.RepeatMode = RepeatMode.Disabled;
                    }
                    else
                    {
                        MediaPlayer.RepeatMode = RepeatMode.RepeatOne;
                    }
                    RepeatMode.UpdateUI();
                });
                onPauseEvent.AddListener(() => {
                    RepeatMode.UpdateUI();
                });
                onDestroyEvent.AddListener(() => {
                    RepeatMode.DisabledEvent.RemoveListener(disabledEvent);
                    RepeatMode.RepeatOneEvent.RemoveListener(repeatOneEvent);
                    RepeatMode.RepeatAllEvent.RemoveListener(repeatAllEvent);
                });
                break;

            case ButtonType.LoopAllButton:
                disabledEvent = () => {
                    if (image != null) image.color = ButtonColor.Disabled.LightMode;
                };
                repeatOneEvent = () => {
                    if (image != null) image.color = ButtonColor.Enabled.LightMode;
                };
                repeatAllEvent = () => {
                    if (image != null) image.color = ButtonColor.Disabled.LightMode;
                };
                RepeatMode.DisabledEvent.AddListener(disabledEvent);
                RepeatMode.RepeatOneEvent.AddListener(repeatOneEvent);
                RepeatMode.RepeatAllEvent.AddListener(repeatAllEvent);

                mediaEvent.AddListener(() =>
                {
                    if (MediaPlayer.RepeatMode == RepeatMode.RepeatAll)
                    {
                        MediaPlayer.RepeatMode = RepeatMode.Disabled;
                    }
                    else
                    {
                        MediaPlayer.RepeatMode = RepeatMode.RepeatAll;
                    }
                    RepeatMode.UpdateUI();
                });
                onPauseEvent.AddListener(() => {
                    RepeatMode.UpdateUI();
                });
                onDestroyEvent.AddListener(() => {
                    RepeatMode.DisabledEvent.RemoveListener(disabledEvent);
                    RepeatMode.RepeatOneEvent.RemoveListener(repeatOneEvent);
                    RepeatMode.RepeatAllEvent.RemoveListener(repeatAllEvent);
                });
                break;

            case ButtonType.ShuffleButton:
                UnityAction shuffleModeEnabledEvent = () => { if (image != null) image.color = ButtonColor.Enabled.LightMode; };
                UnityAction shuffleModeDisabledEvent = () => { if (image != null) image.color = ButtonColor.Disabled.LightMode; };
                ShuffleMode.OnShuffleModeEnabledEvent.AddListener(shuffleModeEnabledEvent);
                ShuffleMode.OnShuffleModeDisabledEvent.AddListener(shuffleModeDisabledEvent);
                mediaEvent.AddListener(() =>
                {
                    if (MediaPlayer.ShuffleModeEnabled)
                    {
                        MediaPlayer.ShuffleModeEnabled = false;
                    }
                    else
                    {
                        MediaPlayer.ShuffleModeEnabled = true;
                    }
                    ShuffleMode.UpdateUI();
                });
                onPauseEvent.AddListener(() => {
                    ShuffleMode.UpdateUI();
                });
                onDestroyEvent.AddListener(() => {
                    ShuffleMode.OnShuffleModeEnabledEvent.RemoveListener(shuffleModeEnabledEvent);
                    ShuffleMode.OnShuffleModeDisabledEvent.RemoveListener(shuffleModeDisabledEvent);
                });
                break;
        }
    }
}
