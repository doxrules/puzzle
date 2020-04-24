using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCopy : MonoBehaviour
{
    [SerializeField] private Camera _copyCamera;
    [SerializeField] private Camera _targetCamera;
    
    void Start()
    {
        Debug.Assert(_copyCamera != null, "Copy camera not set");
        Debug.Assert(_targetCamera != null, "Target camera not set");
    }
    
    void LateUpdate()
    {
        _targetCamera.orthographicSize = _copyCamera.orthographicSize;
    }
}
