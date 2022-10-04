using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Codeqo.NativeMediaPlayer
{
    [System.Serializable]
    public class Playlist
    {
        public UnityEvent UIEvent = new UnityEvent();
        public int Id;
        public UriType Path;
        public string Title;
        public string Artist;
        public List<MediaItem> MediaItems;
        public int Count => MediaItems.Count;

        public Playlist(int id,  UriType path, List<MediaItem> mediaItems = null, string title = null, string artist = null)
        {
            if (mediaItems == null) mediaItems = new List<MediaItem>();
            Id = id;
            Path = path;
            Title = title;
            Artist = artist;
            MediaItems = mediaItems;
        }

        public Playlist(UriType path, List<MediaItem> mediaItems = null, string title = null, string artist = null)
        {
            if (mediaItems == null) mediaItems = new List<MediaItem>();
            Path = path;
            Title = title;
            Artist = artist;
            MediaItems = mediaItems;
        }

        public void AddMediaItem(int index = -1, MediaItem item = null)
        {
            if (item != null && Path != item.Path)
            {
                Debug.LogError("MediaItem's UriType must be equal to the Playlist's UriType");
                return;
            }

            /* Create a new MediaItem if item is null */
            if (item == null) item = new MediaItem(Path, null, new MediaMetadata());

            item.Id = Count;
            MediaItems.Insert(item.Id, item);
            Sync();
        }

        public void AddMediaItem(MediaItem item = null)
        {
            if (item != null && Path != item.Path)
            {
                Debug.LogError("MediaItem's UriType must be equal to the Playlist's UriType");
                return;
            }

            /* Create a new MediaItem if item is null */
            if (item == null) item = new MediaItem(Path, null, new MediaMetadata());

            item.Id = Count;
            MediaItems.Add(item);
            Sync();
        }

        public void RemoveMediaItem(int index)
        {
            MediaItems.Remove(MediaItems[index]);
            Sync();
        }

        public void SetMediaItems(List<MediaItem> mediaItems)
        {
            MediaItems = mediaItems;
            Sync();
        }

        public void ClearMediaItems()
        {
            MediaItems.Clear();
            Sync();
        }

        public void Sync()
        {
            MediaEvents.MetadataUpdate.Clear();
            MediaEvents.UnregisterMetadataUpdates();

            PlayerPrefs.SetInt("URI_TYPE", (int)Path);
            PlayerPrefs.SetInt("NUM_TRACKS", MediaItems.Count);
            PlayerPrefs.Save();

            for (int i = 0; i < MediaItems.Count; i++)
            {
                MediaItems[i].Sync();
            }

            MediaEvents.RegisterMetadataUpdates();
        }
    }
}