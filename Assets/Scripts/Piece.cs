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

    [SerializeField] private GlobalConfig GlobalConfig;
    [SerializeField] private TextMeshPro _numberLabel;
    
    public PieceType pieceType;
    public float SearchRange;

    private int _pieceNumber;
    
    public void Initialize(int pieceNumber)
    {
        if (GlobalConfig.DebugModeEnabled)
        {
            _numberLabel.gameObject.SetActive(true);
            this.name = "Piece_" + pieceNumber;
            _numberLabel.text = pieceNumber.ToString();
        }
        else
        {
            _numberLabel.gameObject.SetActive(false);
        }
    }
}
