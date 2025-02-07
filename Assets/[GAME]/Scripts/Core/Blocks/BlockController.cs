using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Vector3 _startPosition;
    private bool _isDragging = false;
    private float _dragYOffset = 1f;
    private bool _isFitting = false;

    [SerializeField] private BlockHelper blockHelper;
    [SerializeField] private Collider2D[] blockColliders;

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

            // Eğer sadece Left ve Right seçiliyse
            if (blockHelper.BlockDirections.Left || blockHelper.BlockDirections.Right &&
                !blockHelper.BlockDirections.Up && !blockHelper.BlockDirections.Down)
            {
                if (localItemPosition.x > 0) // Eğer sol
                {
                    item.ResetEdges();
                    blockHelper.BlockDirections.Right = false;
                    blockHelper.BlockDirections.Left = true;
                    item.HighlightEdge(blockHelper.BlockDirections);
                }
                else if (localItemPosition.x < 0) // Eğer sağ
                {
                    item.ResetEdges();
                    blockHelper.BlockDirections.Right = true;
                    blockHelper.BlockDirections.Left = false;
                    item.HighlightEdge(blockHelper.BlockDirections);
                }
            }
            // Eğer sadece Up ve Down seçiliyse
            else if (blockHelper.BlockDirections.Up || blockHelper.BlockDirections.Down &&
                     !blockHelper.BlockDirections.Left && !blockHelper.BlockDirections.Right)
            {
                if (localItemPosition.y > 0) // Eğer yukarıdaysa
                {
                    item.ResetEdges();
                    blockHelper.BlockDirections.Up = false;
                    blockHelper.BlockDirections.Down = true;
                    item.HighlightEdge(blockHelper.BlockDirections);
                }
                else if (localItemPosition.y < 0) // Eğer aşağıdaysa
                {
                    item.ResetEdges();
                    blockHelper.BlockDirections.Up = true;
                    blockHelper.BlockDirections.Down = false;
                    item.HighlightEdge(blockHelper.BlockDirections);
                }
            }
        }

        return (blockHelper.BlockDirections.Up && !item.CellDirections.Up) ||
               (blockHelper.BlockDirections.Down && !item.CellDirections.Down) ||
               (blockHelper.BlockDirections.Right && !item.CellDirections.Right) ||
               (blockHelper.BlockDirections.Left && !item.CellDirections.Left);
    }

    private void PlaceBlock(CellItem item)
    {
        DisableColliders();
        if (blockHelper.BlockDirections.HasOnlyOneSide)
        {
            transform.DOMove(item.GetEdgeItemByDirection(blockHelper.BlockDirections).transform.position, 0.5f).SetEase(Ease.OutBack);
            return;
        }
        transform.DOMove(item.transform.position, 0.5f).SetEase(Ease.OutBack);
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