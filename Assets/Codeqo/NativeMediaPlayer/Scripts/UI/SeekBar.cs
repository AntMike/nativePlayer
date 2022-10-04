using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Codeqo.NativeMediaPlayer;
using UnityEngine.Events;

public class SeekBar : MonoBehaviour
{
    enum TimeFormat : int
    {
        Sec = 0,
        [InspectorName("Min:Sec (Default)")]
        MinSec = 1,
        [InspectorName("Hour:Min:Sec")]
        HourMinSec = 2
    }

    [SerializeField] Slider slider;
    [SerializeField] float updateInterval = 0.5f;
    [SerializeField] Text currentPositionText;
    [SerializeField] Text durationText;
    [SerializeField] TimeFormat timeFormat = TimeFormat.MinSec;
    [SerializeField] bool showMillisec;
    private bool seekBarUpdateRunning;
    private bool ignoreValueChange;
    private UnityAction onInitAction;

    void Awake()
    {
        if (slider == null) return;

        onInitAction = () => {
            if (slider == null) return;
            slider.value = 0;
            slider.onValueChanged.AddListener(SetPosition);
            MediaEvents.OnIsPlayingChangedTrue.AddListener(StartUpdatingSeekBar);
            MediaEvents.OnIsPlayingChangedFalse.AddListener(StopUpdatingSeekBar);
            MediaEvents.OnPrepared.AddListener(UpdateSeekBarDuration);
            MediaEvents.OnPrepared.AddListener(UpdateSeekBarCurrentPosition);
            MediaEvents.OnPrepared.AddListener(UpdateSeekBarSliderValue);
        };

        MediaEventListener.AddOnInitListener(onInitAction);
    }

    void OnDestroy()
    {
        MediaEvents.OnInit.RemoveListener(onInitAction);
    }

    private void SetPosition(float value)
    {
        if (!ignoreValueChange)
        {
            seekBarUpdateRunning = false;
            MediaPlayer.SeekTo((float)value * MediaPlayer.GetDuration());
            UpdateSeekBarCurrentPosition();
            StartUpdatingSeekBar();
        }
    }

    public void StartUpdatingSeekBar()
    {
        StartCoroutine(StartUpdatingSeekBar(.2f));
    }

    private IEnumerator StartUpdatingSeekBar(float delay)
    {
        yield return new WaitForSeconds(delay);
        seekBarUpdateRunning = true;
        StartCoroutine(SeekBarUpdateEvent());
    }

    private void StopUpdatingSeekBar()
    {
        seekBarUpdateRunning = false;
    }

    private IEnumerator SeekBarUpdateEvent()
    {
        while (seekBarUpdateRunning)
        {
            ignoreValueChange = true;
            UpdateSeekBarCurrentPosition();
            UpdateSeekBarSliderValue();
            UpdateSeekBarDuration();
            ignoreValueChange = false;
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void UpdateSeekBarCurrentPosition()
    {
        if (MediaPlayer.GetDuration() > 0)
        {
            if (MediaPlayer.isPlaying)
            {
                TimeSpan position = TimeSpan.FromSeconds(MediaPlayer.GetCurrentPosition());
                currentPositionText.text = GetTimeInFormat(position);
            }
            else
            {
                TimeSpan position = TimeSpan.FromSeconds(MediaPlayer.GetDuration() * slider.value);
                currentPositionText.text = GetTimeInFormat(position);
            }
        }
    }

    private void UpdateSeekBarSliderValue()
    {
        if (MediaPlayer.GetDuration() > 0)
        {
            slider.value = MediaPlayer.GetCurrentPosition() / MediaPlayer.GetDuration();
        }
    }

    private void UpdateSeekBarDuration()
    {
        if (MediaPlayer.GetDuration() > 0)
        {
            slider.interactable = true;
            TimeSpan duration = TimeSpan.FromSeconds(MediaPlayer.GetDuration());
            durationText.text = GetTimeInFormat(duration);
        }
        else
        {
            slider.interactable = false;
            durationText.text = GetTimeInFormat(new TimeSpan());
        }
    }

    private string GetTimeInFormat(TimeSpan timeSpan)
    {
        string t;

        if (timeSpan != null)
        {
            switch (timeFormat)
            {
                case TimeFormat.Sec: t = string.Format("{0:0}", Mathf.FloorToInt((float)timeSpan.TotalSeconds)); break;
                case TimeFormat.HourMinSec: t = string.Format("{0:0}:{1:00}:{2:00}", Mathf.FloorToInt((float)timeSpan.TotalHours), timeSpan.Minutes, timeSpan.Seconds); break;
                default: t = string.Format("{0:0}:{1:00}", Mathf.FloorToInt((float)timeSpan.TotalMinutes), timeSpan.Seconds); break;
            }

            if (showMillisec) t += string.Format(":{0:000}", timeSpan.Milliseconds);
            return t;
        }
        else
        {
            switch (timeFormat)
            {
                case TimeFormat.Sec: t = "0"; break;
                case TimeFormat.HourMinSec: t = "0:00:00"; break;
                default: t = "0:00"; break;
            }

            if (showMillisec) t += ":000";
            return t;
        }
    }
}
