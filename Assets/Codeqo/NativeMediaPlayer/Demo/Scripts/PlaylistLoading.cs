using Codeqo.NativeMediaPlayer;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlaylistLoading : MonoBehaviour
{
    CanvasGroup folder;
    RectTransform circle;
    CanvasGroup circleC => circle.GetComponent<CanvasGroup>();
    bool loading;
    static UnityAction action;

    public static void Attach (GameObject where, int _id, CanvasGroup _folder, RectTransform _circle)
    {
        PlaylistLoading builder = where.AddComponent<PlaylistLoading>();
        builder.folder = _folder;
        builder.circle = _circle;
        if (_id == 0)
        {
            PlaylistSelector.OnSelectLocal.AddListener(builder.StartLoading);
            builder.folder.alpha = 0;
            builder.circleC.alpha = 0;
        }
        if (_id == 1) PlaylistSelector.OnSelectRemote.AddListener(builder.StartLoading);
        action = builder.EndLoading;
        MediaEvents.OnRetrieved.AddListener(action);
    }

    private void OnDestroy()
    {
        MediaEvents.OnRetrieved.RemoveListener(action);
    }

    public void StartLoading()
    {
        folder.alpha = 1;
        circleC.alpha = 1;
        folder.blocksRaycasts = true;
        folder.interactable = true;
        circleC.blocksRaycasts = true;
        circleC.interactable = true;
        loading = true;
        StartCoroutine(RotateCircle());
    }

    public void EndLoading()
    {
        if (!loading) return;
        StartCoroutine(FloatLerp());
    }

    IEnumerator FloatLerp()
    {
        float lerpDur = 1.5f;
        float timeElapsed = 0f;

        /* Lerp starts */
        while (timeElapsed < lerpDur)
        {
            float lerpTime = timeElapsed / lerpDur;
            lerpTime = lerpTime * lerpTime * (3f - 2f * lerpTime);
            float lerpValue = Mathf.Lerp(1f, 0f, lerpTime);
            folder.alpha = lerpValue;
            circleC.alpha = lerpValue;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        folder.blocksRaycasts = false;
        folder.interactable = false;
        circleC.blocksRaycasts = false;
        circleC.interactable = false;
        loading = false;
    }

    IEnumerator RotateCircle()
    {
        while (loading)
        {
            circle.Rotate(new Vector3(0, 0, -1f));
            yield return null;
        }
        circle.localRotation = new Quaternion(0, 0, 0, 0);
    }
}
