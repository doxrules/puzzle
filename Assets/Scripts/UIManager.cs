using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] PieceCollection PieceCollection;
    [SerializeField] Image PieceIcon;
    [SerializeField] private TextMeshProUGUI RemainingPieces;
    [SerializeField] private TextMeshProUGUI RemainingMoves;
    
    public void Initialize(LevelConfig levelConfig)
    {
        var levelObjective = levelConfig.LevelObjectives[0];
        var pieceConfig = PieceCollection.GetPieceConfig(levelObjective.PieceType);
        
        UpdatePieces(levelObjective.Number);
        PieceIcon.sprite = pieceConfig.PieceSprite;
        RemainingMoves.text = levelConfig.TotalMoves.ToString();
    }

    public void UpdatePieces(int remainingPieces)
    {
        RemainingPieces.text = remainingPieces.ToString();
    }

    public void UpdateRemainingMovements(int remainingMovements)
    {
        RemainingMoves.text = remainingMovements.ToString();
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
