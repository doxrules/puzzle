using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{

    [SerializeField] private float SpawnDelay;
    [SerializeField] private BoxCollider SpawnArea;
    
    private int _piecesToGenerate;
    private List<GameObject> _piecePrefabs;
    private Transform _piecesParent;
    
    private int _piecesGenerated;

    private float _xRangeInterval;
    private float _yRangeInterval;
    
    public void GeneratePieces(int piecesToGenerate, List<GameObject> piecePrefabs, Transform piecesParent)
    {
        _piecesToGenerate = piecesToGenerate;
        _piecePrefabs = piecePrefabs;
        _piecesParent = piecesParent;
        
        _xRangeInterval = SpawnArea.size.x * 0.5f;
        _yRangeInterval = SpawnArea.size.y * 0.5f;

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
        var randomX = SpawnArea.transform.position.x + Random.Range(-_xRangeInterval, _xRangeInterval);
        var randomY = SpawnArea.transform.position.y + Random.Range(-_yRangeInterval, _yRangeInterval);
        
        return new Vector3(randomX, randomY, SpawnArea.transform.position.z);
    }

    GameObject GetRandomPiece()
    {
        return _piecePrefabs[Random.Range(0, _piecePrefabs.Count - 1)];
    }
}
