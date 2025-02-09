using System;
using DG.Tweening;
using GarawellGames.Core;
using GarawellGames.Managers;
using UnityEngine;
using UnityEngine.Events;
using Grid = GarawellGames.Core.Grid;

public class CellItem : ItemBase
{
    public static event UnityAction OnBlockProcessDone;

    public bool IsFilled = false;
    public Directions CellDirections;
    public CellItemEdge RightEdge;
    public CellItemEdge LeftEdge;
    public CellItemEdge UpEdge;
    public CellItemEdge DownEdge;

    [SerializeField] private CellItemTargetHolder targetHolder;

    
    public void DropBlocks()
    {
        transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 0, 0.4f);
        itemVisual.PlayDropEffect();
        ClearItem();
    }

    public void EarnPrize()
    {
        targetHolder.EarnTarget();
    }

    public bool CheckFilled()
    {
        return CellDirections.Right && CellDirections.Left && CellDirections.Up && CellDirections.Down;
    }

    public void FillItem()
    {
        ResetSortOrders();
        IsFilled = true;
        AudioManager.Instance.PlayAnySound(AudioManager.SoundType.CELL_FILLED);
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
        ForceResetEdges();
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

    private void ForceResetEdges()
    {
        DownEdge.ResetEdge();
        UpEdge.ResetEdge();
        RightEdge.ResetEdge();
        LeftEdge.ResetEdge();
    }

    public void ResetEdges()
    {
        if (!CellDirections.Down)
        {
            DownEdge.ResetEdge();
        }

        if (!CellDirections.Up)
        {
            UpEdge.ResetEdge();
        }

        if (!CellDirections.Right)
        {
            RightEdge.ResetEdge();
        }

        if (!CellDirections.Left)
        {
            LeftEdge.ResetEdge();
        }
    }

    private void AddDirections(Directions directions)
    {
        if (directions.Down)
        {
            DownEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
            CellDirections.Down = true;
        }

        if (directions.Up)
        {
            UpEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
            CellDirections.Up = true;
        }

        if (directions.Right)
        {
            RightEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
            CellDirections.Right = true;
        }

        if (directions.Left)
        {
            LeftEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
    
    public void EnableTargetItem()
    {
        targetHolder.SpawnTargetItem();
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
                    leftItem.RightEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
                        LeftEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
                    rightItem.LeftEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
                        RightEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
                    downItem.UpEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
                        DownEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
                    upItem.DownEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
                        UpEdge.EdgeSprite.color = ColorManager.Instance.LevelColor;
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
                upItem.UnFillItem();
                upItem.DownEdge.ResetEdge();
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
                downItem.UnFillItem();
                downItem.UpEdge.ResetEdge();

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
                rightItem.UnFillItem();
                rightItem.LeftEdge.ResetEdge();

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
                leftItem.UnFillItem();
                leftItem.RightEdge.ResetEdge();
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
