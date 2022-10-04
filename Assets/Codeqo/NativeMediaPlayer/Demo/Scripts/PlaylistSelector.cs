using Codeqo.NativeMediaPlayer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlaylistSelector : MonoBehaviour
{
    public static UnityEvent OnSelectLocal = new UnityEvent();
    public static UnityEvent OnSelectRemote = new UnityEvent();
    [SerializeField] Image local, remote;
    [SerializeField] Slide slide;
    Color color = new Color(241 / 255f, 243 / 255f, 244 / 255f);

    private void Awake()
    {
        MediaEvents.OnReady.AddListener(() => {

            MediaController.Playlists[0].UIEvent.AddListener(() => {
                local.color = color;
                remote.color = Color.white;
            });

            if (MediaController.Playlists.Count > 1)
            {
                MediaController.Playlists[1].UIEvent.AddListener(() =>
                {
                    local.color = Color.white;
                    remote.color = color;
                });
            }
        });
    }

    public void SelectLocal()
    {
        if (MediaController.CurrentPlaylistIndex == 0) return;
        OnSelectLocal.Invoke();
        slide.SlideToRightExt().OnCompletion(() => MediaController.PreparePlaylist(0, false));
    }

    public void SelectRemote()
    {
        if (MediaController.CurrentPlaylistIndex == 1) return;
        OnSelectRemote.Invoke();
        slide.SlideToLeftExt().OnCompletion(() => MediaController.PreparePlaylist(1, false));
    }
}
