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
    
    private PieceGenerator _pieceGenerator;
    
    void Start()
    {
        Debug.Assert(GlobalConfig != null, "GlobalConfig not set");
        Debug.Assert(LevelConfig != null, "LevelConfig not set");
        Debug.Assert(PiecesParent != null, "PieceParent not set");
        
        _pieceGenerator = FindObjectOfType<PieceGenerator>();
        _pieceGenerator.GeneratePieces(LevelConfig.TotalPieces, LevelConfig.PiecesPrefabs, PiecesParent);
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
                DestroyGroup(group);
            }
        }
        
    }

    void DestroyGroup(List<GameObject> group)
    {
        foreach (var piece in group)
        {
            Destroy(piece);
        }
        
    }
}