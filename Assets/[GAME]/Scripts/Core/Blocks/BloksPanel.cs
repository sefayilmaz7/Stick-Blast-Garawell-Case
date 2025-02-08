using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class BloksPanel : MonoBehaviour
{
    public GameBuildData data; // temporary it will get from levelmanager

    [SerializeField] private Transform[] blockPlaces; 
    
    private BlockHelper[] _availableBlocks;
    private int _activeBlockCount = 3;

    private void Awake()
    {
        _availableBlocks = data.LevelBlocks;
    }

    private void Start()
    {
        SpawnBlocks(false);
    }

    private void SpawnBlocks(bool animate)
    {
        for (int i = 0; i < 3; i++)
        {
            var blockToSpawn = _availableBlocks[Random.Range(0, _availableBlocks.Length)];
            var spawnedBlock = Instantiate(blockToSpawn, blockPlaces[i].position + new Vector3(blockToSpawn.PlacingOffset.x, blockToSpawn.PlacingOffset.y), blockToSpawn.transform.rotation, blockPlaces[i]);
            foreach (var sprite in spawnedBlock.BlockSprites)
            {
                sprite.color = data.BlockColorForLevel;
            }

            if (animate)
            {
                spawnedBlock.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 0, 0.3f);
            }
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
