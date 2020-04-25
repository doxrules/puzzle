using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using Events;
using UnityEngine;

public class TouchManager : MonoBehaviour
{

    public enum TouchState
    {
        None,
        InitGesture,
        InitPan,
        UpdatePan,
        FinishPan,
        InitMultiTouch,
        UpdateMultiTouch,
        FinishMultiTouch,
        Tap
    }

    [SerializeField] float MaxScreenPercentageDistanceForValidTap = 0.02f;
    private bool _inPanGesture = false;
    private Vector2 _initGesturePosition;
    private Vector2 _prevGesturePosition;
    private Vector2 _curGesturePosition;
    private TouchState _touchState;
    private int _panTouchId;

    private TouchEvent _touchEvent;

    private bool _isMobileDevice;

    private Vector2 _multiTouchPreviousVector;
    private float _deltaMultiTouch;
    private float _screenDiagonal;
    
    private int _touchLayer;
    void OnEnable ()
    {
        _touchState = TouchState.None;

        _touchLayer = 1 << 29;
        
        _touchEvent = new TouchEvent();
        _touchEvent.Initialize();

        _isMobileDevice = Application.platform == RuntimePlatform.Android ||
                          Application.platform == RuntimePlatform.IPhonePlayer;
        
        _screenDiagonal = Mathf.Sqrt((Screen.width * Screen.width) + (Screen.height * Screen.height));
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Update ()
    {
        /* TODO Not implemented yet
        if (_isMobileDevice)
        {
            UpdateMobileInput();
            return;
        }
        */

        UpdatePCInput();
    }


    void UpdatePCInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _initGesturePosition = Input.mousePosition;
            _prevGesturePosition = _initGesturePosition;
            _curGesturePosition = _initGesturePosition;
            
            _touchState = TouchState.InitGesture;
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            _prevGesturePosition = _curGesturePosition;
            _curGesturePosition = Input.mousePosition;

            var increment = _prevGesturePosition - _curGesturePosition;
            
            if (_touchState == TouchState.InitGesture && 
                GetDistanceInScreenPercentage(increment.magnitude) > MaxScreenPercentageDistanceForValidTap)
            {
                _touchState = TouchState.InitPan;
            }
            else if (_touchState == TouchState.InitPan)
            {
                _touchState = TouchState.UpdatePan;
            }
            
            TriggerTouchEvent(increment, increment.magnitude);
            return;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            _prevGesturePosition = _curGesturePosition;
            _curGesturePosition = Input.mousePosition;
            
            if (_touchState == TouchState.InitGesture)
            {
                _touchState = TouchState.Tap;
                TriggerTouchEvent(Vector2.zero, 0f);
                return;
            }

            _touchState = TouchState.None;
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

    void TriggerTouchEvent(Vector2 deltaIncrement, float deltaIncrementMagnitude)
    {
        var touchEventData = new TouchEventData();
        touchEventData.TouchState = _touchState;
        touchEventData.CurPosition = _curGesturePosition;
        touchEventData.DeltaIncrement = deltaIncrement;
        touchEventData.DeltaIncrementMagnitude = deltaIncrementMagnitude;
        touchEventData.InitPosition = _initGesturePosition;

        EventManager.Instance.TriggerEvent(TouchEvent.EventName, touchEventData);
    }

    float GetDistanceInScreenPercentage(float distance)
    {
        return distance / _screenDiagonal;
    }
    
    
    
    /* TODO Not implemented yet
    void UpdateMobileInput()
    {
        var touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            var touch = Input.touches[0];
            //UpdateSingleTouch(touch.phase, touch.position);
            return;
        }
        
        _initGesturePosition = Vector2.zero;
        _prevGesturePosition = Vector2.zero;
        _curGesturePosition = Vector2.zero;
        
        if (touchCount == 0 || touchCount > 2)
        {
            _deltaMultiTouch = 0f;
            _multiTouchPreviousVector = Vector2.zero;
            
            if (_touchState == TouchState.UpdateMultiTouch)
            {
                _touchState = TouchState.FinishMultiTouch;
                TriggerTouchEvent(Vector2.zero, 0f);
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
                TriggerTouchEvent(Vector2.zero, 0f);
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
    */

}
