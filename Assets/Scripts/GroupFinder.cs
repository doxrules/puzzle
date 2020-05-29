using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroupFinder
{
    private List<Piece> _uniquePieces;
    private Piece.PieceType _pieceTypeSearch;
    
    public List<Piece> FindGroup(Piece piece)
    {
        _uniquePieces = new List<Piece>();
        _pieceTypeSearch = piece.pieceType;
        
        _uniquePieces.Add(piece);
        TryToAddPieces(piece);
        
        return _uniquePieces;
    }

    void TryToAddPieces(Piece piece)
    {
        var hits = Physics.OverlapSphere(piece.transform.position, piece.SearchRange, 1<<piece.gameObject.layer);

        foreach (var hit in hits)
        {
            if(hit.gameObject == piece.gameObject)
                continue;
            
            var newPiece = hit.GetComponent<Piece>();
            if (newPiece == null)
                continue;
            
            if (_uniquePieces.Contains(newPiece))
                continue;

            if (newPiece.pieceType != _pieceTypeSearch)
                continue;
            
            _uniquePieces.Add(newPiece);
            TryToAddPieces(newPiece);
            
        }
        
    }

}
