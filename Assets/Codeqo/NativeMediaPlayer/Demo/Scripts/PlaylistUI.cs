using Codeqo.NativeMediaPlayer;
using UnityEngine;
using UnityEngine.UI;

public class PlaylistUI : MonoBehaviour
{
    public int PlaylistId;

    [SerializeField] Text title;
    [SerializeField] Text artist;

    private Playlist GetPlaylist()
    {
        return MediaController.Playlists[PlaylistId];
    }

    public void Create(int _playlistId)
    {
        PlaylistId = _playlistId;
        MediaEvents.OnReady.AddListener(Init);
    }

    private void Init()
    {
        if (GetPlaylist() == null) return;
        if (title != null) title.text = GetPlaylist().Title;
        if (artist != null) artist.text = GetPlaylist().Artist;
    }
}
