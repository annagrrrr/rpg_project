using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Button Settings")]
    [SerializeField] private bool showVisualFeedback = true;
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color normalColor = Color.white;

    
    public bool WasPressed { get; private set; }
    public bool IsHeld { get; private set; }

    
    private Image _buttonImage;
    private Button _buttonComponent;

    private void Start()
    {
        
        _buttonImage = GetComponent<Image>();
        _buttonComponent = GetComponent<Button>();

        
        if (_buttonImage != null && showVisualFeedback)
        {
            _buttonImage.color = normalColor;
        }

        Debug.Log($"✅ MobileButton initialized: {gameObject.name}");
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (_buttonComponent != null && !_buttonComponent.interactable)
            return;

        WasPressed = true;
        IsHeld = true;

        
        if (showVisualFeedback && _buttonImage != null)
        {
            _buttonImage.color = pressedColor;
        }

        Debug.Log($"🔘 {gameObject.name} pressed down");
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
        IsHeld = false;

        
        if (showVisualFeedback && _buttonImage != null)
        {
            _buttonImage.color = normalColor;
        }

        Debug.Log($"🔘 {gameObject.name} released");
    }

    
    private void LateUpdate()
    {
        WasPressed = false;
    }

  
    public void SetInteractable(bool interactable)
    {
        if (_buttonComponent != null)
        {
            _buttonComponent.interactable = interactable;
        }

        
        if (_buttonImage != null)
        {
            _buttonImage.color = interactable ? normalColor : new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }

    
    public void SetVisible(bool visible)
    {
        if (_buttonImage != null)
        {
            _buttonImage.enabled = visible;
        }
    }

    
    public void ResetState()
    {
        WasPressed = false;
        IsHeld = false;

        if (_buttonImage != null && showVisualFeedback)
        {
            _buttonImage.color = normalColor;
        }
    }

    
    private void OnEnable()
    {
        ResetState();
    }

    private void OnDisable()
    {
        ResetState();
    }
}