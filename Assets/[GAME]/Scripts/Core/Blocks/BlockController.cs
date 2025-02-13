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
    private bool _isDragging = false;
    private float _dragYOffset = 1f;
    private bool _isFitting = false;
    private bool _canGetInput = true;
    private Vector3 _dragOffset;

    public BlockHelper BlockHelper;
    [SerializeField] private Collider2D[] blockColliders;

    public static event UnityAction<CellItem, Directions> OnBlockPlaced;
    public static event UnityAction OnBlockUsed;

    private void Start()
    {
        _startPosition = transform.position;
    }
    
    
    private void OnMouseDown()
    {
        if (!_canGetInput)
            return;
        
        AudioManager.Instance.PlayAnySound(AudioManager.SoundType.BLOCK_SELECT);
        _isDragging = true;
        _dragOffset = transform.position - GetMouseWorldPosition(); 
    }

    private void OnMouseDrag()
    {
        if (_isDragging && BlockHelper.ItemToPlace)
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
        _isDragging = false;

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
        OnBlockUsed?.Invoke();

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
        if (_isDragging)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            transform.position = new Vector3(
                mousePosition.x + _dragOffset.x, 
                mousePosition.y + _dragYOffset, 
                transform.position.z 
            );
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
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