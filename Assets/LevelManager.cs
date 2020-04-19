using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfig LevelConfig;
    [SerializeField] private Transform PiecesParent;

    private const int _pieceTouchLayer = 1 << 10;
    
    private PieceGenerator _pieceGenerator;
    
    void Start()
    {
        _pieceGenerator = FindObjectOfType<PieceGenerator>();
        _pieceGenerator.GeneratePieces(LevelConfig.TotalPieces, LevelConfig.PiecesPrefabs, PiecesParent);
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            DetectTouch();
        }
    }

    void DetectTouch()
    {
        var ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.magenta, 1f);
        RaycastHit hit;
        
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 9999f, _pieceTouchLayer))
        {
            
            
        }
        else
        {
            
        }
        
    }
}
