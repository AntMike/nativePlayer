using Codeqo.NativeMediaPlayer;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArtworkLoaderDemo : MonoBehaviour
{
    [SerializeField] Image albumArt;
    [SerializeField] RectTransform loadingRect;
    [SerializeField] Text albumArtText;
    bool loading;
    UnityAction action;
    float frame = 0.03f;

    private void Awake()
    {
        action = () => {
            StartCoroutine(FloatLerp(true));
        };
        MediaEvents.OnPrepared.AddListener(action);
    }

    private void OnDestroy()
    {
        MediaEvents.OnPrepared.RemoveListener(action);
    }

    IEnumerator FloatLerp(bool start)
    {
        float lerpDur = .5f;
        float timeElapsed = 0f;
        int _id = MediaPlayer.CurrentMediaItemIndex;

        bool newArt = false;
        GameObject newArtObj = null;
        Image newArtImg = null;

        /* Pre-lerp */
        if (start)
        {
            loading = true;
            StartCoroutine(RotateCircle());
        }
        else
        {
            if (MediaPlayer.CurrentPlaylist.MediaItems[_id].Artwork != null)
            {
                albumArtText.text = "";
                newArt = true;
                newArtObj = Instantiate(albumArt.gameObject, albumArt.transform.parent);
                newArtImg = newArtObj.GetComponent<Image>();
                newArtObj.transform.SetAsFirstSibling();
                newArtImg.sprite = MediaPlayer.CurrentPlaylist.MediaItems[_id].Artwork;
            }
            else
            {
                albumArtText.text = "NO ARTWORK";
                albumArt.sprite = null;
            }
        }

        /* Lerp starts */

        while (timeElapsed < lerpDur)
        {
            float lerpTime = timeElapsed / lerpDur;
            lerpTime = lerpTime * lerpTime * (3f - 2f * lerpTime);

            if (start)
            {
                /* Player starts loading a media */
                if (!(loadingRect.GetComponent<CanvasGroup>().alpha == 1))
                    loadingRect.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, lerpTime);
            }
            else
            {
                /* Player finishes loading a media */
                if (newArt)
                {
                    if (!(albumArt.GetComponent<CanvasGroup>().alpha == 0))
                        albumArt.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, lerpTime);
                }

                if (!(loadingRect.GetComponent<CanvasGroup>().alpha == 0))
                    loadingRect.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, lerpTime);
            }

            timeElapsed += Time.deltaTime + frame;
            yield return new WaitForSeconds(frame);
        }


        /* Post-lerp */
        if (start)
        {
            loadingRect.GetComponent<CanvasGroup>().alpha = 1;
            StartCoroutine(FloatLerp(false));
        }
        else
        {
            if (newArtObj != null)
            {
                Destroy(albumArt.gameObject);
                albumArt = newArtImg;
                newArtObj.GetComponent<CanvasGroup>().alpha = 1;
            }
            loadingRect.GetComponent<CanvasGroup>().alpha = 0;
            loading = false;
        }
    }

    IEnumerator RotateCircle()
    {
        while (loading)
        {
            loadingRect.Rotate(new Vector3(0, 0, -0.5f));
            yield return new WaitForSeconds(frame);
        }
        loadingRect.localRotation = new Quaternion(0, 0, 0, 0);
        loadingRect.GetComponent<CanvasGroup>().alpha = 0;
    }
}
