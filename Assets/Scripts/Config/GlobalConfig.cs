using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "ScriptableObjects/GlobalConfig", order = 1)]
public class GlobalConfig : ScriptableObject
{
    public int MinGroupSizeToDestroy;
}
