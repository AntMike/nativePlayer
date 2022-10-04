using System.Collections.Generic;
using UnityEngine;

namespace Codeqo.NativeMediaPlayer
{
    public class MediaController
    {
        /* Variables */
        public static int CurrentPlaylistIndex = 0;
        public static Playlist CurrentPlaylist => Playlists[CurrentPlaylistIndex];
        public static List<Playlist> Playlists;
        public static int Count => Playlists.Count;
        public static bool HasNextPlaylist => CurrentPlaylistIndex != Playlists.Count - 1;
        public static bool HasPreviousPlaylist => CurrentPlaylistIndex != 0;

        /* Methods */
        public static void AddMediaItem(int _playlist, int _index, MediaItem _item)
        {
            if (Playlists[_playlist].Path != _item.Path)
            {
                Debug.LogError("MediaItem's UriType must be equal to the Playlist's UriType");
                return;
            }
            Playlists[_playlist].AddMediaItem(_index, _item);
        }

        public static void AddPlaylist(Playlist _list)
        {
            Playlists.Add(_list);
        }

        public static void RemovePlaylist(Playlist _list)
        {
            Playlists.Remove(_list);
        }

        public static void RemovePlaylist(int _id)
        {
            Playlists.RemoveAt(_id);
        }

        public static void RemoveLastPlaylist()
        {
            Playlists.RemoveAt(Count - 1);
        }

        public static void Clear()
        {
            Playlists.Clear();
        }

        public static void NextPlaylist()
        {
            if (HasNextPlaylist)
            {
                CurrentPlaylistIndex++;
            }
            else
            {
                CurrentPlaylistIndex = 0;
            }
            PreparePlaylist();
        }

        public static void PreviousPlaylist()
        {
            if (HasPreviousPlaylist)
            {
                CurrentPlaylistIndex--;
            }
            else
            {
                CurrentPlaylistIndex = Playlists.Count - 1;
            }
            PreparePlaylist();
        }

        public static void PreparePlaylist(int listId = -1, bool playWhenReady = false)
        {
            if (listId != -1)
            {
                if (CurrentPlaylistIndex == listId || Playlists.Count - 1 < listId) return;
                CurrentPlaylistIndex = listId;
            }
            Playlists[CurrentPlaylistIndex].UIEvent.Invoke();
            MediaPlayer.Prepare(playWhenReady, 0, Playlists[listId]);
        }

        public static void PrepareMediaItem(int itemId, int listId, bool playWhenReady)
        {
            if (listId != CurrentPlaylistIndex) MediaPlayer.Prepare(playWhenReady, itemId, CurrentPlaylist);
            MediaPlayer.Prepare(playWhenReady, itemId);
        }

        /* Init Variations */
        public static void Init(MediaItem _item, string _listenerId, bool _playWhenReady)
        {
            Playlists = new List<Playlist>();
            Playlists.Add(new Playlist(0, _item.Path));
            AddMediaItem(0, _item.Id, _item);
            MediaPlayer.Init(CurrentPlaylist, _listenerId, _playWhenReady);
        }

        public static void Init(Playlist _list, string _listenerId, bool _playWhenReady)
        {
            Playlists = new List<Playlist>();
            AddPlaylist(_list);
            MediaPlayer.Init(CurrentPlaylist, _listenerId, _playWhenReady);
        }

        public static void Init(List<Playlist> _lists, string _listenerId, bool _playWhenReady)
        {
            Playlists = _lists;
            MediaPlayer.Init(CurrentPlaylist, _listenerId, _playWhenReady);
        }
    }
}

