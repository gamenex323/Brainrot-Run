using UnityEngine;
using UnityEngine.EventSystems;

public class TapButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("True = Left Button, False = Right Button")]
    public bool isLeft;

    public delegate void ButtonClickedEvent(bool isLeft, bool isPressed);
    public static event ButtonClickedEvent btnClickedEvent;

    // Called when finger/mouse button is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        btnClickedEvent?.Invoke(isLeft, true);
    }

    // Called when finger/mouse button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        btnClickedEvent?.Invoke(isLeft, false);
    }
}
