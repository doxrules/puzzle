using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceCollection", menuName = "ScriptableObjects/PieceCollection", order = 1)]
public class PieceCollection : ScriptableObject
{
    public List<PieceConfig> PieceConfigs;
    
    public bool DebugModeEnabled;

    public PieceConfig GetPieceConfig(Piece.PieceType pieceType)
    {
        foreach (var pieceConfig in PieceConfigs)
        {
            if (pieceConfig.PieceType == pieceType)
                return pieceConfig;
        }

        return null;
    }
}
