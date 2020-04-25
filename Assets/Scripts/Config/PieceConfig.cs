using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceConfig", menuName = "ScriptableObjects/PieceConfig", order = 1)]
public class PieceConfig : ScriptableObject
{
    public Piece.PieceType PieceType;
    public Sprite PieceSprite;
    public GameObject Prefab;
    public GameObject DestroyParticles;
    public bool IsBooster = false;
}
