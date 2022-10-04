using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Codeqo.NativeMediaPlayer.UI;

public class DemoButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] UnityEvent toggleTrue = new UnityEvent();
    [SerializeField] UnityEvent toggleFalse = new UnityEvent();
    public bool isInteractable = true;
    bool isToggle;
    bool pointerDown;

    Image image => this.GetComponent<Image>();

    public void Toggle()
    {
        if (isToggle) // button is pressed
        {
            isToggle = false;
            image.color = ButtonColor.Disabled.LightMode;
            if (toggleFalse.GetPersistentEventCount() > 0) toggleFalse.Invoke();
        }
        else
        {
            isToggle = true;
            image.color = ButtonColor.Enabled.LightMode;
            if (toggleTrue.GetPersistentEventCount() > 0) toggleTrue.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!pointerDown && isInteractable)
        {
            Toggle();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isInteractable)
        {
            pointerDown = true;
            image.color = ButtonColor.Disabled.LightMode;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInteractable)
        {
            pointerDown = false;
            image.color = ButtonColor.Enabled.LightMode;
        }
    }
}
