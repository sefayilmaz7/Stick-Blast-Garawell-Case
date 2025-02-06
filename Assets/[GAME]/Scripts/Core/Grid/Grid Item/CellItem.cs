using System.Collections;
using System.Collections.Generic;
using GarawellGames.Core;
using UnityEngine;

public class CellItem : ItemBase
{
    [SerializeField] private List<CellItemCorner> itemCorners = new List<CellItemCorner>();
}
