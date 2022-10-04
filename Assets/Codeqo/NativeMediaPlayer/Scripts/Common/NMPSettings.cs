using Codeqo.NativeMediaPlayer.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Codeqo.NativeMediaPlayer
{
    public class NMPSettings : MonoBehaviour
    {
        public enum RepeatMode : short
        {
            Disabled = 0,
            RepeatOne,
            RepeatAll
        }

        [SerializeField] bool autoInit = true;
        [SerializeField] bool autoSave = true;
        [SerializeField] bool useRemoteCommands = true;
        [SerializeField] bool playOnAppStart = false;
        [SerializeField] MediaSource source;
        [SerializeField] MediaEventListener listener;
        [SerializeField] AndroidSettings androidSettings;
        [SerializeField] iOSSettings iosSettings;

        [SerializeField] RepeatMode repeatMode = 0;
        [SerializeField] bool shuffleMode = false;
        [SerializeField] int seekIncrement = 10;
        [SerializeField] int preBufferDuration = 5;
        [SerializeField] int startIndex = 0;
        [SerializeField] AddIndexType addIndexType = AddIndexType.None;
        [SerializeField] string albumTitle = "Unknown Album Title";
        [SerializeField] string albumArtist = "Unknown Album Artist";
        [SerializeField] string title = "Unknown Title";
        [SerializeField] string artist = "Unknown Artist";

        void Awake()
        {
            /* Saving Default Settings */
            DefaultMediaMetadata.AddIndex = addIndexType;
            DefaultMediaMetadata.AlbumArtist = albumArtist;
            DefaultMediaMetadata.AlbumTitle = albumTitle;
            DefaultMediaMetadata.Title = title;
            DefaultMediaMetadata.Artist = artist;

            PlayerPrefs.SetInt("DEFAULT_ADD_INDEX_TYPE", (int)addIndexType);
            PlayerPrefs.SetString("DEFAULT_ALBUM_ARTIST", albumArtist);
            PlayerPrefs.SetString("DEFAULT_ALBUM_TITLE", albumTitle);
            PlayerPrefs.SetString("DEFAULT_TITLE", title);
            PlayerPrefs.SetString("DEFAULT_ARTIST", artist);
            PlayerPrefs.SetString("LISTENER_ID", listener.name);
            PlayerPrefs.SetInt("START_INDEX", startIndex);

            PlayerPrefs.SetInt("AUTO_SAVE", autoSave ? 1 : 0);
            PlayerPrefs.SetInt("SEEK_INCREMENT", seekIncrement);
            PlayerPrefs.SetInt("PRE_BUFFER_DURATION", preBufferDuration);
            PlayerPrefs.SetInt("USE_REMOTE_COMMANDS", useRemoteCommands ? 1 : 0);

            if (PlayerPrefs.HasKey("SHUFFLE_MODE") || !autoSave)
                PlayerPrefs.SetInt("SHUFFLE_MODE", shuffleMode ? 1 : 0);
            if (PlayerPrefs.HasKey("REPEAT_MODE") || !autoSave)
                PlayerPrefs.SetInt("REPEAT_MODE", (int)repeatMode);

            PlayerPrefs.Save();
        }

        void Start()
        {
            if (autoInit)
            {
                if (useRemoteCommands)
                {
                    androidSettings.SaveToPlayerPrefs();
                    iosSettings.SaveToPlayerPrefs();
                    Debug.Log("Native Settings Saved to PlayerPrefs");
                }
                InitPlayer();
#if UNITY_EDITOR
                CheckStreamingAssets();
#endif
            }
        }

        public void InitPlayer()
        {
            if (source == null)
            {
                GetError();
                return;
            }

            switch (source.initWith)
            {
                case InitWith.MediaItem:
                    if (source.defaultMediaItem == null) GetError();
                    MediaController.Init(source.defaultMediaItem.MediaItem, listener.name, playOnAppStart);
                    break;

                case InitWith.Playlist:
                    if (source.defaultPlaylist == null) GetError();
                    MediaController.Init(source.defaultPlaylist.Playlist, listener.name, playOnAppStart);
                    break;

                case InitWith.Playlists:
                    if (source.defaultPlaylists.Count == 0) GetError();

                    List<Playlist> playlists = new List<Playlist>();
                    for (int i = 0; i < source.defaultPlaylists.Count; i++)
                    {
                        playlists.Add(source.defaultPlaylists[i].Playlist);
                    }

                    MediaController.Init(playlists, listener.name, playOnAppStart);
                    break;

                default:
                    Debug.LogError("Invalid Media Source type");
                    break;
            }
        }

        void GetError()
        {
            Debug.LogError("Failed to initialize MediaStyleNotification. Default Media Source is null");
        }

        void OnApplicationPause(bool pause)
        {
            if (!pause && MediaPlayer.isInit)
            {
                MediaEvents.OnPrepared.Invoke();
            }
        }

        public void CheckStreamingAssets()
        {
#if UNITY_EDITOR
            if (MediaPlayer.CurrentUriType == (int)UriType.StreamingAssets && !Directory.Exists(Application.streamingAssetsPath))
            {
                Debug.LogWarning("StreamingAssets folder doesn't exist. Creating a new StreamingAssets folder.");
                Debug.LogWarning("Please make sure to put your audio files in StreamingAssets folder (QuickStart.pdf)");
                Debug.LogError("StreamingAssets folder created.\nPlease restart the Editor player.");
                Directory.CreateDirectory(Application.streamingAssetsPath);

                foreach (string file in Directory.GetFiles(Application.dataPath + "/Codeqo/MediaPlayer/AudioSamples/", "*.*"))
                {
                    string path = Application.streamingAssetsPath + "/" + Path.GetFileName(file);
                    Debug.LogWarning("Moving " + file + " to " + path);
                    File.Move(file, path);
                }
            }
#endif
        }
    }
}
