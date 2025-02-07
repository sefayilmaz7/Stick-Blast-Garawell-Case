using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BloksPanel : MonoBehaviour
{
    public GameBuildData data; // temporary it will get from levelmanager

    [SerializeField] private Transform[] blockPlaces; 
    
    
    private BlockHelper[] _availableBlocks;

    private void Awake()
    {
        _availableBlocks = data.LevelBlocks;
    }

    private void Start()
    {
        SpawnBlocks();
    }

    private void SpawnBlocks()
    {
        for (int i = 0; i < 3; i++)
        {
            var blockToSpawn = _availableBlocks[Random.Range(0, _availableBlocks.Length)];
            var spawnedBlock = Instantiate(blockToSpawn, blockPlaces[i].position + new Vector3(blockToSpawn.PlacingOffset.x, blockToSpawn.PlacingOffset.y), Quaternion.identity, blockPlaces[i]);
            foreach (var sprite in spawnedBlock.BlockSprites)
            {
                sprite.color = data.BlockColorForLevel;
            }
        }
    }
}
