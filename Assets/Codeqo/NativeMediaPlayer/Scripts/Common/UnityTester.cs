#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Codeqo.NativeMediaPlayer;
using System.IO;

public class UnityTester : MonoBehaviour
{
    /* Player State Contstants */
    private const string ON_INIT = "ON_INIT";
    private const string ON_READY = "ON_READY";
    private const string ON_PREPARED = "ON_PREPARED";
    private const string ON_COMPLETE = "ON_COMPLETE";
    private const string ON_IS_PLAYING_CHANGED_TRUE = "ON_IS_PLAYING_CHANGED_TRUE";
    private const string ON_IS_PLAYING_CHANGED_FALSE = "ON_IS_PLAYING_CHANGED_FALSE";
    private const string ON_IS_LOADING_CHANGED_TRUE = "ON_IS_LOADING_CHANGED_TRUE";
    private const string ON_IS_LOADING_CHANGED_FALSE = "ON_IS_LOADING_CHANGED_FALSE";

    private AudioSource player;
    private AudioClip audioClip;
    private MediaEventListener mediaEventListener;

    public int currentMediaItemIndex { get; private set; }
    public int currentUriType => (int)playlist.Path;
    private int count => playlist.Count;
    private Playlist playlist;

    private bool _isPlaying = false;
    private bool _isLoading = false;
    private bool _playlistLoaded = false;

    private bool playWhenReady = false;
    private int repeatMode;
    private bool shuffleModeEnabled;
    private int timeSkipped;
    private int[] playOrder;

    public bool isPlaying
    {
        get
        {
            return _isPlaying;
        }
        set
        {
            _isPlaying = value;
            if (value) mediaEventListener.UnityReceiveMessage(ON_IS_PLAYING_CHANGED_TRUE);
            else mediaEventListener.UnityReceiveMessage(ON_IS_PLAYING_CHANGED_FALSE);
        }
    }

    public bool isLoading
    {
        get
        {
            return _isLoading;
        }
        set
        {
            _isLoading = value;
            if (value) mediaEventListener.UnityReceiveMessage(ON_IS_LOADING_CHANGED_TRUE);            
            else mediaEventListener.UnityReceiveMessage(ON_IS_LOADING_CHANGED_FALSE);
        }
    }

    public bool hasPreviousMediaItem
    {
        get
        {
            if (currentMediaItemIndex == 0)
                return repeatMode == RepeatMode.RepeatAll;
            return true;
        }
    }

    public bool hasNextMediaItem
    {
        get
        {
            if (currentMediaItemIndex == count - 1)
                return repeatMode == RepeatMode.RepeatAll;
            return true;
        }
    }

    /* Auto-save feature not available yet for tester */
    public static UnityTester Create(GameObject _go, string _listener, bool _playWhenReady, Playlist _list)
    {
        UnityTester unityTester = _go.AddComponent<UnityTester>();
        unityTester.mediaEventListener = GameObject.Find(_listener).GetComponent<MediaEventListener>();
        unityTester.player = _go.AddComponent<AudioSource>();
        unityTester.playWhenReady = _playWhenReady;
        unityTester.timeSkipped = PlayerPrefs.GetInt("SKIP_TIME", 10);
        unityTester.prepare(_playWhenReady, PlayerPrefs.GetInt("START_INDEX", 0), _list);
        return unityTester;
    }

    public void prepare(bool _playWhenReady = false, int _id = -1, Playlist _list = null)
    {
        if (isLoading) return;
        if (isPlaying) stop();

        if (_list != null)
        {
            playlist = _list;
            playOrder = getIntegerArray(_list.MediaItems.Count);
            _playlistLoaded = false;
            currentMediaItemIndex = 0;

            if (MediaPlayer.isInit)
            {
                setShuffleModeEnabled(false);
            }
        }

        if (_id != -1) currentMediaItemIndex = _id;
        StartCoroutine(_prepare(_playWhenReady));
    }

    private void release()
    {
        player = null;
        player = MediaPlayer._tester.gameObject.AddComponent<AudioSource>();
    }

    public void destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator _prepare(bool _playWhenReady)
    {
        yield return new WaitUntil(() => playlist != null && !isLoading);

        if (count == 0)
        {
            Debug.LogError("Playlist is empty. Please add a MediaItem and re-initiate the Native Media Player");
            yield break;
        }

        player.Stop();

        if (_playWhenReady)
        {
            yield return StartCoroutine(getFile());
            player.Play();
            isPlaying = true;
            StartCoroutine(onCompletionListener());
        }
        else
        {
            StartCoroutine(getFile());
        }
    }

    public void play()
    {
        if (isLoading) return;

        if (player.time == 0)
        {
            if (player.clip.loadState == AudioDataLoadState.Loaded)
            {
                player.Play();
                isPlaying = true;
                StartCoroutine(onCompletionListener());
                Debug.Log("UnityTester: Play");
            }
            else
            {
                prepare(true);
                Debug.Log("UnityTester: PlayOnPrepared");
            }
        }
        else
        {
            player.UnPause();
            isPlaying = true;
            Debug.Log("UnityTester: UnPause");
        }
    }

    public void stop()
    {
        if (isLoading) return;
        if (player != null)
        {
            player.Stop();
            player.time = 0;
            isPlaying = false;
        }
    }

    public void pause()
    {
        if (isLoading) return;
        player.Pause();
        isPlaying = false;
    }

    public void previous()
    {
        if (isLoading) return;
        if (player.time > 1)
        {
            player.time = 0; 
            return;
        }
        if (currentMediaItemIndex != 0)
        {
            currentMediaItemIndex--;
        }
        else
        {
            if (repeatMode != RepeatMode.RepeatAll) return;
            currentMediaItemIndex = count - 1;
        }
        prepare(isPlaying);
    }

    public void next()
    {
        if (isLoading) return;
        if (currentMediaItemIndex != count - 1)
        {
            currentMediaItemIndex++;
        }
        else
        {
            if (repeatMode != RepeatMode.RepeatAll) return;
            currentMediaItemIndex = 0;
        }
        prepare(isPlaying);
    }

    public void seekTo(float _time)
    {
        if (isLoading) return;
        player.time = _time;
    }

    public void rewind()
    {
        if (isLoading) return;
        if (player.time - timeSkipped > 0) player.time -= timeSkipped;
        else player.time = 0;
    }

    public void fastForward()
    {
        if (isLoading) return;
        if (player.time + timeSkipped < player.clip.length) player.time += timeSkipped;
        else next();
    }

    IEnumerator onCompletionListener()
    {
        Debug.Log("Unity Tester OnCompletionListener has started.");
        yield return new WaitUntil(() =>
        {
            float timeLeft = player.clip.length - player.time;
            return timeLeft <= 0;
        });

        mediaEventListener.UnityReceiveMessage(ON_COMPLETE);

        switch (repeatMode)
        {
            case RepeatMode.RepeatOne:
                play();
                break;
            case RepeatMode.RepeatAll:
                next();
                break;
            default:
                if (isLastMediaItem())
                {
                    stop();
                    currentMediaItemIndex = syncIndex(playOrder[0]);
                    prepare(false);
                } 
                else
                {
                    next();
                }
                break;
        }
    }

    private bool isLastMediaItem()
    {
        return currentMediaItemIndex == count - 1;
    }

    public void setVolume(float _volume)
    {
        player.volume = _volume;
    }

    public void setRepeatMode(int _repeat)
    {
        repeatMode = _repeat;
        RepeatMode.UpdateUI();
    }

    public void setShuffleModeEnabled(bool _shuffle)
    {
        shuffleModeEnabled = _shuffle;

        int current = playOrder[currentMediaItemIndex];

        if (_shuffle)
        {
            for (int i = 0; i < playOrder.Length; i++)
            {
                int rnd = Random.Range(0, playOrder.Length);
                int temp = playOrder[rnd];
                playOrder[rnd] = playOrder[i];
                playOrder[i] = temp;
            }
        }
        else
        {
            playOrder = getIntegerArray(count);
        }

        currentMediaItemIndex = syncIndex(current);
        ShuffleMode.UpdateUI();
    }

    private int syncIndex(int current)
    {
        int valueTo = 0;

        for (int i = 0; i < playOrder.Length; i++)
        {
            if (current == playOrder[i])
            {
                valueTo = i;
            }
        }

        return valueTo;
    }

    private int[] getIntegerArray(int count)
    {
        int[] array = new int[count];
        for (int i = 0; i < count; i++)
        {
            array[i] = i;
        }
        return array;
    }

    public int[] getShuffleOrder()
    {
        return playOrder;
    }

    public bool getShuffleModeEnabled()
    {
        return shuffleModeEnabled;
    }

    public float getVolume()
    {
        if (player != null) return player.volume;
        return 1;
    }

    public int getRepeatMode()
    {
        return repeatMode;
    }

    public int getCurrentMediaItemIndex()
    {
        return playOrder[currentMediaItemIndex];
    }

    public float getCurrentDuration()
    {
        if (player != null && !isLoading)
        {
            return player.clip.length;
        }
        return 0;
    }

    public float getCurrentPosition()
    {
        if (player != null && !isLoading)
        {
            return player.time;
        }
        return 0;
    }

    IEnumerator getFile()
    {
        int _id = getCurrentMediaItemIndex();

        if (playlist.MediaItems[_id] == null)
        {
            onError("MediaItem#" + _id + " is null");
            yield break;
        }

        if (playlist.MediaItems[_id].MediaUri == null)
        {
            onError("Player could not load the source. Invalid media location.");
            release();
            yield break;
        }

        isLoading = true;
        Debug.Log(playlist.MediaItems[_id].MediaUri);
        AudioType audioType;

        string path = playlist.MediaItems[_id].MediaUri;
        string extension = Path.GetExtension(path);

        switch (extension)
        {
            case ".ogg": audioType = AudioType.OGGVORBIS; break;
            case ".mp3": audioType = AudioType.MPEG; break;
            case ".wav": audioType = AudioType.WAV; break;
            default: audioType = AudioType.MPEG; break;
        }

        if (playlist.Path == UriType.StreamingAssets)
        {
            path = "file://" + System.IO.Path.Combine(Application.streamingAssetsPath, path);
        }

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                isLoading = false;
                release();
                string error = "There was an error while loading this file.\nMake sure you have audio files under Assets/StreamingAssets/ folder if you are using local files";
                error += "\n" + www.error;
                onError(error);
            }
            else
            {
                audioClip = DownloadHandlerAudioClip.GetContent(www);
                player.time = 0;
                player.clip = audioClip;
                yield return new WaitUntil(() => player.clip.loadState == AudioDataLoadState.Loaded);
                isLoading = false;
                onPrepared();
                Debug.Log("Unity Tester Ready");
            }
        }
    }

    public void reload()
    {
        _playlistLoaded = false;
        prepare(playWhenReady, currentMediaItemIndex);
    }

    private void onPrepared()
    {
        if (!_playlistLoaded)
        {
            mediaEventListener.UnityReceiveMessage(ON_INIT);
            mediaEventListener.UnityReceiveMessage(ON_READY);
            _playlistLoaded = true;
        }
        mediaEventListener.UnityReceiveMessage(ON_PREPARED);
        if (playWhenReady) play();
    }

    private void onError(string _error)
    {
        Debug.LogError(_error);
        mediaEventListener.UnityReceiveError(_error);
    }

    public void addMediaItem(int _id, MediaItem _item)
    {
        playlist.AddMediaItem(_id, _item);
        setShuffleModeEnabled(shuffleModeEnabled);
    }

    public void removeMediaItem(int _id)
    {
        playlist.RemoveMediaItem(_id);
        setShuffleModeEnabled(shuffleModeEnabled);
    }
}
#endif