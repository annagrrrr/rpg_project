using UnityEngine;

public class MobileCameraInputService : ICameraInputService
{
    private float _lastTouchX;
    private float _lastTouchY;
    private bool _isDragging = false;
    private int _cameraTouchId = -1;

    private float sensitivity = 0.2f;

    public float GetMouseX()
    {
#if UNITY_ANDROID || UNITY_IOS
        return GetTouchDeltaX();
#else
        return Input.GetAxis("Mouse X");
#endif
    }

    public float GetMouseY()
    {
#if UNITY_ANDROID || UNITY_IOS
        return GetTouchDeltaY();
#else
        return Input.GetAxis("Mouse Y");
#endif
    }

    private float GetTouchDeltaX()
    {
        if (Input.touchCount == 0)
        {
            _isDragging = false;
            _cameraTouchId = -1;
            return 0f;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            bool isCameraTouch = IsCameraControlTouch(touch);

            if (isCameraTouch)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _lastTouchX = touch.position.x;
                        _lastTouchY = touch.position.y;
                        _isDragging = true;
                        _cameraTouchId = touch.fingerId;
                        return 0f;

                    case TouchPhase.Moved:
                        if (_isDragging && touch.fingerId == _cameraTouchId)
                        {
                            float deltaX = (touch.position.x - _lastTouchX) * sensitivity * Time.deltaTime * 60f;
                            _lastTouchX = touch.position.x;
                            return deltaX;
                        }
                        break;

                    case TouchPhase.Stationary:
                        return 0f;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (touch.fingerId == _cameraTouchId)
                        {
                            _isDragging = false;
                            _cameraTouchId = -1;
                        }
                        return 0f;
                }
            }
        }

        return 0f;
    }

    private float GetTouchDeltaY()
    {
        if (Input.touchCount == 0)
        {
            return 0f;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            bool isCameraTouch = IsCameraControlTouch(touch);

            if (isCameraTouch)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _lastTouchX = touch.position.x;
                        _lastTouchY = touch.position.y;
                        _isDragging = true;
                        _cameraTouchId = touch.fingerId;
                        return 0f;

                    case TouchPhase.Moved:
                        if (_isDragging && touch.fingerId == _cameraTouchId)
                        {
                            float deltaY = (touch.position.y - _lastTouchY) * sensitivity * Time.deltaTime * 60f;
                            _lastTouchY = touch.position.y;
                            return deltaY;
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (touch.fingerId == _cameraTouchId)
                        {
                            _isDragging = false;
                            _cameraTouchId = -1;
                        }
                        return 0f;
                }
            }
        }

        return 0f;
    }

    private bool IsCameraControlTouch(Touch touch)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        bool isRightSide = touch.position.x > screenWidth * 0.4f; 
        bool isNotButtonArea = touch.position.y < screenHeight * 0.7f; 

        return isRightSide && isNotButtonArea;
    }
}