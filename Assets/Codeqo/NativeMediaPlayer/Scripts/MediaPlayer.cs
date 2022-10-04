using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Codeqo.NativeMediaPlayer
{
    public enum RetrieveMetadataType : short
    {
        RetrieveMediaMetadata = 0,
        AddCustomMediaMetadata = 1
    }

    public enum RetrieveArtworkType : short
    {
        RetrieveArtwork = 0,
        AddCustomArtwork = 1,
        AddCustomArtworkWhenArtworkIsUnavailble = 2
    }

    public enum UriType : short
    {
        StreamingAssets = 0,
        RemoteURL = 1
    }

    public class MediaPlayer
    {
        /* Static */
        public static bool isLoaded = false;
        public static bool isInit = false;

        public static string Error; // Returns the latest error message
        public static Playlist CurrentPlaylist 
        {
            get
            {
                if (MediaController.CurrentPlaylist == null)
                {
                    Debug.LogError("MediaController is not initiated");
                    return null;
                }
                return MediaController.CurrentPlaylist;
            }
        }
        public static UriType CurrentUriType
        {
            get
            {
                int type = PlayerPrefs.GetInt("URI_TYPE");
                if (type == 0) return UriType.StreamingAssets;
                else return UriType.RemoteURL;
            }
        }
        public static int SeekIncrement => PlayerPrefs.GetInt("SEEK_INCREMENT");
        public static int PreBufferDuration => PlayerPrefs.GetInt("PRE_BUFFER_DURATION");
        public static string ListenerId => PlayerPrefs.GetString("LISTENER_ID");

#if UNITY_EDITOR

        public static UnityTester _tester;

#elif UNITY_ANDROID

        // Java Plugin
        private const string PACKAGE_NAME = "com.codeqo.MediaStyleNotification.Bridge";
        private const string UNITY_DEFAULT_JAVA_CLASS = "com.unity3d.player.UnityPlayer";
        private static AndroidJavaObject _plugin;

        // Java Plugin Android Core Methods
        private const string CREATE = "create";
        private const string PREPARE = "prepare";

        // Java Plugin Player Methods
        private const string PLAY = "play";
        private const string PAUSE = "pause";
        private const string STOP = "stop";
        private const string PREVIOUS = "previous";
        private const string NEXT = "next";
        private const string REWIND = "rewind";
        private const string FASTFORWARD = "fastForward";
        private const string SEEK_TO = "seekTo";
        private const string RELEASE = "release";

        // Java Plugin Set Methods
        private const string SET_VOLUME = "setVolume";
        private const string SET_REPEAT_MODE = "setRepeatMode";
        private const string SET_SHUFFLE_MODE_ENABLED = "setShuffleModeEnabled";
        private const string SET_CURRENT_POSITION = "seekTo";

        // Java Plugin Get Methods
        private const string GET_VOLUME = "getVolume";
        private const string GET_REPEAT_MODE = "getRepeatMode";
        private const string GET_SHUFFLE_MODE_ENABLED = "getShuffleModeEnabled";
        private const string GET_CURRENT_MEDIA_ITEM_INDEX = "getCurrentMediaItemIndex";
        private const string GET_DURATION = "getDuration";
        private const string GET_CURRENT_POSITION = "getCurrentPosition";
        private const string GET_SHUFFLE_ORDER = "getShuffleOrder";
        private const string IS_PLAYING = "isPlaying";
        private const string IS_LOADING = "isLoading";
        private const string HAS_NEXT_MEDIA_ITEM = "hasNextMediaItem";
        private const string HAS_PREVIOUS_MEDIA_ITEM = "hasPreviousMediaItem";

        // Retrieving Metadata
        private const string RETRIEVE_ALBUM_TITLE = "retrieveAlbumTitle";
        private const string RETRIEVE_ALBUM_ARTIST = "retrieveAlbumArtist";
        private const string RETRIEVE_TITLE = "retrieveTitle";
        private const string RETRIEVE_ARTIST = "retrieveArtist";
        private const string RETRIEVE_GENRE = "retrieveGenre";
        private const string RETRIEVE_RELEASE_DATE = "retrieveReleaseDate";
        private const string RETRIEVE_ARTWORK = "retrieveArtwork";

        // Playlists
        private const string ADD_MEDIA_ITEM = "addMediaItem";
        private const string REMOVE_MEDIA_ITEM = "removeMediaItem";        

#elif UNITY_IPHONE

        // DllImports : iOS Core Methods
        [DllImport("__Internal")] private static extern void _prepare(int _id, string _listenerId, bool _playWhenReady);
        [DllImport("__Internal")] private static extern void _prepareMediaItem(int _id, bool _playWhenReady, bool _newPlaylist);
        [DllImport("__Internal")] private static extern void _reload();

        // DllImports : Playback Functions
        [DllImport("__Internal")] private static extern void _play();
        [DllImport("__Internal")] private static extern void _pause();
        [DllImport("__Internal")] private static extern void _stop();
        [DllImport("__Internal")] private static extern void _frelease();
        [DllImport("__Internal")] private static extern void _previousTrack();
        [DllImport("__Internal")] private static extern void _nextTrack();
        [DllImport("__Internal")] private static extern void _seekBackward();
        [DllImport("__Internal")] private static extern void _seekForward();
        [DllImport("__Internal")] private static extern void _seekTo(float _position);

        // DllImports : Core Variables
        [DllImport("__Internal")] private static extern float _getVolume();
        [DllImport("__Internal")] private static extern void _setVolume(float _volume);
        [DllImport("__Internal")] private static extern bool _isPlaying();
        [DllImport("__Internal")] private static extern bool _isLoading();
        [DllImport("__Internal")] private static extern float _getDuration();
        [DllImport("__Internal")] private static extern float _getCurrentPosition();

        // DllImports : Extended Variables
        [DllImport("__Internal")] private static extern int _getCurrentMediaItemIndex();
        [DllImport("__Internal")] private static extern int _getRepeatMode();
        [DllImport("__Internal")] private static extern void _setRepeatMode(int _repeatMode);
        [DllImport("__Internal")] private static extern bool _getShuffleModeEnabled();
        [DllImport("__Internal")] private static extern void _setShuffleModeEnabled(bool _shuffleModeEnabled);
        [DllImport("__Internal")] private static extern int _getShuffleOrder(int _id);
        [DllImport("__Internal")] private static extern bool _hasPreviousMediaItem();
        [DllImport("__Internal")] private static extern bool _hasNextMediaItem();

        // DllImports : Retrieving Metadata
        [DllImport("__Internal")] private static extern string _retrieveAlbumTitle(int _id);
        [DllImport("__Internal")] private static extern string _retrieveAlbumArtist(int _id);
        [DllImport("__Internal")] private static extern string _retrieveTitle(int _id);
        [DllImport("__Internal")] private static extern string _retrieveArtist(int _id);
        [DllImport("__Internal")] private static extern string _retrieveGenre(int _id);
        [DllImport("__Internal")] private static extern string _retrieveReleaseDate(int _id);
        [DllImport("__Internal")] private static extern string _retrieveArtwork(int _id);

        // DllImports : Playlists
        [DllImport("__Internal")] private static extern void _addMediaItem(int _id);
        [DllImport("__Internal")] private static extern void _removeMediaItem(int _id);

#endif

        public static float Volume
        {
#if UNITY_EDITOR
            get => _tester.getVolume();
            set => _tester.setVolume(value);
#elif UNITY_ANDROID
            get => _plugin.Call<float>(GET_VOLUME);
            set => _plugin.Call(SET_VOLUME, value);
#elif UNITY_IPHONE
            get => _getVolume();
            set => _setVolume(value);                
#endif
        }

        public static int RepeatMode
        {
            get
            {
#if UNITY_EDITOR
                return _tester.getRepeatMode();
#elif UNITY_ANDROID
                return _plugin.Call<int>(GET_REPEAT_MODE);
#elif UNITY_IPHONE
                return _getRepeatMode();
#endif
            }

            set
            {
#if UNITY_EDITOR
                _tester.setRepeatMode(value);
#elif UNITY_ANDROID
                _plugin.Call(SET_REPEAT_MODE, value);
#elif UNITY_IPHONE            
                _setRepeatMode(value);               
#endif
            }
        }

        public static bool ShuffleModeEnabled
        {
#if UNITY_EDITOR
            get => _tester.getShuffleModeEnabled();
            set => _tester.setShuffleModeEnabled(value);
#elif UNITY_ANDROID
            get => _plugin.Call<bool>(GET_SHUFFLE_MODE_ENABLED);
            set => _plugin.Call(SET_SHUFFLE_MODE_ENABLED, value);
#elif UNITY_IPHONE            
            get => _getShuffleModeEnabled();
            set => _setShuffleModeEnabled(value);            
#endif
        }

        public static bool isPlaying
        {
#if UNITY_EDITOR
            get => _tester.isPlaying;
#elif UNITY_ANDROID
            get => _plugin.Call<bool>(IS_PLAYING);
#elif UNITY_IPHONE
            get => _isPlaying();
#endif
        }

        public static bool isLoading
        {
#if UNITY_EDITOR
            get => _tester.isLoading;
#elif UNITY_ANDROID
            get => _plugin.Call<bool>(IS_LOADING);
#elif UNITY_IPHONE
            get => _isLoading();
#endif
        }

        public static int CurrentMediaItemIndex
        {
#if UNITY_EDITOR
            get => _tester.getCurrentMediaItemIndex();
#elif UNITY_ANDROID
            get => _plugin.Call<int>(GET_CURRENT_MEDIA_ITEM_INDEX);
#elif UNITY_IPHONE
            get => _getCurrentMediaItemIndex();
#endif
        }

        public static void Init(Playlist _list, string _listenerId, bool _playWhenReady = false)
        {
            if (isLoaded)
            {
                Debug.Log("NativeMediaPlayer has already been initiated.");
#if UNITY_EDITOR
                _tester.reload();
#elif UNITY_ANDROID
                _plugin.Call("reload");
#elif UNITY_IPHONE
                _reload();
#endif
                return;
            }

            isLoaded = true;
            _list.Sync();

#if UNITY_EDITOR
            GameObject testerObj = new GameObject("UnityTester");
            _tester = UnityTester.Create(testerObj, _listenerId, _playWhenReady, _list);
#elif UNITY_ANDROID
            using (var pc = new AndroidJavaObject(PACKAGE_NAME))
                _plugin = pc.CallStatic<AndroidJavaObject>("getInstance");
            AndroidJavaClass _class = new AndroidJavaClass(UNITY_DEFAULT_JAVA_CLASS);
            AndroidJavaObject _activity = _class.GetStatic<AndroidJavaObject>("currentActivity");
            _plugin.Call(CREATE, _activity, _listenerId, _playWhenReady);
#elif UNITY_IPHONE
            _prepare(PlayerPrefs.GetInt("START_INDEX", 0), _listenerId, _playWhenReady);
#endif
        }

        public static void Prepare(bool _playWhenReady = false, int _id = -1, Playlist _list = null)
        {
            bool _listExists = _list != null;
            if (_list != null) 
            { 
                if (_list.MediaItems.Count > 0)
                {
                    _list.Sync();
                }
                else
                {
                    Debug.LogError("This playlist is empty. Please add a media item before preparing this playlist.");
                    return;
                }
            }
            if (_id == -1) _id = CurrentMediaItemIndex;

#if UNITY_EDITOR
            _tester.prepare(_playWhenReady, _id, _list);
#elif UNITY_ANDROID
            _plugin.Call(PREPARE, _playWhenReady, _id, _listExists);
#elif UNITY_IPHONE
            _prepareMediaItem(_id, _playWhenReady, _listExists);
#endif
        }

        public static void Play()
        {
#if UNITY_EDITOR
            _tester.play();
#elif UNITY_ANDROID
            _plugin.Call(PLAY);
#elif UNITY_IPHONE
            _play();
#endif
        }

        public static void Play(int _id)
        {
            Prepare(true, _id);
        }

        public static void Stop()
        {
#if UNITY_EDITOR
            _tester.stop();
#elif UNITY_ANDROID
            _plugin.Call(STOP);
#elif UNITY_IPHONE
            _stop();
#endif
        }

        public static void Release()
        {
#if UNITY_EDITOR
            _tester.destroy();
#elif UNITY_ANDROID
            _plugin.Call(RELEASE);
            _plugin = null;
#elif UNITY_IPHONE
            _frelease();
#endif
        }

        public static void Pause()
        {
#if UNITY_EDITOR
            _tester.pause();
#elif UNITY_ANDROID
            _plugin.Call(PAUSE);
#elif UNITY_IPHONE
            _pause();
#endif
        }

        public static void Previous()
        {
#if UNITY_EDITOR
            _tester.previous();
#elif UNITY_ANDROID
            _plugin.Call(PREVIOUS);
#elif UNITY_IPHONE
            _previousTrack();
#endif
        }

        public static void Next()
        {
#if UNITY_EDITOR
            _tester.next();
#elif UNITY_ANDROID
            _plugin.Call(NEXT);
#elif UNITY_IPHONE
            _nextTrack();
#endif
        }

        public static bool HasPreviousMediaItem()
        {
#if UNITY_EDITOR
            return _tester.hasPreviousMediaItem;
#elif UNITY_ANDROID
            return _plugin.Call<bool>(HAS_PREVIOUS_MEDIA_ITEM);
#elif UNITY_IPHONE
            return _hasPreviousMediaItem();
#endif
        }

        public static bool HasNextMediaItem()
        {
#if UNITY_EDITOR
            return _tester.hasNextMediaItem;
#elif UNITY_ANDROID
            return _plugin.Call<bool>(HAS_NEXT_MEDIA_ITEM);
#elif UNITY_IPHONE
            return _hasNextMediaItem();
#endif
        }

        public static void FastForward()
        {
#if UNITY_EDITOR
            _tester.fastForward();
#elif UNITY_ANDROID
            _plugin.Call(FASTFORWARD);
#elif UNITY_IPHONE
            _seekForward();
#endif
        }

        public static void Rewind()
        {
#if UNITY_EDITOR
            _tester.rewind();
#elif UNITY_ANDROID
            _plugin.Call(REWIND);
#elif UNITY_IPHONE
            _seekBackward();
#endif
        }

        public static void SeekTo(float _time)
        {
#if UNITY_EDITOR
            _tester.seekTo(_time);
#elif UNITY_ANDROID
            _plugin.Call(SEEK_TO, _time);
#elif UNITY_IPHONE
            _seekTo(_time);
#endif
        }

        public static float GetDuration()
        {
#if UNITY_EDITOR
            return _tester.getCurrentDuration();
#elif UNITY_ANDROID
            return _plugin.Call<float>(GET_DURATION);
#elif UNITY_IPHONE
            return _getDuration();
#endif
        }

        public static float GetCurrentPosition()
        {
#if UNITY_EDITOR
            return _tester.getCurrentPosition();
#elif UNITY_ANDROID
            return _plugin.Call<float>(GET_CURRENT_POSITION);
#elif UNITY_IPHONE
            return _getCurrentPosition();
#endif
        }

        public static int[] GetShuffleOrder()
        {
#if UNITY_EDITOR
            return _tester.getShuffleOrder();
#elif UNITY_ANDROID
            return _plugin.Call<int[]>(GET_SHUFFLE_ORDER);
#elif UNITY_IPHONE
            int[] shuffleOrder = new int[CurrentPlaylist.Count];
            for (int i = 0; i < CurrentPlaylist.Count; i++)
            {
                shuffleOrder[i] = _getShuffleOrder(i);
            }
            return shuffleOrder;
#endif
        }

        public static string RetrieveAlbumTitle(int _id = -1)
        {
            if (_id == -1) _id = CurrentMediaItemIndex;

#if UNITY_EDITOR
            return "Retrieved Album Title #" + (_id + 1);
#elif UNITY_ANDROID
            return _plugin.Call<string>(RETRIEVE_ALBUM_TITLE, _id);
#elif UNITY_IPHONE
            return _retrieveAlbumTitle(_id);
#endif
        }

        public static string RetrieveAlbumArtist(int _id = -1)
        {
            if (_id == -1) _id = CurrentMediaItemIndex;

#if UNITY_EDITOR
            return "Retrieved Album Artist #" + (_id + 1);
#elif UNITY_ANDROID
            return _plugin.Call<string>(RETRIEVE_ALBUM_ARTIST, _id);
#elif UNITY_IPHONE
            return _retrieveAlbumArtist(_id);
#endif
        }

        public static string RetrieveTitle(int _id = -1)
        {
            if (_id == -1) _id = CurrentMediaItemIndex;

#if UNITY_EDITOR
            return "Retrieved Title #" + (_id + 1);
#elif UNITY_ANDROID
            return _plugin.Call<string>(RETRIEVE_TITLE, _id);
#elif UNITY_IPHONE
            return _retrieveTitle(_id);
#endif
        }

        public static string RetrieveArtist(int _id = -1)
        {
            if (_id == -1) _id = CurrentMediaItemIndex;

#if UNITY_EDITOR
            return "Retrieved Artist #" + (_id + 1);
#elif UNITY_ANDROID
            return _plugin.Call<string>(RETRIEVE_ARTIST, _id);
#elif UNITY_IPHONE
            return _retrieveArtist(_id);
#endif
        }

        public static string RetrieveGenre(int _id = -1)
        {
            if (_id == -1) _id = CurrentMediaItemIndex;

#if UNITY_EDITOR
            return "Retrieved Genre #" + (_id + 1);
#elif UNITY_ANDROID
            return _plugin.Call<string>(RETRIEVE_GENRE, _id);
#elif UNITY_IPHONE
            return _retrieveGenre(_id);
#endif
        }

        public static string RetrieveReleaseDate(int _id = -1)
        {
            if (_id == -1) _id = CurrentMediaItemIndex;

#if UNITY_EDITOR
            return "Retrieved Release Date #" + (_id + 1);
#elif UNITY_ANDROID
            return _plugin.Call<string>(RETRIEVE_RELEASE_DATE, _id);
#elif UNITY_IPHONE
            return _retrieveReleaseDate(_id);
#endif
        }

        public static string RetrieveArtwork(int _id = -1)
        {
            if (_id == -1) _id = CurrentMediaItemIndex;

#if UNITY_EDITOR
            return null;
#elif UNITY_ANDROID
            return _plugin.Call<string>(RETRIEVE_ARTWORK, _id);
#elif UNITY_IPHONE
            return _retrieveArtwork(_id);
#endif
        }

        public static void AddMediaItem(MediaItem _item, int _id = -1)
        {
            if (_id == -1) _id = CurrentPlaylist.Count;
            CurrentPlaylist.AddMediaItem(_id, _item);

#if UNITY_EDITOR
            _tester.addMediaItem(_id, _item);
#elif UNITY_ANDROID
            _plugin.Call(ADD_MEDIA_ITEM, _id);
#elif UNITY_IPHONE
            _addMediaItem(_id);
#endif
        }

        public static void RemoveMediaItem(int _id)
        {
            CurrentPlaylist.RemoveMediaItem(_id);

#if UNITY_EDITOR
            _tester.removeMediaItem(_id);
#elif UNITY_ANDROID
            _plugin.Call(REMOVE_MEDIA_ITEM, _id);
#elif UNITY_IPHONE
            _removeMediaItem(_id);
#endif
        }
    }
}
