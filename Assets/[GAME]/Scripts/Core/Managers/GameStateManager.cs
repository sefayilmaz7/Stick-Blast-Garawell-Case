using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Grid = GarawellGames.Core.Grid;

namespace GarawellGames.Managers
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        public static event UnityAction OnLevelSucces;
        public static event UnityAction OnLevelFailed;

        [SerializeField] private BlocksPanel blocksPanel;

        private Grid _grid;

        private bool _isSucces = false;
        private bool _isFail = false;

        private void Start()
        {
            _grid = GameBuilder.Instance.GetGrid();
        }

        private void CheckGameFail()
        {
            if (_isFail)
                return;

            foreach (var block in blocksPanel.SpawnedBlocks)
            {
                if (block == null)
                {
                    continue;
                }

                foreach (var item in _grid.GetAllItems())
                {
                    if (item is CellItem cellItem && block.BlockHelper.BlockDirections.HasOnlyOneSide)
                    {
                        if (block.CanFitForOneSided(cellItem))
                        {
                            return;
                        }
                    }
                    else if (item is CellItem cellItemm && !block.BlockHelper.BlockDirections.HasOnlyOneSide)
                    {
                        if (block.CanFit(cellItemm))
                        {
                            return;
                        }
                    }
                }
            }

            _isFail = true;
            OnLevelFailed?.Invoke();
        }

        public void InvokeGameSucces()
        {
            if (_isSucces)
                return;

            _isSucces = true;
            OnLevelSucces?.Invoke();
        }

        private void OnEnable()
        {
            BoardController.OnBoardProcessDone += CheckGameFail;
        }

        private void OnDisable()
        {
            BoardController.OnBoardProcessDone -= CheckGameFail;
        }
    }
}