using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockHelper : MonoBehaviour
{
    [SerializeField] private Collider2D blockCollider;
    [SerializeField] private LayerMask cellItemLayerMask;

    public SpriteRenderer[] BlockSprites;
    public Vector2 PlacingOffset;
    public bool CanPlace;
    public CellItem ItemToPlace;
    public Directions BlockDirections;
    
    
    
    private void FixedUpdate()
    {
        CheckPlacement();
    }

    private void CheckPlacement()
    {
        CanPlace = false;
        
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.7f,0.7f), 0, cellItemLayerMask);
    
        if (collider != null && collider.TryGetComponent(out CellItem cellItem))
        {
            CanPlace = true;
            if (ItemToPlace && cellItem != ItemToPlace)
            {
                ItemToPlace.ResetEdges();
            }
            ItemToPlace = cellItem;
        }
        else
        {
            if (ItemToPlace)
            {
                ItemToPlace.ResetEdges();
            }
            ItemToPlace = null;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (blockCollider == null)
            return;

        // Gizmo rengini belirle
        Gizmos.color = Color.red;

        // OverlapBox'ın boyutunu al
        Vector2 size = new Vector2(0.7f,0.7f);

        // Merkez noktasını belirle
        Vector3 position = transform.position;

        // Kutuyu çiz (Z rotasyonu olmadığı için sadece X ve Y kullanıyoruz)
        Gizmos.DrawWireCube(position, size);
    }
}

