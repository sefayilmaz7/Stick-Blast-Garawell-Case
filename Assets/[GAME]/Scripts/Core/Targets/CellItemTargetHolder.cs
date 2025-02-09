using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CellItemTargetHolder : MonoBehaviour
{
    public bool HasTargetItem = false;

    [SerializeField] private SpriteRenderer targetItemImage;

    private TargetItem.TargetType _currentTargetType;

    private void SetTargetItemValues(int amount, Sprite targetSprite, TargetItem.TargetType type)
    {
        targetItemImage.sprite = targetSprite;
        _currentTargetType = type;
    }

    private void SpawnTargetItem()
    {
        if (HasTargetItem || _currentTargetType == TargetItem.TargetType.Score)
            return;

        HasTargetItem = true;
        targetItemImage.transform.DOScale(0.5f, 0.2f);
    }

    private void OnEnable()
    {
        InGameUIManager.OnTargetAssigned += SetTargetItemValues;
    }

    private void OnDisable()
    {
        InGameUIManager.OnTargetAssigned -= SetTargetItemValues;
    }
}
