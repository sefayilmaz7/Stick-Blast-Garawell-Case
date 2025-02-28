using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GarawellGames.Managers;
using UnityEngine;
using UnityEngine.Events;

public class BlockController : MonoBehaviour
{
    private Vector3 _startPosition;
    public bool IsDragging = false;
    private float _dragYOffset = 1f;
    private bool _isFitting = false;
    private bool _canGetInput = true;
    private Vector3 _dragOffset;
    private float _smoothTime = 0.05f; 
    private Vector3 _velocity = Vector3.zero;

    public BlockHelper BlockHelper;
    [SerializeField] private Collider2D[] blockColliders;

    public static event UnityAction<CellItem, Directions> OnBlockPlaced;

    private void Start()
    {
        _startPosition = transform.position;
    }
    
    
    private void OnMouseDown()
    {
        if (!_canGetInput)
            return;
        
        AudioManager.Instance.PlayAnySound(AudioManager.SoundType.BLOCK_SELECT);
        Vector3 mousePosition = GetMouseWorldPosition();
        _dragOffset = transform.position - mousePosition;
        IsDragging = true;
    }

    private void OnMouseDrag()
    {
        if (IsDragging && BlockHelper.ItemToPlace)
        {
            _isFitting = CanFit(BlockHelper.ItemToPlace);
            if (CanFit(BlockHelper.ItemToPlace))
            {
                BlockHelper.ItemToPlace.HighlightEdge(BlockHelper.BlockDirections);
            }
        }
        else
        {
            _isFitting = false;
        }
    }

    private void OnMouseUp()
    {
        IsDragging = false;

        if (BlockHelper.CanPlace && !_isFitting)
        {
            BlockHelper.ItemToPlace.ResetEdges();
        }

        if (BlockHelper.CanPlace && _isFitting)
        {
            PlaceBlock(BlockHelper.ItemToPlace);
            return;
        }

        transform.DOMove(_startPosition, 0.5f).SetEase(Ease.OutBack);
    }

    public bool CanFit(CellItem item)
    {
        if (item == null)
            return false;

        if (BlockHelper.BlockDirections.HasOnlyOneSide)
        {
            Vector3 localItemPosition = transform.InverseTransformPoint(item.transform.position);

            if (BlockHelper.BlockDirections.Left || BlockHelper.BlockDirections.Right &&
                !BlockHelper.BlockDirections.Up && !BlockHelper.BlockDirections.Down)
            {
                if (localItemPosition.x > 0 && !item.CellDirections.Left) //Left
                {
                    item.ResetEdges();
                    BlockHelper.BlockDirections.Right = false;
                    BlockHelper.BlockDirections.Left = true;
                    item.HighlightEdge(BlockHelper.BlockDirections);
                }
                else if (localItemPosition.x < 0 && !item.CellDirections.Right) //Right
                {
                    item.ResetEdges();
                    BlockHelper.BlockDirections.Right = true;
                    BlockHelper.BlockDirections.Left = false;
                    item.HighlightEdge(BlockHelper.BlockDirections);
                }
            }
            else if (BlockHelper.BlockDirections.Up || BlockHelper.BlockDirections.Down &&
                     !BlockHelper.BlockDirections.Left && !BlockHelper.BlockDirections.Right)
            {
                if (localItemPosition.y > 0 && !item.CellDirections.Down) //Up
                {
                    item.ResetEdges();
                    BlockHelper.BlockDirections.Up = false;
                    BlockHelper.BlockDirections.Down = true;
                    item.HighlightEdge(BlockHelper.BlockDirections);
                }
                else if (localItemPosition.y < 0 && !item.CellDirections.Up) //Down
                {
                    item.ResetEdges();
                    BlockHelper.BlockDirections.Up = true;
                    BlockHelper.BlockDirections.Down = false;
                    item.HighlightEdge(BlockHelper.BlockDirections);
                }
            }
        }

        if ((BlockHelper.BlockDirections.Up && item.CellDirections.Up) ||
            (BlockHelper.BlockDirections.Down && item.CellDirections.Down) ||
            (BlockHelper.BlockDirections.Right && item.CellDirections.Right) ||
            (BlockHelper.BlockDirections.Left && item.CellDirections.Left))
        {
            return false;
        }

        return true;
    }

    public bool CanFitForOneSided(CellItem item)
    {
        if (BlockHelper.BlockDirections.Right || BlockHelper.BlockDirections.Left)
        {
            if (!item.CellDirections.Right || !item.CellDirections.Left)
            {
                return true;
            }
        }

        if (BlockHelper.BlockDirections.Up || BlockHelper.BlockDirections.Down)
        {
            if (!item.CellDirections.Up || !item.CellDirections.Down)
            {
                return true;
            }
        }

        return false;
    }

    private void PlaceBlock(CellItem item)
    {
        DisableColliders();
        if (BlockHelper.BlockDirections.HasOnlyOneSide)
        {
            transform.DOMove(item.GetEdgeItemByDirection(BlockHelper.BlockDirections).transform.position, 0.25f)
                .SetEase(Ease.OutBack).OnComplete(() =>
                {
                    AudioManager.Instance.PlayAnySound(AudioManager.SoundType.BLOCK_PLACED);
                    transform.SetParent(item.transform);
                });
        }
        else
        {
            transform.DOMove(item.transform.position, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                AudioManager.Instance.PlayAnySound(AudioManager.SoundType.BLOCK_PLACED);
                transform.SetParent(item.transform);
            });
        }

        item.ResetEdges();

        DOVirtual.DelayedCall(0.26f, () =>
        {
            OnBlockPlaced?.Invoke(item, BlockHelper.BlockDirections);
            Destroy(gameObject);
        });
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
        if (IsDragging)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 targetPosition = new Vector3(
                mousePosition.x + _dragOffset.x, 
                mousePosition.y + _dragOffset.y + _dragYOffset, 
                transform.position.z
            );
            
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z; 
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void DisableInput()
    {
        _canGetInput = false;
    }

    private void OnEnable()
    {
        GameStateManager.OnLevelSucces += DisableInput;
        GameStateManager.OnLevelFailed += DisableInput;
    }

    private void OnDisable()
    {
        GameStateManager.OnLevelSucces -= DisableInput;
        GameStateManager.OnLevelFailed -= DisableInput;
    }
}