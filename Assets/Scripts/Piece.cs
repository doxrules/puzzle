using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceType
    {
        Orange,
        Pink,
        Green,
        Blue
    }

    [SerializeField] private TextMeshPro _numberLabel;
    
    public PieceType pieceType;
    public float SearchRange;

    private int _pieceNumber;
    
    public void Initialize(int pieceNumber)
    {
        this.name = "Piece_" + pieceNumber;
        _numberLabel.text = pieceNumber.ToString();
    }
}
