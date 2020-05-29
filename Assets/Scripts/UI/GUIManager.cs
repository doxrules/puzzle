using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OpenPuzzleSelectionMenu()
    {
        SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }
}
