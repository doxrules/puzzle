using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterConfig", menuName = "ScriptableObjects/BoosterConfig", order = 1)]
public class BoosterConfig : ScriptableObject
{
    [Serializable]
    public class BoosterItemConfig
    {
        public int MinRequiredPieces;
        public PieceConfig PieceConfig;
    }

    public List<BoosterItemConfig> BoosterItems;

    public PieceConfig GetBooster(int groupSize)
    {
        if (groupSize < BoosterItems[0].MinRequiredPieces)
        {
            return null;
        }

        foreach (var boosterItem in BoosterItems)
        {
            if (groupSize >= boosterItem.MinRequiredPieces)
                return boosterItem.PieceConfig;
        }

        return null;
    }
}
