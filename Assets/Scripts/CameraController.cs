using System;
using UnityTemplateProjects.Events;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        Cinematic,
        Manual
    }

    [SerializeField] private Camera MainCamera;

    [SerializeField] private BoxCollider CameraBounds;
    
    [Range(0,10)]
    [SerializeField] float lerpValue = 1;

    [Range(1, 50)]
    [SerializeField] float CameraHorizontalPanSpeed = 10;
    
    [Range(1, 50)]
    [SerializeField] float CameraVerticalPanSpeed = 10;
    
    [Range(1, 100)]
    [SerializeField] float CameraZoomSpeed = 10;


    [SerializeField] private float _zoom;

    [SerializeField] private float MinZoom = 40;
    [SerializeField] private float MaxZoom = 140;
    
    
    private CameraMode _cameraMode = CameraMode.Manual;
    
    private Vector3 _targetCameraPosition;

    public Camera GetMainCamera()
    {
        return MainCamera;
    }
    
    public float Zoom
    {
        get { return _zoom; }
        set { _zoom = Mathf.Clamp(value,  MinZoom, MaxZoom); }
    }
    
    public Vector3 TargetCameraPosition
    {
        get { return _targetCameraPosition; }
        set
        {
            if (CameraBounds.bounds.Contains(value))
            {
                _targetCameraPosition = value;
            }

            _targetCameraPosition = CameraBounds.bounds.ClosestPoint(value);
        }
    }
    
    void Start ()
    {
        SetInitialCamera();

        EventManager.Instance.StartListening(TouchEvent.EventName, OnTouchUpdated);
    }
    
    
    public void SetInitialCamera()
    {
        _targetCameraPosition = MainCamera.transform.position;
        Zoom = MainCamera.orthographicSize;
    }
    
    private void OnTouchUpdated(BaseEventData ev)
    {
        var touchEventData = (TouchEventData) ev;

        switch (touchEventData.TouchState)
        {
            case TouchManager.TouchState.InitPan:
                //_initialDragPosition = sender.ScreenPosition;
                //GetInitialDirectionOnScreenSpace();
                break;
            case TouchManager.TouchState.UpdatePan:
                PanCamera(touchEventData.DeltaIncrement);
                break;
            case TouchManager.TouchState.FinishPan:
                //ResetRotation();
                break;
            case TouchManager.TouchState.UpdateMultiTouch:
                UpdateZoom(-touchEventData.DeltaIncrementMagnitude);
                break;
        }
    }

    void Update ()
    {
        if (_cameraMode == CameraMode.Manual)
        {
            UpdateManualCamera();
        }
    }


    void UpdateManualCamera()
    {
        MainCamera.transform.position = Vector3.Slerp(transform.position,  TargetCameraPosition, lerpValue);
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, Zoom, lerpValue);
    }

    void PanCamera(Vector2 deltaMovement)
    {
        TargetCameraPosition += new Vector3(0f, deltaMovement.y * CameraVerticalPanSpeed * 0.01f, deltaMovement.x * CameraHorizontalPanSpeed * 0.01f); 
    }
    
    void UpdateZoom(float delta)
    {
        Zoom += delta * CameraZoomSpeed;
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening(TouchEvent.EventName, OnTouchUpdated);
    }
}
