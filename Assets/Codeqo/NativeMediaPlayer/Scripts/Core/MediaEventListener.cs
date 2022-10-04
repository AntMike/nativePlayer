using UnityEngine;
using UnityEngine.Events;

namespace Codeqo.NativeMediaPlayer
{
    public class MediaEventListener : MonoBehaviour
    {
        private const string ON_INIT = "ON_INIT";
        private const string ON_READY = "ON_READY";
        private const string ON_RETRIEVED = "ON_RETRIEVED";
        private const string ON_PREPARED = "ON_PREPARED";
        private const string ON_COMPLETE = "ON_COMPLETE";
        private const string ON_IS_PLAYING_CHANGED_TRUE = "ON_IS_PLAYING_CHANGED_TRUE";
        private const string ON_IS_PLAYING_CHANGED_FALSE = "ON_IS_PLAYING_CHANGED_FALSE";
        private const string ON_IS_LOADING_CHANGED_TRUE = "ON_IS_LOADING_CHANGED_TRUE";
        private const string ON_IS_LOADING_CHANGED_FALSE = "ON_IS_LOADING_CHANGED_FALSE";
        private const string ON_IS_BUFFERING_CHANGED_TRUE = "ON_IS_BUFFERING_CHANGED_TRUE";
        private const string ON_IS_BUFFERING_CHANGED_FALSE = "ON_IS_BUFFERING_CHANGED_FALSE";

        [SerializeField] UnityEvent onInit = new UnityEvent();
        [SerializeField] UnityEvent onReady = new UnityEvent();
        [SerializeField] UnityEvent onRetrieved = new UnityEvent();
        [SerializeField] UnityEvent onPrepared = new UnityEvent();
        [SerializeField] UnityEvent onComplete = new UnityEvent();
        [SerializeField] UnityEvent onIsPlayingChangedTrue = new UnityEvent();
        [SerializeField] UnityEvent onIsPlayingChangedFalse = new UnityEvent();
        [SerializeField] UnityEvent onIsLoadingChangedTrue = new UnityEvent();
        [SerializeField] UnityEvent onIsLoadingChangedFalse = new UnityEvent();
        [SerializeField] UnityEvent onIsBufferingChangedTrue = new UnityEvent();
        [SerializeField] UnityEvent onIsBufferingChangedFalse = new UnityEvent();
        [SerializeField] UnityEvent onError = new UnityEvent();

        void Awake()
        {
            if (FindObjectsOfType<MediaEventListener>().Length > 1)
            {
                Debug.LogError("MediaEventListener already exists in the current scene.");
                Destroy(gameObject);
            }
        }

        public static void AddOnInitListener(UnityAction action)
        {
            if (action == null) return;
            MediaEvents.OnInit.AddListener(action);
        }

        public static void AddOnReadyListener(UnityAction action)
        {
            if (action == null) return;
            MediaEvents.OnReady.AddListener(action);
        }

        public static void AddOnPreparedListener(UnityAction action)
        {
            if (action == null) return;
            MediaEvents.OnPrepared.AddListener(action);
        }

        public static void AddOnCompleteListener(UnityAction action)
        {
            if (action == null) return;
            MediaEvents.OnComplete.AddListener(action);
        }

        public static void AddOnIsPlayingChangedListener(bool isPlaying, UnityAction action)
        {
            if (action == null) return;
            if (isPlaying) MediaEvents.OnIsPlayingChangedTrue.AddListener(action);
            else MediaEvents.OnIsPlayingChangedFalse.AddListener(action);            
        }

        public static void AddOnIsLoadingChangedListener(bool isLoading, UnityAction action)
        {
            if (action == null) return;
            if (isLoading) MediaEvents.OnIsLoadingChangedTrue.AddListener(action);
            else MediaEvents.OnIsLoadingChangedFalse.AddListener(action);
        }

        public static void AddOnIsBufferingChangedListener(bool isBuffering, UnityAction action)
        {
            if (action == null) return;
            if (isBuffering) MediaEvents.OnIsBufferingChangedTrue.AddListener(action);
            else MediaEvents.OnIsBufferingChangedFalse.AddListener(action);
        }

        private void OnDestroy()
        {
            if (MediaPlayer.isInit) MediaPlayer.isInit = false;
            if (MediaPlayer.isPlaying) MediaPlayer.Stop();
        }

        public void UnityReceiveMessage(string _state)
        {
            switch (_state)
            {
                case ON_INIT:
                    if (MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnInit");
                    MediaPlayer.isInit = true;
                    MediaEvents.OnInit.Invoke();
                    if (onInit.GetPersistentEventCount() > 0)
                    {
                        onInit.Invoke();
                    }
                    break;

                case ON_READY:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnReady");
                    MediaEvents.OnReady.Invoke();
                    if (onReady.GetPersistentEventCount() > 0)
                    {
                        onReady.Invoke();
                    }
                    UnityReceiveMessage(ON_RETRIEVED);
                    break;

                case ON_RETRIEVED:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnRetrieved");
                    MediaEvents.OnRetrieved.Invoke();
                    if (onRetrieved.GetPersistentEventCount() > 0)
                    {
                        onRetrieved.Invoke();
                    }
                    break;

                case ON_PREPARED:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnPrepared");
                    MediaEvents.OnPrepared.Invoke();
                    if (onPrepared.GetPersistentEventCount() > 0)
                    {
                        onPrepared.Invoke();
                    }
                    break;

                case ON_COMPLETE:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnCompletion");
                    MediaEvents.OnComplete.Invoke();
                    if (onComplete.GetPersistentEventCount() > 0)
                    {
                        onComplete.Invoke();
                    }
                    break;

                case ON_IS_PLAYING_CHANGED_TRUE:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnIsPlayingChanged(True)");
                    MediaEvents.OnIsPlayingChangedTrue.Invoke();
                    if (onIsPlayingChangedTrue.GetPersistentEventCount() > 0)
                    {
                        onIsPlayingChangedTrue.Invoke();
                    }
                    break;

                case ON_IS_PLAYING_CHANGED_FALSE:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnIsPlayingChanged(False)");
                    MediaEvents.OnIsPlayingChangedFalse.Invoke();
                    if (onIsPlayingChangedFalse.GetPersistentEventCount() > 0)
                    {
                        onIsPlayingChangedFalse.Invoke();
                    }
                    break;

                case ON_IS_LOADING_CHANGED_TRUE:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnIsLoadingChanged(True)");
                    MediaEvents.OnIsLoadingChangedTrue.Invoke();
                    if (onIsLoadingChangedTrue.GetPersistentEventCount() > 0)
                    {
                        onIsLoadingChangedTrue.Invoke();
                    }
                    break;

                case ON_IS_LOADING_CHANGED_FALSE:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnIsLoadingChanged(False)");
                    MediaEvents.OnIsLoadingChangedFalse.Invoke();
                    if (onIsLoadingChangedFalse.GetPersistentEventCount() > 0)
                    {
                        onIsLoadingChangedFalse.Invoke();
                    }
                    break;

                case ON_IS_BUFFERING_CHANGED_TRUE:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnIsBufferingChanged(True)");
                    MediaEvents.OnIsBufferingChangedTrue.Invoke();
                    if (onIsBufferingChangedTrue.GetPersistentEventCount() > 0)
                    {
                        onIsBufferingChangedTrue.Invoke();
                    }
                    break;

                case ON_IS_BUFFERING_CHANGED_FALSE:
                    if (!MediaPlayer.isInit) return;
                    Debug.Log("MediaEventLisener : OnIsBufferingChanged(False)");
                    MediaEvents.OnIsBufferingChangedFalse.Invoke();
                    if (onIsBufferingChangedFalse.GetPersistentEventCount() > 0)
                    {
                        onIsBufferingChangedFalse.Invoke();
                    }
                    break;
            }
        }

        public void UnityReceiveError(string _error)
        {
            if (!MediaPlayer.isInit) return;

            if (onError.GetPersistentEventCount() > 0)
            {
                MediaPlayer.Error = _error;
                onError.Invoke();
            }
        }
    }
}

