using Codeqo.NativeMediaPlayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    [SerializeField] Text player;
    UnityAction action;

    private void OnEnable()
    {
        action = () => { player.text = MediaEvents.GetPlayerName(); };
        MediaEvents.OnPrepared.AddListener(action);
    }

    private void OnDestroy()
    {
        MediaEvents.OnPrepared.RemoveListener(action);
    }
}
