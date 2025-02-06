using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BoardBuildData", menuName = "ScriptableObjects/BoardBuildData", order = 1)]
public class GameBuildData : ScriptableObject
{
    [FormerlySerializedAs("baseSquareItem")] public CellItem cellItem;
    public int width;
    public int height;
}
