using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public float duration;
    public List<Transform> transforms;
    private List<float> destinations;
    private bool inAction;
    private Action onCompletion;
    private int index = 0;
    float frame = 0.03f;

    public void SlideToRight()
    {
        if (index == 1) return;
        index = 1;
        StartCoroutine(IEAction(false));
    }

    public void SlideToLeft()
    {
        if (index == -1) return;
        index = -1;
        StartCoroutine(IEAction(true));
    }

    public Slide SlideToRightExt()
    {
        if (index == 1) return this;
        index = 1;
        StartCoroutine(IEAction(false));
        return this;
    }

    public Slide SlideToLeftExt()
    {
        if (index == -1) return this;
        index = -1;
        StartCoroutine(IEAction(true));
        return this;
    }

    public void OnCompletion(Action action)
    {
        onCompletion = action;
    }

    IEnumerator IEAction(bool slideToLeft)
    {
        yield return new WaitUntil(() => !inAction);

        inAction = true;
        destinations = new List<float>();

        float timeElapsed = 0f;
        float moveAmount = Screen.width;

        /* Lerp starts */

        while (timeElapsed < duration)
        {
            float lerpTime = timeElapsed / duration;
            lerpTime = lerpTime * lerpTime * (3f - 2f * lerpTime);

            if (!slideToLeft)
            {
                for (int i = 0; i < transforms.Count; i++)
                {
                    float x = transforms[i].position.x;
                    destinations.Add(x + moveAmount);

                    if (!transforms[i].gameObject.GetComponent<CanvasGroup>()) transforms[i].gameObject.AddComponent<CanvasGroup>();

                    float xLerp = Mathf.Lerp(x, destinations[i], lerpTime);
                    transforms[i].position = new Vector2(xLerp, transforms[i].position.y);
                }
            }
            else
            {
                for (int i = 0; i < transforms.Count; i++)
                {
                    float x = transforms[i].position.x;
                    destinations.Add(x - moveAmount);

                    if (!transforms[i].gameObject.GetComponent<CanvasGroup>()) transforms[i].gameObject.AddComponent<CanvasGroup>();

                    float xLerp = Mathf.Lerp(x, destinations[i], lerpTime);
                    transforms[i].position = new Vector2(xLerp, transforms[i].position.y);
                }
            }

            timeElapsed += Time.deltaTime + frame;
            yield return new WaitForSeconds(frame);
        }

        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].position = new Vector2(destinations[i], transforms[i].position.y);
        }

        onCompletion?.Invoke();
        inAction = false;
    }
}
