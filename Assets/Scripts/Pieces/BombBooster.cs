using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombBooster : Piece, IPieceBooster
{
    [SerializeField] BombBoosterConfig BombBoosterConfig;
    private int _coroutinesCreated = 0;
    private int _pieceDestroyed = 0;
    
    List<Piece> destroyedPieces = new List<Piece>();

    private bool _boosterExecuted = false;
    private LevelManager _levelManager;
    
    public void Initialize(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }
    
    public void ExecuteBooster()
    {
        _boosterExecuted = true;

        var hits = Physics.OverlapSphere(transform.position, BombBoosterConfig.DestroyRange, 1<<LayerMask.NameToLayer("Piece"));

        foreach (var piece in hits)
        {
            if (piece.gameObject == this.gameObject)
                continue;

            var pieceComponent = piece.GetComponent<Piece>();
            
            if (piece == null)
            {
                return;
            }
            
            if (destroyedPieces.Contains(pieceComponent))
            {
                continue;
            }
            
            destroyedPieces.Add(pieceComponent);
            
            var distance = Vector3.Distance(transform.position, piece.gameObject.transform.position);
            
            ++_coroutinesCreated;
            ++_pieceDestroyed;
            
            StartCoroutine(DestroyPiece(pieceComponent, (distance * BombBoosterConfig.DestroyDelayPerMeter) - BombBoosterConfig.DestroyDelayPerMeter));
        }

        CreateParticles();
    }

    IEnumerator DestroyPiece(Piece piece, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (piece != null)
        {
            piece.DestroyPiece();
        }

        CoroutineFinished();
    }

    void CoroutineFinished()
    {
        --_coroutinesCreated;

        if (_coroutinesCreated <= 0)
        {
            _levelManager.BoosterDestroyed(_pieceDestroyed);
            DestroyPiece(false);
        }
    }
    
    public override void DestroyPiece(bool createParticles = true)
    {
        base.DestroyPiece(createParticles);
        
        if (!_boosterExecuted)
            ExecuteBooster();
    }
}
