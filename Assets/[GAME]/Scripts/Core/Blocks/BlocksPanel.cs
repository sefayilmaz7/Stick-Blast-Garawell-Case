using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BlocksPanel : MonoBehaviour
{
    public List<BlockController> SpawnedBlocks = new List<BlockController>();

    [SerializeField] private Transform[] blockPlaces; 
    
    private BlockHelper[] _availableBlocks;
    private int _activeBlockCount = 3;
    private GameBuildData _data;

    private void Awake()
    {
        _data = LevelManager.Instance.GetLevelData();
        _availableBlocks = _data.LevelBlocks;
    }

    private void Start()
    {
        SpawnBlocks(false);
    }

    private void SpawnBlocks(bool animate)
    {
        SpawnedBlocks.Clear();
        for (int i = 0; i < 3; i++)
        {
            var blockToSpawn = _availableBlocks[Random.Range(0, _availableBlocks.Length)];
            var spawnedBlock = Instantiate(blockToSpawn, blockPlaces[i].position + new Vector3(blockToSpawn.PlacingOffset.x, blockToSpawn.PlacingOffset.y), blockToSpawn.transform.rotation, blockPlaces[i]);
            foreach (var sprite in spawnedBlock.BlockSprites)
            {
                sprite.color = _data.BlockColorForLevel;
            }

            if (animate)
            {
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

    private void OnEnable()
    {
        BlockController.OnBlockUsed += CheckBlockSpawn;
    }

    private void OnDisable()
    {
        BlockController.OnBlockUsed -= CheckBlockSpawn;
    }
}
