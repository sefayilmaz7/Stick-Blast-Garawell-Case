using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BlockController : MonoBehaviour
{
    private Vector3 _startPosition;
    private bool _isDragging = false;
    private float _dragYOffset = 1f;
    private bool _isFitting = false;

    [SerializeField] private BlockHelper blockHelper;
    [SerializeField] private Collider2D[] blockColliders;

    public static event UnityAction<CellItem, Directions> OnBlockPlaced; 
    public static event UnityAction OnBlockUsed; 

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnMouseDown()
    {
        _isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (_isDragging && blockHelper.ItemToPlace)
        {
            _isFitting = CanFit(blockHelper.ItemToPlace);
            if (CanFit(blockHelper.ItemToPlace))
            {
                blockHelper.ItemToPlace.HighlightEdge(blockHelper.BlockDirections);
            }
        }
        else
        {
            _isFitting = false;
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        
        if (blockHelper.CanPlace && !_isFitting)
        {
            blockHelper.ItemToPlace.ResetEdges();
        }
        
        if (blockHelper.CanPlace && _isFitting)
        {
            PlaceBlock(blockHelper.ItemToPlace);
            return;
        }

        transform.DOMove(_startPosition, 0.5f).SetEase(Ease.OutBack);
    }

    private bool CanFit(CellItem item)
    {
        if (item == null)
            return false;

        if (blockHelper.BlockDirections.HasOnlyOneSide)
        {
            Vector3 localItemPosition = transform.InverseTransformPoint(item.transform.position);
            
            if (blockHelper.BlockDirections.Left || blockHelper.BlockDirections.Right &&
                !blockHelper.BlockDirections.Up && !blockHelper.BlockDirections.Down)
            {
                if (localItemPosition.x > 0) //Left
                {
                    item.ResetEdges();
                    blockHelper.BlockDirections.Right = false;
                    blockHelper.BlockDirections.Left = true;
                    item.HighlightEdge(blockHelper.BlockDirections);
                }
                else if (localItemPosition.x < 0) //Right
                {
                    item.ResetEdges();
                    blockHelper.BlockDirections.Right = true;
                    blockHelper.BlockDirections.Left = false;
                    item.HighlightEdge(blockHelper.BlockDirections);
                }
            }
            else if (blockHelper.BlockDirections.Up || blockHelper.BlockDirections.Down &&
                     !blockHelper.BlockDirections.Left && !blockHelper.BlockDirections.Right)
            {
                if (localItemPosition.y > 0) //Up
                {
                    item.ResetEdges();
                    blockHelper.BlockDirections.Up = false;
                    blockHelper.BlockDirections.Down = true;
                    item.HighlightEdge(blockHelper.BlockDirections);
                }
                else if (localItemPosition.y < 0) //Down
                {
                    item.ResetEdges();
                    blockHelper.BlockDirections.Up = true;
                    blockHelper.BlockDirections.Down = false;
                    item.HighlightEdge(blockHelper.BlockDirections);
                }
            }
        }

        // **Yeni Doğru Return Mantığı**
        if ((blockHelper.BlockDirections.Up && item.CellDirections.Up) ||
            (blockHelper.BlockDirections.Down && item.CellDirections.Down) ||
            (blockHelper.BlockDirections.Right && item.CellDirections.Right) ||
            (blockHelper.BlockDirections.Left && item.CellDirections.Left))
        {
            return false; // **Eğer herhangi bir yön eşleşiyorsa false döndür**
        }

        return true; // **Eğer hiç eşleşme yoksa true döndür**
    }

    private void PlaceBlock(CellItem item)
    {
        DisableColliders();
        if (blockHelper.BlockDirections.HasOnlyOneSide)
        {
            transform.DOMove(item.GetEdgeItemByDirection(blockHelper.BlockDirections).transform.position, 0.5f)
                .SetEase(Ease.OutBack).OnComplete(() =>
                {
                    transform.SetParent(item.transform);
                });
            
        }
        else
        {
            transform.DOMove(item.transform.position, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                transform.SetParent(item.transform);
            });
        }
        
        item.ResetEdges();
        DOVirtual.DelayedCall(0.5f, () => OnBlockPlaced?.Invoke(item, blockHelper.BlockDirections));
        OnBlockUsed?.Invoke();
        item.AddBlockToList(gameObject);
    }

    private void DisableColliders()
    {
        foreach (var collider in blockColliders)
        {
            collider.enabled = false;
        }
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            transform.position = new Vector3(mousePosition.x, mousePosition.y + _dragYOffset, transform.position.z);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}