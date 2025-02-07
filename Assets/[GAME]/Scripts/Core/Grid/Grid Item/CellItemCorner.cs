using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellItemCorner : MonoBehaviour
{
    [SerializeField] private Collider2D cornerCollider;
    [SerializeField] private Color cornerDefaultColor; 
    [SerializeField] private Color cornerSelectedColor; 
    
    public CellItem CellItem;

    public void SwitchCornerCollider(bool enable)
    {
        cornerCollider.enabled = enable;
    }

    public void Highlight()
    {
        GetComponent<SpriteRenderer>().color = cornerSelectedColor;
    }

    public void ResetCorner()
    {
        GetComponent<SpriteRenderer>().color = cornerSelectedColor; 
    }
}
