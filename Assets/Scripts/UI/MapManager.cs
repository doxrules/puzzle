using System;
using UnityEngine;
using Events;

public class MapManager : MonoBehaviour
{
    private int _buildingMapLayer;
    private const string layer3DUI = "3DUI";
    
    void Start()
    {
        _buildingMapLayer = 1 << LayerMask.NameToLayer(layer3DUI);
        
        EventManager.Instance.StartListening(TouchEvent.EventName, OnTouchEvent);
    }
    
    private void OnTouchEvent(BaseEventData ev)
    {
        var touchEventData = (TouchEventData) ev;

        switch (touchEventData.TouchState)
        {
            case TouchManager.TouchState.Tap:
                FindTapOnBuilding(touchEventData.CurPosition);
            break;
        }
    }

    void FindTapOnBuilding(Vector2 tapPosition)
    {
        var ray = Camera.main.ScreenPointToRay (tapPosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 99999f, _buildingMapLayer))
        {
            var buildingComponent = hit.collider.gameObject.GetComponent<MapBuilding>();
            
            if (!buildingComponent)
                return;
            
            buildingComponent.BuildingTapped();
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening(TouchEvent.EventName, OnTouchEvent);
    }
}
