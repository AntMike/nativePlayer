using UnityEngine;
using UnityEngine.UI;
using Codeqo.NativeMediaPlayer;
using System.Collections;

public class PluginStateDisplay : MonoBehaviour
{
    private Color pluginStateBlue = new Color(66 / 255f, 133 / 255f, 243 / 255f); //Ended
    private Color pluginStateRed = new Color(234 / 255f, 67 / 255f, 53 / 255f); //Idle
    private Color pluginStateYellow = new Color(251 / 255f, 188 / 255f, 5 / 255f); //Buffering
    private Color pluginStateGreen = new Color(52 / 255f, 168 / 255f, 83 / 255f); //Ready

    [SerializeField] Image coloredObj;
    [SerializeField] Text stateTxt;
    [SerializeField] GameObject error;
    [SerializeField] Image errorColoredObj;
    [SerializeField] Text errorTxt;
    private bool buffering;
    private bool loading;
    private int bufferingCount;

    private void Awake()
    {
        MediaEvents.OnInit.AddListener(() => {
            MediaEvents.OnPrepared.AddListener(OnPlaybackStateReady);
            MediaEvents.OnIsLoadingChangedTrue.AddListener(() =>
            {
                loading = true;
                OnPlaybackStateLoading();
            });
            MediaEvents.OnIsLoadingChangedFalse.AddListener(() => {
                loading = false;
                if (buffering) OnPlaybackStateBuffering();
                else OnPlaybackStateReady();
            });
            MediaEvents.OnIsBufferingChangedTrue.AddListener(() => {
                buffering = true;
                OnPlaybackStateBuffering();
            });
            MediaEvents.OnIsBufferingChangedFalse.AddListener(() => {
                buffering = false;
                if (loading) OnPlaybackStateLoading();
                else OnPlaybackStateReady();
            });
            MediaEvents.OnComplete.AddListener(OnPlaybackStateEnded);
        }); 
    }

    public void OnPlaybackStateLoading()
    {
        coloredObj.color = pluginStateRed;
        stateTxt.text = "LOADING";
    }

    public void OnPlaybackStateBuffering()
    {
        coloredObj.color = pluginStateYellow;
        stateTxt.text = "BUFFERING";
        bufferingCount = 0;
        StartCoroutine(StartBuffering());
    }

    IEnumerator StartBuffering()
    {
        while (buffering)
        {
            if (bufferingCount < 4)
            {
                bufferingCount++;
                stateTxt.text += ".";
            }
            else
            {
                bufferingCount = 0;
                stateTxt.text.Replace("...", "");
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnPlaybackStateReady()
    {
        coloredObj.color = pluginStateBlue;
        stateTxt.text = "READY";
     }
    public void OnPlaybackStateEnded()
    {
        coloredObj.color = pluginStateGreen;
        stateTxt.text = "ENDED";
    }

    public void OnPlayerError()
    {
        error.SetActive(true);
        errorColoredObj.color = pluginStateRed;
        errorTxt.text = MediaPlayer.Error;
        StartCoroutine(WaitAndSetActiveFalse(error, 10f));
    }

    IEnumerator WaitAndSetActiveFalse(GameObject _obj, float _value)
    {
        yield return new WaitForSeconds(_value);
        _obj.SetActive(false);
    }
}
