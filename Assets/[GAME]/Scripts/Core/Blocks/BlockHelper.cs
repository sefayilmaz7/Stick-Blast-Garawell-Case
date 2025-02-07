using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockHelper : MonoBehaviour
{
    [SerializeField] private int[] possibleAngles; 
    [SerializeField] private Collider2D[] blockColliders;

    public SpriteRenderer[] BlockSprites;
    public Vector2 PlacingOffset;
    public bool CanPlace;
    public CellItem ItemToPlace;
    
    private void Update()
    {
        CheckPlacement();
    }

    private void CheckPlacement()
    {
        CanPlace = true;

        foreach (var blockCollider in blockColliders)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(blockCollider.transform.position, blockCollider.bounds.size *2, 0);

            bool isValid = false;
            foreach (var collider in colliders)
            {
                if (collider != null && collider.TryGetComponent(out CellItemEdge edge))
                {
                    isValid = true;
                    ItemToPlace = edge.CellItem;
                    break;
                }
            }

            if (!isValid)
            {
                ItemToPlace = null;
                CanPlace = false;
                return;
            }
        }
    }

}
