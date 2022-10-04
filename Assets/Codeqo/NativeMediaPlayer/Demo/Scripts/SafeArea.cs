using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public float topMarginAndroid = 0f;
    public float topMarginIphone = 100f;

    void Awake()
    {
        RectTransform rect = this.GetComponent<RectTransform>();
#if UNITY_ANDROID
        rect.offsetMax = new Vector2(rect.offsetMax.x, rect.offsetMax.y - topMarginAndroid);
#elif UNITY_IPHONE
        rect.offsetMax = new Vector2(rect.offsetMax.x, rect.offsetMax.y - topMarginIphone);
#endif
    }
}
