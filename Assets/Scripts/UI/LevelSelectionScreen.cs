using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionScreen : MonoBehaviour
{
    private const string TownScene = "TownScene";
    
    public void LoadLevel(LevelConfig levelConfig)
    {
        SceneManager.LoadScene("BasePuzzleScene", LoadSceneMode.Single);
        SceneManager.LoadScene(levelConfig.SceneName, LoadSceneMode.Additive);
    }

    public void OpenMapScene()
    {
        SceneManager.LoadScene(TownScene);
    }
}
