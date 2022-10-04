using UnityEngine;
using UnityEngine.UI;
using Codeqo.NativeMediaPlayer;

public class MetadataDisplayDemo : MonoBehaviour
{
    [SerializeField] Text trackTitle;
    [SerializeField] Text trackArtist;
    [SerializeField] Text genre;
    [SerializeField] Text releaseDate;
    [SerializeField] Text albumTitle;
    [SerializeField] Text albumArtist;
    [SerializeField] Text metadataState;
    [SerializeField] Text uriType;

    private void Awake()
    {
        MediaEvents.OnPrepared.AddListener(UpdateMetadata);
    }

    private void OnDestroy()
    {
        MediaEvents.OnPrepared.RemoveListener(UpdateMetadata);
    }

    public void UpdateMetadata()
    {
        if (!metadataState || !trackTitle || !trackArtist || !genre || !releaseDate || !albumTitle || !albumArtist || !uriType)
        {
            Debug.LogError("Demo metadata display object not found");
            return;
        }

        int _id = MediaPlayer.CurrentMediaItemIndex;
        if (MediaPlayer.CurrentPlaylist.MediaItems[_id] == null) return;
        if (MediaPlayer.CurrentPlaylist.MediaItems[_id].HasCustomMediaMetadata) metadataState.text = "Custom";
        else metadataState.text = "Retrieved";
        trackTitle.text = MediaPlayer.CurrentPlaylist.MediaItems[_id].Title;
        trackArtist.text = MediaPlayer.CurrentPlaylist.MediaItems[_id].Artist;
        genre.text = MediaPlayer.CurrentPlaylist.MediaItems[_id].Genre;
        releaseDate.text = MediaPlayer.CurrentPlaylist.MediaItems[_id].ReleaseDate;
        albumTitle.text = MediaPlayer.CurrentPlaylist.MediaItems[_id].AlbumTitle;
        albumArtist.text = MediaPlayer.CurrentPlaylist.MediaItems[_id].AlbumArtist;
        uriType.text = MediaPlayer.CurrentPlaylist.Path == UriType.StreamingAssets ? "Streaming Assets" : "Remote Url";
    }
}
