using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    private Vector2 _input = Vector2.zero;
    private float _radius;

    public float Horizontal => _input.x;
    public float Vertical => _input.y;
    public Vector2 Direction => new Vector2(Horizontal, Vertical);

    private void Start()
    {
        _radius = background.sizeDelta.x * 0.5f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos = pos / _radius;
            _input = (pos.magnitude > 1) ? pos.normalized : pos;

            handle.anchoredPosition = new Vector2(_input.x * _radius, _input.y * _radius);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
