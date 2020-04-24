using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapBuilding : MonoBehaviour
{
    [SerializeField] private BuildingConfig _buildingConfig;
    [SerializeField] private TextMeshProUGUI _buildingName;
    
    private GUIManager _guiManager;
    
    void Start()
    {
        Debug.Assert(_buildingConfig != null, "Building config not set");
        
        _buildingName.text = _buildingConfig.BuildingName;
        _guiManager = FindObjectOfType<GUIManager>();
    }
    
    public void BuildingTapped()
    {
        _guiManager.OpenBuildingPopup(_buildingConfig);
    }
}
