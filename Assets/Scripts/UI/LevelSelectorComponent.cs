using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectorComponent : MonoBehaviour
{
    [SerializeField] private LevelConfig LevelConfig;
    [SerializeField] private TextMeshProUGUI LevelName;
    
    void Start()
    {
        LevelName.text = LevelConfig.SceneName;
    }
    
    public void OnClick()
    {
        LevelSelectionScreen levelSelectionScreen = FindObjectOfType<LevelSelectionScreen>();
        levelSelectionScreen.LoadLevel(LevelConfig);
    }
}
