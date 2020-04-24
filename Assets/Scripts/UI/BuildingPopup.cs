using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buildingName;
    
    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
    
    public void OpenPopup(BuildingConfig buildingConfig)
    {
        this.gameObject.SetActive(true);
        
        _buildingName.text = buildingConfig.BuildingName;
    }

}
