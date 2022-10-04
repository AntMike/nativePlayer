using System.Collections.Generic;
using UnityEngine;

namespace Codeqo.NativeMediaPlayer
{
    public enum InitWith : short
    {
        MediaItem = 0,
        Playlist = 1,
        Playlists = 2
    }

    public class MediaSource : MonoBehaviour
    {
        public InitWith initWith = InitWith.Playlist;
        public UnityMediaItem defaultMediaItem;
        public UnityPlaylist defaultPlaylist;
        public List<UnityPlaylist> defaultPlaylists;
    }
}

