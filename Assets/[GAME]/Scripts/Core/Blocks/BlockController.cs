using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Vector3 _startPosition;
    private bool _isDragging = false;
    private float _dragYOffset = 1f;

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

    private void OnMouseUp()
    {
        _isDragging = false;
        if (blockHelper.CanPlace)
        {
            PlaceBlock(blockHelper.ItemToPlace);
            return;
        }
        transform.DOMove(_startPosition, 0.5f).SetEase(Ease.OutBack);
    }

    private void PlaceBlock(CellItem item)
    {
        DisableColliders();
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