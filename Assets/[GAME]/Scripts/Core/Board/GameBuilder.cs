using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GarawellGames.Core.Grid;

public class GameBuilder : Singleton<GameBuilder>
{
    [SerializeField] private Grid _grid;
    
    private int _topPositionOfBoard;
    private GameBuildData _gameBuildData;

    protected override void AwakeSingleton()
    {
        base.AwakeSingleton();
        _gameBuildData = LevelManager.Instance.GetLevelData();
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        // Update Data (from LevelManager) before getting values 
        _grid = new Grid(_gameBuildData.Width, _gameBuildData.Height, 1);
        _topPositionOfBoard = _grid.Height - 1;
    }

    private void Start()
    {
        BuildBoard(_gameBuildData);
    }

    private void BuildBoard(GameBuildData levelData)
    {
        for (int y = 0; y < _grid.Height; y++)
        {
            for (int x = 0; x < _grid.Width; x++)
            {
                CellItem baseItem = Instantiate(levelData.CellItem, transform);
                
                Vector2 itemPosition = new Vector2(x * _grid.CellSize, (_topPositionOfBoard - y) * _grid.CellSize);
            
                baseItem.Initialize(new int[] { x, y }, itemPosition, transform, _grid.GetCellByCoordinates(x,y), _gameBuildData.TargetType);
            } 
        }
    }

    #region Getters

    public Grid GetGrid()
    {
        return _grid;
    }

    #endregion
}
