using System.Collections;
using System.Collections.Generic;
using GarawellGames.Core;
using UnityEngine;

public class CellItem : ItemBase
{
    public bool IsFilled = false;
    [SerializeField] private List<CellItemEdge> itemCorners = new List<CellItemEdge>();

    public void FillItem(Color color)
    {
        IsFilled = true;
        itemVisual.FillCellItem(color);
    }

    public void ClearItem()
    {
        IsFilled = false;
        itemVisual.VisualReset();
    }
}
