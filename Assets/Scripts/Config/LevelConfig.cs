using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    [Serializable]
    public class LevelObjective
    {
        public Piece.PieceType PieceType;
        public int Number;
    }

    public string SceneName;
    public int TotalPieces;
    public int TotalMoves;
    public List<Piece.PieceType> AvailablePieces;
    public List<LevelObjective> LevelObjectives;
}
