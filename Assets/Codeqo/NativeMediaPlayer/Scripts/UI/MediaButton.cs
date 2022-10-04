using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Codeqo.NativeMediaPlayer;
using Codeqo.NativeMediaPlayer.UI;

public class MediaButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image image;
    [SerializeField] Sprite[] sprites;
    [SerializeField] UnityEvent mediaEvent = new UnityEvent();
    [SerializeField] UnityEvent extraEvent = new UnityEvent();
    [SerializeField] bool isToggle;
    [SerializeField] bool disableOnCondition;
    public ButtonType buttonType;
    public bool isInteractable;
    public bool darkMode;
    UnityEvent onPauseEvent = new UnityEvent();
    UnityEvent onDestroyEvent = new UnityEvent();
    bool pointerDown;
 
    private void Awake()
    {
        ButtonEvent.AddEvent(buttonType, image, sprites, mediaEvent, onPauseEvent, onDestroyEvent, ToggleInteractable());

        MediaEvents.OnIsLoadingChangedFalse.AddListener(() => {
            isInteractable = true;
        });

        MediaEvents.OnIsLoadingChangedTrue.AddListener(() => {
            isInteractable = false;
        });
    }

    public UnityAction ToggleInteractable()
    {
        return () => {
            if (disableOnCondition && ButtonEvent.GetDisableCondition(buttonType))
            {
                image.color = darkMode? ButtonColor.Disabled.DarkMode : ButtonColor.Disabled.LightMode;
                isInteractable = false;
            }
            else
            {
                image.color = darkMode ? ButtonColor.Enabled.DarkMode : ButtonColor.Enabled.LightMode;
                isInteractable = true;
            }
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!pointerDown && isInteractable)
        {
            mediaEvent.Invoke();
            if (extraEvent.GetPersistentEventCount() > 0) extraEvent.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isInteractable)
        {
            pointerDown = true;
            image.color = darkMode ? ButtonColor.Disabled.DarkMode : ButtonColor.Disabled.LightMode;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInteractable)
        {
            pointerDown = false;
            image.color = darkMode ? ButtonColor.Enabled.DarkMode : ButtonColor.Enabled.LightMode;
        }
    }

    void OnDestroy()
    {
        onDestroyEvent.Invoke();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus && isToggle && MediaPlayer.isInit)
        {
            onPauseEvent.Invoke();
        }
    }
}
