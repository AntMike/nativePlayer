using System.Collections.Generic;
using UnityEngine.Events;

namespace Codeqo.NativeMediaPlayer
{
    public static class MediaEvents
    {
        // Remote Only 
        public static UnityEvent OnIsBufferingChangedTrue = new UnityEvent(); //DisableUIComponents
        public static UnityEvent OnIsBufferingChangedFalse = new UnityEvent(); //EnableUIComponents

        // Shared
        public static UnityEvent OnIsPlayingChangedTrue = new UnityEvent();
        public static UnityEvent OnIsPlayingChangedFalse = new UnityEvent();
        public static UnityEvent OnIsLoadingChangedTrue = new UnityEvent();
        public static UnityEvent OnIsLoadingChangedFalse = new UnityEvent();
        public static UnityEvent OnInit = new UnityEvent();
        public static UnityEvent OnReady = new UnityEvent();
        public static UnityEvent OnRetrieved = new UnityEvent();
        public static UnityEvent OnPrepared = new UnityEvent(); //Also invoked when app gets focus again
        public static UnityEvent OnComplete = new UnityEvent();        
        public static Dictionary<int, UnityAction> MetadataUpdate = new Dictionary<int, UnityAction>();

        public static void Clear()
        {
            OnIsBufferingChangedTrue.RemoveAllListeners();
            OnIsBufferingChangedFalse.RemoveAllListeners();
            OnIsPlayingChangedTrue.RemoveAllListeners();
            OnIsPlayingChangedFalse.RemoveAllListeners();
            OnIsLoadingChangedTrue.RemoveAllListeners();
            OnIsLoadingChangedFalse.RemoveAllListeners();
            OnInit.RemoveAllListeners();
            OnReady.RemoveAllListeners();
            OnRetrieved.RemoveAllListeners();
            OnPrepared.RemoveAllListeners();
            OnComplete.RemoveAllListeners();
        }

        public static void RegisterMetadataUpdates()
        {
            for (int i = 0; i < MetadataUpdate.Count; i++)
            {
                OnReady.AddListener(MetadataUpdate[i]);
            }
        }

        public static void UnregisterMetadataUpdates()
        {
            for (int i = 0; i < MetadataUpdate.Count; i++)
            {
                OnReady.RemoveListener(MetadataUpdate[i]);
            }
        }
        

        public static string GetPlayerName()
        {
#if UNITY_EDITOR
            return "UnityTester (Editor)";
#elif UNITY_ANDROID
            if (MediaPlayer.CurrentPlaylist.Path == UriType.StreamingAssets)
            {
                return "MediaPlayer (Android)";
            }
            else
            {
                return "ExoPlayer (Android)";
            }
#elif UNITY_IPHONE
            if (MediaPlayer.CurrentPlaylist.Path == UriType.StreamingAssets)
            {
                return "AVAudioPlayer (iOS)";
            }
            else
            {
                return "AVPlayer (iOS)";
            }
#endif
        }
    }

    public static class RepeatMode
    {
        public const int Disabled = 0;
        public const int RepeatOne = 1;
        public const int RepeatAll = 2;

        public static UnityEvent DisabledEvent = new UnityEvent();
        public static UnityEvent RepeatOneEvent = new UnityEvent();
        public static UnityEvent RepeatAllEvent = new UnityEvent();

        public static void OnRepeatModeChanged(UnityAction _action) 
        {
            RepeatOneEvent.AddListener(_action);
            RepeatAllEvent.AddListener(_action);
            DisabledEvent.AddListener(_action);
        }

        public static void UpdateUI()
        {
            switch (MediaPlayer.RepeatMode)
            {
                case RepeatOne:
                    RepeatOneEvent.Invoke();
                    break;
                case RepeatAll:
                    RepeatAllEvent.Invoke();
                    break;
                default:
                    DisabledEvent.Invoke();
                    break;
            }
        }
    }

    public static class ShuffleMode
    {
        public static UnityEvent OnShuffleModeEnabledEvent = new UnityEvent();
        public static UnityEvent OnShuffleModeDisabledEvent = new UnityEvent();

        public static void OnShuffleModeEnabledChanged(UnityAction _action)
        {
            OnShuffleModeEnabledEvent.AddListener(_action);
            OnShuffleModeDisabledEvent.AddListener(_action);
        }

        public static void UpdateUI()
        {
            if (MediaPlayer.ShuffleModeEnabled)
            {
                OnShuffleModeEnabledEvent.Invoke();
            }
            else
            {
                OnShuffleModeDisabledEvent.Invoke();
            }
        }
    }
}

