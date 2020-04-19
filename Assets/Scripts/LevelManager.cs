using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GlobalConfig GlobalConfig;
    [SerializeField] private LevelConfig LevelConfig;
    [SerializeField] private Transform PiecesParent;

    private const int _pieceTouchLayer = 1 << 10;
    private UIManager _uiManager;
    
    private PieceGenerator _pieceGenerator;

    private int _remainingMovements;
    private int _remainingPieces;
    private Piece.PieceType _targetPieceType;
    
    void Start()
    {
        Debug.Assert(GlobalConfig != null, "GlobalConfig not set");
        Debug.Assert(LevelConfig != null, "LevelConfig not set");
        Debug.Assert(PiecesParent != null, "PieceParent not set");
        
        _pieceGenerator = FindObjectOfType<PieceGenerator>();
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.Initialize(LevelConfig);

        _remainingMovements = LevelConfig.TotalMoves;
        _remainingPieces = LevelConfig.LevelObjectives[0].Number;
        _targetPieceType = LevelConfig.LevelObjectives[0].PieceType;
        
        _pieceGenerator.GeneratePieces(LevelConfig.TotalPieces, LevelConfig.AvailablePieces, PiecesParent);
    }

    
    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
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
            var piece = hit.collider.GetComponent<Piece>();
            
            var groupFinder = new GroupFinder();
            var group = groupFinder.FindGroup(piece);

            if (group != null && group.Count >= GlobalConfig.MinGroupSizeToDestroy)
            {
                ProcessGroup(group);
            }
        }
        
    }

    void ProcessGroup(List<GameObject> group)
    {
        _remainingMovements = Mathf.Clamp(_remainingMovements - 1, 0, _remainingMovements);
        _uiManager.UpdateRemainingMovements(_remainingMovements);
        
        var groupPieceType = group[0].GetComponent<Piece>().pieceType;
        var groupSize = group.Count;
        
        DestroyGroup(group);

        if (groupPieceType == _targetPieceType)
        {
            Debug.Log("BLASTED " + groupSize);
            _remainingPieces = Mathf.Clamp(_remainingPieces - groupSize, 0, _remainingPieces);
            _uiManager.UpdatePieces(_remainingPieces);
            
            if (_remainingPieces == 0)
            {
                Victory();
                return;
            }
        }
        
        if (_remainingMovements <= 0)
        {
             GameOver();
             return;
        }

    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
    }

    void Victory()
    {
        Debug.Log("VICTORY");
    }
    
    void DestroyGroup(List<GameObject> group)
    {
        foreach (var piece in group)
        {
            Destroy(piece);
        }
        
    }
}