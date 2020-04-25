using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BombBoosterConfig", menuName = "ScriptableObjects/Boosters/BombBoosterConfig", order = 1)]
public class BombBoosterConfig : ScriptableObject
{
    public float DestroyRange = 4f;
    public float DestroyDelayPerMeter = 0.3f;
}
