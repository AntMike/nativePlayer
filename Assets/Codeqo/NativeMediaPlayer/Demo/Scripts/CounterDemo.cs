using Codeqo.NativeMediaPlayer;
using UnityEngine;
using UnityEngine.UI;

public class CounterDemo : MonoBehaviour
{
    private void Awake()
    {
        MediaEvents.OnPrepared.AddListener(Display);
    }

    private void OnDestroy()
    {
        MediaEvents.OnPrepared.RemoveListener(Display);
    }

    private void Display()
    {
        GetComponent<Text>().text = (MediaPlayer.CurrentMediaItemIndex + 1) + "/" + MediaPlayer.CurrentPlaylist.MediaItems.Count;
    }
}
