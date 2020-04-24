using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private GenericPopup GenericPopup;
    [SerializeField] private BuildingPopup BuildingPopup;
    
    public void OpenPopup()
    {
        GenericPopup.OpenPopup();
    }

    public void OpenBuildingPopup(BuildingConfig buildingConfig)
    {
        BuildingPopup.OpenPopup(buildingConfig);
    }
}
