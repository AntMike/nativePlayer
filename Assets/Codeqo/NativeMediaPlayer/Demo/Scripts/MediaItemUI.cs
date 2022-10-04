using Codeqo.NativeMediaPlayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MediaItemUI : MonoBehaviour
{
    public static UnityEvent DeselectEvent = new UnityEvent();

    public int PlaylistId;
    public int MediaItemId;

    [SerializeField] Text albumTitle;
    [SerializeField] Text albumArtist;

    [SerializeField] Text title;
    [SerializeField] Text artist;
    [SerializeField] Text genre;
    [SerializeField] Text releaseDate;
    [SerializeField] Image artwork;
    [SerializeField] Sprite noImage;

    [SerializeField] Sprite play, pause;
    [SerializeField] Image icon;
    [SerializeField] Button playBtn;
    [SerializeField] Button selectBtn;
    [SerializeField] Image select;

    public UnityEvent playEvent = new UnityEvent();
    public UnityEvent selectEvent = new UnityEvent();

    bool selected = false;
    bool isPlaying = false;
    bool isPrepared = false;

    private MediaItem GetMediaItem()
    {
        return MediaController.Playlists[PlaylistId].MediaItems[MediaItemId];
    }

    private void OnDestroy()
    {
        MediaEvents.OnRetrieved.RemoveListener(UpdateMediaMetadata);
        MediaEvents.OnPrepared.RemoveListener(UpdateSelection);
    }

    public void Create(int _playlistId, int _mediaItemId)
    {
        PlaylistId = _playlistId;
        MediaItemId = _mediaItemId;
        MediaEvents.OnRetrieved.AddListener(UpdateMediaMetadata);
        MediaEvents.OnPrepared.AddListener(UpdateSelection);
        DeselectEvent.AddListener(Deselect);

        if (playBtn != null)
        {
            playEvent.AddListener(() => {

                if (playBtn == null || icon == null || play == null || pause == null || select == null)
                {
                    Debug.LogError("Some components are missing");
                    return;
                }

                if (isPrepared)
                {
                    if (isPlaying)
                    {
                        MediaPlayer.Pause();
                        Pause();
                    }
                    else
                    {
                        MediaPlayer.Play();
                        Play();
                    }
                }
                else
                {
                    Prepare();
                    Select();
                }
            });

            playBtn.onClick.AddListener(playEvent.Invoke);
        }

        if (selectBtn != null) selectBtn.onClick.AddListener(SelectEvent);
    }

    private void UpdateMediaMetadata()
    {
        if (GetMediaItem() == null) return;
        if (albumTitle != null) albumTitle.text = GetMediaItem().AlbumTitle;
        if (albumArtist != null) albumArtist.text = GetMediaItem().AlbumArtist;
        if (title != null) title.text = GetMediaItem().Title;
        if (artist != null) artist.text = GetMediaItem().Artist;
        if (genre != null) genre.text = GetMediaItem().Genre;
        if (releaseDate != null) releaseDate.text = GetMediaItem().ReleaseDate;
        if (artwork != null)
        {
            Sprite temp = GetMediaItem().Artwork;
            if (temp == null && noImage != null) artwork.sprite = noImage;
            else artwork.sprite = temp;
        }
    }

    private void UpdateSelection()
    {
        if (playBtn == null || icon == null || play == null || pause == null || select == null)
        {
            Debug.LogError("Some components are missing");
            return;
        }

        if (MediaController.CurrentPlaylistIndex == PlaylistId && MediaPlayer.CurrentMediaItemIndex == MediaItemId)
        {
            Select();

            if (isPlaying)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }
        else
        {
            Deselect();
            Pause();
            isPrepared = false;
        }
    }

    private void SelectEvent()
    {
        if (!selected)
        {
            Select();
        }
        else
        {
            if (!isPlaying) Prepare();
            selectEvent.Invoke();
        }
    }

    private void Prepare()
    {
        MediaController.PrepareMediaItem(MediaItemId, PlaylistId, true);
        isPrepared = true;
        Play();
    }

    private void Play()
    {
        isPlaying = true;
        icon.sprite = pause;
    }

    private void Pause()
    {
        isPlaying = false;
        icon.sprite = play;
    }

    private void Select()
    {
        DeselectEvent.Invoke();
        selected = true;
        select.color = new Color(select.color.r, select.color.g, select.color.b, 1f);
    }

    private void Deselect()
    {
        select.color = new Color(select.color.r, select.color.g, select.color.b, 0f);
        selected = false;
    }
}
