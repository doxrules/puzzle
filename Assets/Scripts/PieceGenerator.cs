using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    [SerializeField] private PieceCollection PieceCollection;
    [SerializeField] private float SpawnDelay;
    [SerializeField] private BoxCollider SpawnArea;
    
    private int _piecesToGenerate;
    private List<Piece.PieceType> _availablePieces;
    private Transform _piecesParent;
    
    private int _piecesGenerated;

    private float _xRangeInterval;
    private float _yRangeInterval;
    
    public void GeneratePieces(int piecesToGenerate, List<Piece.PieceType> availablePieces, Transform piecesParent)
    {
        _piecesToGenerate = piecesToGenerate;
        _availablePieces = availablePieces;
        _piecesParent = piecesParent;
        
        _xRangeInterval = SpawnArea.size.x * 0.5f * 100f;
        _yRangeInterval = SpawnArea.size.y * 0.5f * 100f;

        StartCoroutine(nameof(CreatePieces));
    }


    IEnumerator CreatePieces()
    {
        while (_piecesGenerated  <  _piecesToGenerate)
        {
            var newPiece = Instantiate(GetRandomPiece(), GetRandomPosition(), Quaternion.identity, _piecesParent);
            newPiece.GetComponent<Piece>().Initialize(_piecesGenerated);
            ++_piecesGenerated;
            
            yield return SpawnDelay;
        }
    }

    Vector3 GetRandomPosition()
    {
        var randomX = SpawnArea.transform.position.x + Random.Range(-_xRangeInterval, _xRangeInterval) / 100f;
        var randomY = SpawnArea.transform.position.y + Random.Range(-_yRangeInterval, _yRangeInterval) / 100f;
        
        return new Vector3(randomX, randomY, SpawnArea.transform.position.z);
    }

    GameObject GetRandomPiece()
    {
        var pieceType = _availablePieces[Random.Range(0, _availablePieces.Count)];
        return PieceCollection.GetPieceConfig(pieceType).Prefab;
    }
}
