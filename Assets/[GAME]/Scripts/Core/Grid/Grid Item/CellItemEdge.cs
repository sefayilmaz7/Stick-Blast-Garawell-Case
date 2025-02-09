using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CellItemEdge : MonoBehaviour
{
    [SerializeField] private Color edgeDefaultColor; 
    [SerializeField] private Color edgeSelectedColor; 
    
    public CellItem CellItem;
    public SpriteRenderer EdgeSprite;
    
    public void Highlight()
    {
        EdgeSprite.color = edgeSelectedColor;
    }

    public void ResetEdge()
    {
        EdgeSprite.sortingOrder = 0;
        EdgeSprite.color = edgeDefaultColor; 
    }

    public void ColorizeEdge(Color color)
    {
        EdgeSprite.color = color;
        EdgeSprite.sortingOrder = 1;
    }
}
