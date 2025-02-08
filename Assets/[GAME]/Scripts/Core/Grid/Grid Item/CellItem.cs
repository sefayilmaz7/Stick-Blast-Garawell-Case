using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GarawellGames.Core;
using UnityEngine;
using UnityEngine.Events;
using Grid = GarawellGames.Core.Grid;
using Random = UnityEngine.Random;

public class CellItem : ItemBase
{
    public static event UnityAction OnBlockProcessDone;

    public bool IsFilled = false;
    public Directions CellDirections;
    public CellItemEdge RightEdge;
    public CellItemEdge LeftEdge;
    public CellItemEdge UpEdge;
    public CellItemEdge DownEdge;

    private List<GameObject> _blocksInside = new List<GameObject>();

    public void AddBlockToList(GameObject controller)
    {
        _blocksInside.Add(controller);
    }

    public void DropBlocks()
    {
        foreach (var blockController in _blocksInside)
        {
            blockController.transform.DOMoveY(blockController.transform.position.y - 15, 2.5f);
            blockController.transform
                .DORotate(new Vector3(0, 0, Random.Range(0, 360)), 2.5f, RotateMode.FastBeyond360)
                .OnComplete(() => Destroy(blockController));
        }

        ClearItem();
    }

    public bool CheckFilled()
    {
        return CellDirections.Right && CellDirections.Left && CellDirections.Up && CellDirections.Down;
    }

    public void FillItem()
    {
        ResetSortOrders();
        IsFilled = true;
        itemVisual.FillCellItem(ColorManager.Instance.LevelColor);
    }

    public void UnFillItem()
    {
        itemVisual.VisualReset();
    }

    private void ResetSortOrders()
    {
        RightEdge.EdgeSprite.sortingOrder = 0;
        LeftEdge.EdgeSprite.sortingOrder = 0;
        UpEdge.EdgeSprite.sortingOrder = 0;
        DownEdge.EdgeSprite.sortingOrder = 0;
    }

    public void ClearItem()
    {
        IsFilled = false;
        itemVisual.VisualReset();
        _blocksInside.Clear();
        ResetEdges();
        ResetDirections();
        ClearNeighbours();
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

    private void ResetDirections()
    {
        CellDirections.Up = false;
        CellDirections.Down = false;
        CellDirections.Left = false;
        CellDirections.Right = false;
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

    private void OnBlockTaken(CellItem item, Directions directions)
    {
        if (item != this)
            return;

        AddDirections(directions);
        EffectNeighbours(directions);
        if (CheckFilled())
        {
            FillItem();
        }

        DOVirtual.DelayedCall(0.3f, () => OnBlockProcessDone?.Invoke());
    }

    private void EffectNeighbours(Directions directions)
    {
        CheckUpCell(directions);
        CheckDownCell(directions);
        CheckRightCell(directions);
        CheckLeftCell(directions);
    }

    private void ClearNeighbours()
    {
        ClearDownCell();
        ClearUpCell();
        ClearRightCell();
        ClearLeftCell();
    }

    #region Neighbour Updates

    private void CheckLeftCell(Directions directions)
    {
        Grid grid = GameBuilder.Instance.GetGrid();

        Cell leftCell = grid.GetCellByCoordinates(X - 1, Y);
        if (leftCell != null)
        {
            if (directions.Left)
            {
                if (leftCell.GetItem() is CellItem leftItem)
                {
                    leftItem.CellDirections.Right = true;
                    if (leftItem.CheckFilled())
                    {
                        leftItem.FillItem();
                    }
                }
            }
            else
            {
                if (leftCell.GetItem() is CellItem leftItem)
                {
                    if (leftItem.CellDirections.Right)
                    {
                        CellDirections.Left = true;
                    }
                }
            }
        }
    }

    private void CheckRightCell(Directions directions)
    {
        Grid grid = GameBuilder.Instance.GetGrid();

        Cell rightCell = grid.GetCellByCoordinates(X + 1, Y);
        if (rightCell != null)
        {
            if (directions.Right)
            {
                if (rightCell.GetItem() is CellItem rightItem)
                {
                    rightItem.CellDirections.Left = true;
                    if (rightItem.CheckFilled())
                    {
                        rightItem.FillItem();
                    }
                }
            }
            else
            {
                if (rightCell.GetItem() is CellItem rightItem)
                {
                    if (rightItem.CellDirections.Left)
                    {
                        CellDirections.Right = true;
                    }
                }
            }
        }
    }

    private void CheckDownCell(Directions directions)
    {
        Grid grid = GameBuilder.Instance.GetGrid();

        Cell downCell = grid.GetCellByCoordinates(X, Y + 1);
        if (downCell != null)
        {
            if (directions.Down)
            {
                if (downCell.GetItem() is CellItem downItem)
                {
                    downItem.CellDirections.Up = true;
                    if (downItem.CheckFilled())
                    {
                        downItem.FillItem();
                    }
                }
            }
            else
            {
                if (downCell.GetItem() is CellItem downItem)
                {
                    if (downItem.CellDirections.Up)
                    {
                        CellDirections.Down = true;
                    }
                }
            }
        }
    }

    private void CheckUpCell(Directions directions)
    {
        Grid grid = GameBuilder.Instance.GetGrid();

        Cell upCell = grid.GetCellByCoordinates(X, Y - 1);
        if (upCell != null)
        {
            if (directions.Up)
            {
                if (upCell.GetItem() is CellItem upItem)
                {
                    upItem.CellDirections.Down = true;
                    if (upItem.CheckFilled())
                    {
                        upItem.FillItem();
                    }
                }
            }
            else
            {
                if (upCell.GetItem() is CellItem upItem)
                {
                    if (upItem.CellDirections.Down)
                    {
                        CellDirections.Up = true;
                    }
                }
            }
        }
    }

    private void ClearUpCell()
    {
        Grid grid = GameBuilder.Instance.GetGrid();

        Cell upCell = grid.GetCellByCoordinates(X, Y - 1);
        if (upCell != null)
        {
            if (upCell.GetItem() is CellItem upItem)
            {
                upItem.CellDirections.Down = false;
                upItem.DownEdge.ResetEdge();
                upItem.UnFillItem();
            }
        }
    }

    private void ClearDownCell()
    {
        Grid grid = GameBuilder.Instance.GetGrid();

        Cell downCell = grid.GetCellByCoordinates(X, Y + 1);
        if (downCell != null)
        {
            if (downCell.GetItem() is CellItem downItem)
            {
                downItem.CellDirections.Up = false;
                downItem.UpEdge.ResetEdge();
                downItem.UnFillItem();
            }
        }
    }

    private void ClearRightCell()
    {
        Grid grid = GameBuilder.Instance.GetGrid();

        Cell rightCell = grid.GetCellByCoordinates(X+ 1, Y);
        if (rightCell != null)
        {
            if (rightCell.GetItem() is CellItem rightItem)
            {
                rightItem.CellDirections.Left = false;
                rightItem.LeftEdge.ResetEdge();
                rightItem.UnFillItem();
            }
        }
    }

    private void ClearLeftCell()
    {
        Grid grid = GameBuilder.Instance.GetGrid();

        Cell leftCell = grid.GetCellByCoordinates(X+ 1, Y);
        if (leftCell != null)
        {
            if (leftCell.GetItem() is CellItem leftItem)
            {
                leftItem.CellDirections.Right = false;
                leftItem.RightEdge.ResetEdge();
                leftItem.UnFillItem();
            }
        }
    }
    

    #endregion

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