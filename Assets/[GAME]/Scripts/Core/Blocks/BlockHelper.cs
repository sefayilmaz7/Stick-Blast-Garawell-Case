using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHelper : MonoBehaviour
{
    [SerializeField] private int[] possibleAngles; // For Random Z Rotation of the Block
    [SerializeField] private BlockCollider[] blockColliders;
    [SerializeField] private SpriteRenderer[] blockSprites;
    [SerializeField] private Vector2 placingOffset;

    //  threshold L block -> x -0.5
    //  threshold U block -> y -0.5
    //  threshold I block -> x -0.5
}
