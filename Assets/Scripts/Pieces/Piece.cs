using System;
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
        Blue,
        BombBooster,
        ColorDestroyerBooster
    }

    [SerializeField] private GlobalConfig GlobalConfig;
    [SerializeField] public PieceConfig PieceConfig;
    
    public PieceType pieceType;
    public float SearchRange;

    private int _pieceNumber;
    
    public void Initialize()
    {

    }

    protected void OnDestroy()
    {
        
    }

    public virtual void DestroyPiece(bool createParticles = true)
    {
        if (createParticles)
        {
            CreateParticles();
        }

        Destroy(this.gameObject);
    }

    public void CreateParticles()
    {
        var particles = Instantiate(PieceConfig.DestroyParticles, transform, false);
        particles.transform.parent = transform.parent;
        particles.transform.rotation= Quaternion.identity;
    }
}
