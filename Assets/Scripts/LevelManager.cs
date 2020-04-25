using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Assertions;
using Events;
using Vector3 = UnityEngine.Vector3;

public class LevelManager : MonoBehaviour
{
    public enum LevelState
    {
        Running,
        Victory,
        GameOver
    }
    
    [SerializeField] private GlobalConfig GlobalConfig;
    [SerializeField] private LevelConfig LevelConfig;
    [SerializeField] private Transform PiecesParent;
    [SerializeField] private BoosterConfig BoosterConfig;
    private Camera _mainCamera;
    
    private int _pieceTouchLayer;
    private UIManager _uiManager;
    
    private PieceGenerator _pieceGenerator;

    private int _remainingMovements;
    private int _remainingPieces;
    private Piece.PieceType _targetPieceType;

    private LevelState _levelState = LevelState.Running;

    private const string PieceLayerName = "Piece";
    
    void Start()
    {
        Debug.Assert(GlobalConfig != null, "GlobalConfig not set");
        Debug.Assert(LevelConfig != null, "LevelConfig not set");
        Debug.Assert(PiecesParent != null, "PieceParent not set");
        Debug.Assert(BoosterConfig != null, "BoosterConfig not set");

        _mainCamera = Camera.main;
        _pieceTouchLayer = 1 << LayerMask.NameToLayer(PieceLayerName);
        
        _pieceGenerator = FindObjectOfType<PieceGenerator>();
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.Initialize(LevelConfig);

        _remainingMovements = LevelConfig.TotalMoves;
        _remainingPieces = LevelConfig.LevelObjectives[0].Number;
        _targetPieceType = LevelConfig.LevelObjectives[0].PieceType;
        
        _pieceGenerator.Setup(LevelConfig.AvailablePieces, PiecesParent, PieceDestroyed);
        _pieceGenerator.CreatePieces(LevelConfig.TotalPieces, OnPiecesCreated);
        
        EventManager.Instance.StartListening(TouchEvent.EventName, OnTouchEvent);
    }

    #region Callbacks
    
    private void OnTouchEvent(BaseEventData ev)
    {
        if (_levelState != LevelState.Running)
            return;
        
        var touchEventData = (TouchEventData) ev;

        switch (touchEventData.TouchState)
        {
            case TouchManager.TouchState.Tap:
                ProcessTap(touchEventData.CurPosition);
                break;
        }
    }

    public void BoosterDestroyed(int piecesDestroyed)
    {
        _pieceGenerator.CreatePieces(piecesDestroyed, OnPiecesCreated);
    }

    void PieceDestroyed(Piece piece)
    {
        if (piece.pieceType == _targetPieceType)
        {
            //Debug.Log("BLASTED " + groupSize);
            _remainingPieces = Mathf.Clamp(_remainingPieces - 1, 0, _remainingPieces);
            _uiManager.UpdatePieces(_remainingPieces);
            
            if (_remainingPieces == 0)
            {
                Victory();
            }
        }
    }

    void OnPiecesCreated()
    {
        
    }
    #endregion
    
    void ProcessTap(UnityEngine.Vector2 touchPosition)
    {
        var ray = _mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        RaycastHit hit;
        
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 9999f, _pieceTouchLayer))
        {
            var piece = hit.collider.GetComponent<Piece>();
            if (piece == null)
                return;
            
            if (piece.PieceConfig.IsBooster)
            {
                var iPieceBooster = (IPieceBooster) piece;
                iPieceBooster.ExecuteBooster();
                return;
            }
            
            var groupFinder = new GroupFinder();
            var group = groupFinder.FindGroup(piece);

            if (group != null && group.Count >= GlobalConfig.MinGroupSizeToDestroy)
            {
                ProcessGroup(group, piece);
            }
        }
    }

    void ProcessGroup(List<Piece> group, Piece originalPiece)
    {
        _remainingMovements = Mathf.Clamp(_remainingMovements - 1, 0, _remainingMovements);
        _uiManager.UpdateRemainingMovements(_remainingMovements);
        
        DestroyGroup(group, originalPiece);

        if (_remainingMovements <= 0)
        {
             GameOver();
             return;
        }
    }

    void DestroyGroup(List<Piece> group, Piece originalPiece)
    {
        int piecesToDestroy = group.Count;
        var boosterCreated = CreateBoosters(piecesToDestroy, originalPiece);
        
        foreach (var piece in group)
        {
            piece.DestroyPiece();
        }
        
        _pieceGenerator.CreatePieces(piecesToDestroy, OnPiecesCreated);
    }

    bool CreateBoosters(int piecesToDestroy, Piece originalPiece)
    {
        var pieceConfig = BoosterConfig.GetBooster(piecesToDestroy);

        if (pieceConfig == null)
        {
            return false;
        }

        var booster = Instantiate(pieceConfig.Prefab, originalPiece.transform, false);
        booster.transform.parent = originalPiece.transform.parent;
        booster.GetComponent<IPieceBooster>().Initialize(this);
        
        return true;
    }

    void GameOver()
    {
        if (_levelState == LevelState.GameOver)
            return;
        
        if (_levelState == LevelState.Running)
        {
            _levelState = LevelState.GameOver;
            _uiManager.ShowGameOverPanel();
        }
    }

    void Victory()
    {
        if (_levelState == LevelState.Victory)
            return;
        
        if (_levelState == LevelState.Running)
        {
            _levelState = LevelState.GameOver;
            _uiManager.ShowVictoryPanel();
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening(TouchEvent.EventName, OnTouchEvent);
    }
}