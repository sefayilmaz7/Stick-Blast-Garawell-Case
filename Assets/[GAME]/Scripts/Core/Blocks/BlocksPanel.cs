using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GarawellGames.Managers;
using UnityEngine;
using UnityEngine.Events;
using Grid = GarawellGames.Core.Grid;
using Random = UnityEngine.Random;

public class BlocksPanel : MonoBehaviour
{
    public List<BlockController> SpawnedBlocks = new List<BlockController>();

    [SerializeField] private Transform[] blockPlaces;

    private BlockHelper[] _availableBlocks;
    private int _activeBlockCount = 3;
    private GameBuildData _data;
    private Grid _grid;

    private void Awake()
    {
        _data = LevelManager.Instance.GetLevelData();
        _grid = GameBuilder.Instance.GetGrid();
        _availableBlocks = _data.LevelBlocks;
    }

    private void Start()
    {
        SpawnBlocks(false);
    }

    private void SpawnBlocks(bool animate)
    {
        SpawnedBlocks.Clear();
        var possibleBlocks = GetRandomPossibleBlocks();

        for (int i = 0; i < 3; i++)
        {
            var blockToSpawn = possibleBlocks[Random.Range(0, possibleBlocks.Count)].BlockHelper;
            var spawnedBlock = Instantiate(blockToSpawn,
                blockPlaces[i].position + new Vector3(blockToSpawn.PlacingOffset.x, blockToSpawn.PlacingOffset.y),
                blockToSpawn.transform.rotation, blockPlaces[i]);
            foreach (var sprite in spawnedBlock.BlockSprites)
            {
                sprite.color = _data.BlockColorForLevel;
            }

            if (animate)
            {
                AudioManager.Instance.PlayAnySound(AudioManager.SoundType.SPAWN_BLOCK);
                spawnedBlock.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 0, 0.3f);
            }

            SpawnedBlocks.Add(spawnedBlock.GetComponent<BlockController>());
        }
    }

    private void CheckBlockSpawn()
    {
        _activeBlockCount--;
        if (_activeBlockCount == 0)
        {
            SpawnBlocks(true);
            _activeBlockCount = 3;
        }
    }

    private List<BlockController> GetRandomPossibleBlocks()
    {
        List<BlockController> possibleRandomBlocks = new List<BlockController>();
        List<CellItem> unfilledCells = _grid.GetUnFilledItems();

        if (unfilledCells == null || unfilledCells.Count == 0)
        {
            foreach (var helper in _availableBlocks)
            {
                possibleRandomBlocks.Add(helper.Controller);
            }

            return possibleRandomBlocks; //  Return default 
        }

        for (int i = 0; i < 3; i++)
        {
            CellItem randomCell = unfilledCells[Random.Range(0, unfilledCells.Count)];
            BlockHelper selectedBlock = GetRandomAvailableBlockByDirection(randomCell.CellDirections);

            if (selectedBlock != null)
            {
                possibleRandomBlocks.Add(selectedBlock.Controller);
            }
        }

        return possibleRandomBlocks;
    }

    private BlockHelper GetRandomAvailableBlockByDirection(Directions directions)
    {
        var validBlocks = _availableBlocks
            .Where(block =>
                block.BlockDirections.HasOnlyOneSide
                    ? (
                        (block.BlockDirections.Right && block.BlockDirections.Left)
                            ? (!directions.Right || !directions.Left)
                            // ReSharper disable once SimplifyConditionalTernaryExpression
                            : (block.BlockDirections.Up && block.BlockDirections.Down)
                                ? (!directions.Up || !directions.Down)
                                : false
                    )
                    : (
                        (block.BlockDirections.Up ? !directions.Up : true) &&
                        (block.BlockDirections.Down ? !directions.Down : true) &&
                        (block.BlockDirections.Left ? !directions.Left : true) &&
                        (block.BlockDirections.Right ? !directions.Right : true)
                    )
            )
            .ToList();

        if (validBlocks.Count == 0)
        {
            Debug.LogWarning("No valid blocks found for given directions!");
            return null;
        }
        
        return validBlocks[Random.Range(0,validBlocks.Count)];
    }


    private void OnEnable()
    {
        BlockController.OnBlockUsed += CheckBlockSpawn;
    }

    private void OnDisable()
    {
        BlockController.OnBlockUsed -= CheckBlockSpawn;
    }
}