using UnityTemplateProjects.Events;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        Cinematic,
        Manual
    }

    [SerializeField] private Camera TargetCamera;

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
        _targetCameraPosition = TargetCamera.transform.position;
        Zoom = TargetCamera.orthographicSize;
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
                UpdateZoom(-touchEventData.DeltaMultiTouch);
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
        TargetCamera.transform.position = Vector3.Slerp(transform.position,  TargetCameraPosition, lerpValue);
        TargetCamera.orthographicSize = Mathf.Lerp(TargetCamera.orthographicSize, Zoom, lerpValue);
    }

    void PanCamera(Vector2 deltaMovement)
    {
        TargetCameraPosition += new Vector3(0f, -deltaMovement.y * CameraVerticalPanSpeed * 0.01f, -deltaMovement.x * CameraHorizontalPanSpeed * 0.01f); 
    }
    
    void UpdateZoom(float delta)
    {
        Zoom += delta * CameraZoomSpeed;


    }

}
