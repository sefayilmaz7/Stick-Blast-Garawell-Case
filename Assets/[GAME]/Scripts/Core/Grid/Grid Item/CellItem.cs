using System;
using System.Collections;
using System.Collections.Generic;
using GarawellGames.Core;
using UnityEngine;

public class CellItem : ItemBase
{
    public bool IsFilled = false;
    public Directions CellDirections;
    public CellItemEdge RightEdge;
    public CellItemEdge LeftEdge;
    public CellItemEdge UpEdge;
    public CellItemEdge DownEdge;
    

    private bool CheckFilled()
    {
        return CellDirections.Right && CellDirections.Left && CellDirections.Up && CellDirections.Down;
    }

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

    public void HighlightEdge(Directions blockDirections)
    {
        if (blockDirections.Down)
        {
            DownEdge.EdgeSprite.sortingOrder = 1;
            DownEdge.Highlight();
        }
        if (blockDirections.Up)
        {
            UpEdge.EdgeSprite.sortingOrder = 1;
            UpEdge.Highlight();
        }
        if (blockDirections.Right)
        {
            RightEdge.EdgeSprite.sortingOrder = 1;
            RightEdge.Highlight();
        }
        if (blockDirections.Left)
        {
            LeftEdge.EdgeSprite.sortingOrder = 1;
            LeftEdge.Highlight();
        }
    }

    public void ResetEdges()
    {
        DownEdge.ResetEdge();
        UpEdge.ResetEdge();
        RightEdge.ResetEdge();
        LeftEdge.ResetEdge();
    }

    private void AddDirections(Directions directions)
    {
        if (directions.Down)
        {
            CellDirections.Down = true;
        }
        if (directions.Up)
        {
            CellDirections.Up = true;
        }
        if (directions.Right)
        {
            CellDirections.Right = true;
        }
        if (directions.Left)
        {
            CellDirections.Left = true;
        }
    }

    public CellItemEdge GetEdgeItemByDirection(Directions blockDirections)
    {
        if (blockDirections.Down)
        {
            return DownEdge;
        }
        if (blockDirections.Up)
        {
            return UpEdge;
        }
        if (blockDirections.Right)
        {
            return RightEdge;
        }
        if (blockDirections.Left)
        {
            return LeftEdge;
        }

        return null;
    }

    private void OnBlockTaken(CellItem item, Directions directions, Color fillColor)
    {
        if (item != this)
            return;

        Debug.Log("Place Edildi" + " Up: " + directions.Up + "Down: " + directions.Down + "Right: " + directions.Right + "Left: " + directions.Left);
        AddDirections(directions);
        // Effect neighbour items
        if (CheckFilled())
        {
            FillItem(fillColor);
        }
    }

    private void OnEnable()
    {
        BlockController.OnBlockPlaced += OnBlockTaken;
    }

    private void OnDisable()
    {
        BlockController.OnBlockPlaced -= OnBlockTaken;
    }
}

[Serializable]
public class Directions
{
    public bool Up;
    public bool Down;
    public bool Right;
    public bool Left;

    public bool HasOnlyOneSide;
}
