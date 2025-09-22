using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool WasPressed { get; private set; }
    public bool IsHeld { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        WasPressed = true;
        IsHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsHeld = false;
    }

    private void LateUpdate()
    {
        WasPressed = false;
    }
}
