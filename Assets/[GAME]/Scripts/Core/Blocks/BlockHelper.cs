using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockHelper : MonoBehaviour
{
    [SerializeField] private int[] possibleAngles; 
    [SerializeField] private BlockCollider[] blockColliders;

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
            Collider2D[] colliders = Physics2D.OverlapBoxAll(blockCollider.transform.position, blockCollider.GetComponent<Collider2D>().bounds.size *2, 0);

            bool isValid = false;
            foreach (var collider in colliders)
            {
                if (collider != null && collider.TryGetComponent(out CellItemCorner cellItemCorner))
                {
                    isValid = true;
                    ItemToPlace = cellItemCorner.CellItem;
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
