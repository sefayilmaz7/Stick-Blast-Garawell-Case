using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Grid = GarawellGames.Core.Grid;

public class BoardController : MonoBehaviour
{
    public static event UnityAction OnRowOrColumnCleared;
    
    private Grid _grid;

    private void Start()
    {
        _grid = GameBuilder.Instance.GetGrid();
    }

    private void CheckColumns()
    {
        foreach (var column in _grid.ColumnList)
        {
            if (column.IsColumnFilled())
            {
                column.ClearFilledColumn();
                OnRowOrColumnCleared?.Invoke();
            }
        }
    }

    private void CheckRows()
    {
        foreach (var row in _grid.RowList)
        {
            if (row.IsRowFilled())
            {
                row.ClearFilledRow();
                OnRowOrColumnCleared?.Invoke();
            }
        }
    }

    private void OnEnable()
    {
        CellItem.OnBlockProcessDone += CheckColumns;
        CellItem.OnBlockProcessDone += CheckRows;
    }

    private void OnDisable()
    {
        CellItem.OnBlockProcessDone -= CheckColumns;
        CellItem.OnBlockProcessDone -= CheckRows;
    }
}
