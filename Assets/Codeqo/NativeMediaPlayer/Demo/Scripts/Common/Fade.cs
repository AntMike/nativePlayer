using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] CanvasGroup objToFade;

    public void FadeIn()
    {
        StartCoroutine(IEAction(true));
    }

    public void FadeOut()
    {
        StartCoroutine(IEAction(false));
    }

    IEnumerator IEAction(bool fadeIn)
    {
        float lerpDur = duration;
        float timeElapsed = 0f;
        objToFade.interactable = false;
        objToFade.blocksRaycasts = false;

        /* Lerp starts */

        while (timeElapsed < lerpDur)
        {
            float lerpTime = timeElapsed / lerpDur;
            lerpTime = lerpTime * lerpTime * (3f - 2f * lerpTime);

            if (fadeIn)
            {
                objToFade.alpha = Mathf.Lerp(0f, 1f, lerpTime);

            }
            else
            {
                objToFade.alpha = Mathf.Lerp(1f, 0f, lerpTime);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (fadeIn)
        {
            objToFade.alpha = 1;
            objToFade.interactable = true;
            objToFade.blocksRaycasts = true;
        }
        else
        {
            objToFade.alpha = 0;
        }
    }
}
