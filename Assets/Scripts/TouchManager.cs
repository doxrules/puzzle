using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using UnityTemplateProjects.Events;
using UnityEngine;

public class TouchManager : MonoBehaviour
{

    public enum TouchState
    {
        None,
        InitPan,
        UpdatePan,
        FinishPan,
        InitMultiTouch,
        UpdateMultiTouch,
        FinishMultiTouch,
        Tap,
        TapPressing
    }
    
    private bool _inPanGesture = false;
    private Vector2 _initPanPosition;
    private Vector2 _prevPanPosition;
    private Vector2 _curPanPosition;
    private TouchState _touchState;
    private int _panTouchId;

    private TouchEvent _touchEvent;

    private bool _isMobileDevice;

    private Vector2 _multiTouchPreviousVector;
    private float _deltaMultiTouch;
    
    private int _touchLayer;
    void OnEnable ()
    {
        _touchState = TouchState.None;

        _touchLayer = 1 << 29;
        
        _touchEvent = new TouchEvent();
        _touchEvent.Initialize();

        _isMobileDevice = Application.platform == RuntimePlatform.Android ||
                          Application.platform == RuntimePlatform.IPhonePlayer;
    }

    void Update ()
    {
        if (_isMobileDevice)
        {
            UpdateMobileInput();
            return;
        }

        UpdatePCInput();
    }

    void UpdateMobileInput()
    {
        var touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            var touch = Input.touches[0];
            UpdateSingleTouch(touch.phase, touch.position);
            return;
        }
        
        _initPanPosition = Vector2.zero;
        _prevPanPosition = Vector2.zero;
        _curPanPosition = Vector2.zero;
        
        if (touchCount == 0 || touchCount > 2)
        {
            _deltaMultiTouch = 0f;
            _multiTouchPreviousVector = Vector2.zero;
            
            if (_touchState == TouchState.UpdateMultiTouch)
            {
                _touchState = TouchState.FinishMultiTouch;
                TriggerTouchEvent(Vector2.zero);
                return;
            }

            if (_touchState == TouchState.FinishMultiTouch)
            {
                _touchState = TouchState.None;
                return;
            }
        }
        
        if (touchCount == 2)
        {
            var touch1 = Input.touches[0];
            var touch2 = Input.touches[1];
            
            var touchVector = touch2.position - touch1.position;

            // Set initial distance
            if (_multiTouchPreviousVector == Vector2.zero)
            {
                _multiTouchPreviousVector = touchVector;
                _touchState = TouchState.InitMultiTouch;
                TriggerTouchEvent(Vector2.zero);
                _deltaMultiTouch = 0f;
                return;
            }

            var delta = _multiTouchPreviousVector;
            _deltaMultiTouch = touchVector.magnitude - _multiTouchPreviousVector.magnitude;
            _multiTouchPreviousVector = touchVector;
            _touchState = TouchState.UpdateMultiTouch;
            TriggerTouchEvent(Vector2.zero, _deltaMultiTouch);

        }
        
    }

    void UpdatePCInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            UpdateSingleTouch(TouchPhase.Began, Input.mousePosition);
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            UpdateSingleTouch(TouchPhase.Moved, Input.mousePosition);
            return;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            UpdateSingleTouch(TouchPhase.Ended, Input.mousePosition);
            return;
        }
        

        var scrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheelDelta) > 0f)
        {
            _touchState = TouchState.UpdateMultiTouch;
            TriggerTouchEvent(Vector2.zero, scrollWheelDelta);
        }
        else
        {
            if (_touchState == TouchState.UpdateMultiTouch)
            {
                _touchState = TouchState.FinishMultiTouch;
                TriggerTouchEvent(Vector2.zero, 0f);
            }
            
            if (_touchState == TouchState.FinishMultiTouch)
            {
                _touchState = TouchState.None;
            }
        }
    }

    void UpdateSingleTouch(TouchPhase touchPhase, Vector2 touchPosition)
    {
        switch (touchPhase)
        {
            case TouchPhase.Began:
                _touchState = TouchState.InitPan;
                _initPanPosition = Input.mousePosition;
                _prevPanPosition = Vector2.zero;
                _curPanPosition = Input.mousePosition;

                /*
                var ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
                Debug.DrawRay(ray.origin, ray.direction * 20f, Color.magenta, 1f);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 9999f, _touchLayer))
                {
                    _panType = PanType.Player;
                }
                else
                {
                    _panType = PanType.World;
                }
                */
                
                //Debug.Log("START PAN WITH " + _panType);
                TriggerTouchEvent(Vector2.zero);
                break;
            case TouchPhase.Moved:
                _prevPanPosition = _curPanPosition;
                _curPanPosition = Input.mousePosition;

                // TODO IMPLEMENT TAP
                //if()
                _touchState = TouchState.UpdatePan;
                
                //Debug.Log("PREV " + _prevPanPosition + "   CUR: " + _curPanPosition + "   DELTA  "  + (_curPanPosition - _prevPanPosition));
                TriggerTouchEvent(_curPanPosition - _prevPanPosition);
                break;
            case TouchPhase.Ended:
                _touchState = TouchState.FinishPan;
                TriggerTouchEvent(_curPanPosition - _prevPanPosition);
                break;
        }
    }


    void TriggerTouchEvent(Vector2 deltaIncrement, float deltaMultitouch = 0f)
    {
        var touchEventData = new TouchEventData();
        touchEventData.TouchState = _touchState;
        touchEventData.CurPosition = _curPanPosition;
        touchEventData.DeltaIncrement = deltaIncrement;
        touchEventData.DeltaMultiTouch = deltaMultitouch;
        touchEventData.InitPosition = _initPanPosition;
        touchEventData.CurPanDirection = (_curPanPosition - _initPanPosition).normalized;
        touchEventData.TotalPanScreenPercentageSize =
            Vector2.Distance(_initPanPosition, _curPanPosition) / Screen.height;
             
        EventManager.Instance.TriggerEvent(TouchEvent.EventName, touchEventData);
    }
}
