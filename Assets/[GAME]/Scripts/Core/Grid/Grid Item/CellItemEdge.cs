using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CellItemEdge : MonoBehaviour
{
    [SerializeField] private Collider2D edgeCollider;
    [SerializeField] private Color edgeDefaultColor; 
    [SerializeField] private Color edgeSelectedColor; 
    
    public CellItem CellItem;

    public void SwitchEdgeCollider(bool enable)
    {
        edgeCollider.enabled = enable;
    }

    public void Highlight()
    {
        GetComponent<SpriteRenderer>().color = edgeSelectedColor;
    }

    public void ResetCorner()
    {
        GetComponent<SpriteRenderer>().color = edgeSelectedColor; 
    }
}
