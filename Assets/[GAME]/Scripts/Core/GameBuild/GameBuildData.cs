using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BoardBuildData", menuName = "ScriptableObjects/BoardBuildData", order = 1)]
public class GameBuildData : ScriptableObject
{
    public CellItem CellItem;
    public int Width;
    public int Height;
    public float TargetScore;
    public Color BlockColorForLevel;
    public BlockHelper[] LevelBlocks;
}
