using UnityEngine;
using UnityTemplateProjects.Events;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    private int _buildingMapLayer;
    
    void Start()
    {
        _buildingMapLayer = 1 << LayerMask.NameToLayer("3DUI");
        
        EventManager.Instance.StartListening(TouchEvent.EventName, OnTouchEvent);
    }

    
    // Update is called once per frame
    void Update()
    {
        
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
}
