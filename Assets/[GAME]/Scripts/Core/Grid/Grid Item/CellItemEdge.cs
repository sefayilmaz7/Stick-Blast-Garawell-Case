using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CellItemEdge : MonoBehaviour
{
    [SerializeField] private Color edgeDefaultColor; 
    [SerializeField] private Color edgeSelectedColor;
    [SerializeField] private Color squareDefaultColor;
    [SerializeField] private SpriteRenderer[] effectedSquares;
    
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
        foreach (var square in effectedSquares)
        {
            square.sortingOrder = 4;
            square.color = DarkenColor(color);
        }
    }

    public void ResetCorners()
    {
        foreach (var square in effectedSquares)
        {
            square.sortingOrder = 3;
            square.color = squareDefaultColor;
        }
    }
    
    public Color DarkenColor(Color color, float darkenAmount = 0.1f)
    {
        float r = Mathf.Clamp01(color.r - darkenAmount);
        float g = Mathf.Clamp01(color.g - darkenAmount);
        float b = Mathf.Clamp01(color.b - darkenAmount);

        return new Color(r, g, b, color.a);
    }
}
