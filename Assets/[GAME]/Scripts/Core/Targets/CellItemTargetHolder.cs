using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CellItemTargetHolder : MonoBehaviour
{
    public static event UnityAction<TargetItem.TargetType> OnTargetEarned; 
    public bool HasTargetItem = false;

    [SerializeField] private SpriteRenderer targetItemSprite;

    private TargetItem.TargetType _currentTargetType;

    private void Start()
    {
        GameBuildData data = GameBuilder.Instance.GetData();
        SetTargetItemValues(data.TargetAmount, data.TargetSprite, data.TargetType);
    }

    private void SetTargetItemValues(int amount, Sprite targetSprite, TargetItem.TargetType type)
    {
        targetItemSprite.sprite = targetSprite;
        _currentTargetType = type;
    }

    public void SpawnTargetItem()
    {
        /*if (HasTargetItem || _currentTargetType == TargetItem.TargetType.Score)
            return;*/

        HasTargetItem = true;
        targetItemSprite.transform.DOScale(0.5f, 0.2f);
    }

    public void EarnTarget()
    {
        if (_currentTargetType == TargetItem.TargetType.Score)
        {
            OnTargetEarned?.Invoke(TargetItem.TargetType.Score);
        }
        else if (_currentTargetType != TargetItem.TargetType.Score && HasTargetItem)
        {
            var itemToMove = Instantiate(targetItemSprite.gameObject, transform);
            InGameUIManager.Instance.MoveSpriteToTarget(itemToMove.GetComponent<SpriteRenderer>());
            targetItemSprite.transform.DOScale(0, 0);
            HasTargetItem = false;
            OnTargetEarned?.Invoke(_currentTargetType);
        }
    }
    
}
