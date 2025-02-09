using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameUIManager : Singleton<InGameUIManager>
{
    [SerializeField] private GameBuildData data; // temp

    [SerializeField] private CanvasGroup succesPanel;
    [SerializeField] private CanvasGroup failPanel;
    [SerializeField] private Image targetImage;
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private Slider targetSlider;

    public static event UnityAction<int, Sprite, TargetItem.TargetType> OnTargetAssigned;

    private void Awake()
    {
        PrepTargetItems();
    }

    private void PrepTargetItems()
    {
        if(!data)
            return;
        
        targetImage.sprite = data.TargetSprite;
        targetText.text = data.TargetAmount.ToString();
        targetSlider.maxValue = data.TargetAmount;
        OnTargetAssigned?.Invoke(data.TargetAmount, data.TargetSprite, data.TargetType);
    }

    private void ShowFailPanel()
    {
        failPanel.gameObject.SetActive(true);
        failPanel.DOFade(1, 0.3f).From(0);
    }

    private void ShowSuccesPanel()
    {
        succesPanel.gameObject.SetActive(true);
        succesPanel.DOFade(1, 0.3f).From(0);
    }

    private void AddProgress(TargetItem.TargetType type)
    {
        targetSlider.DOValue(targetSlider.value + 1, 0.08f).OnComplete(() =>
        {
            if (targetSlider.value >= targetSlider.maxValue)
            {
                Debug.Log("Succes From UI Manager");
                GameStateManager.Instance.InvokeGameSucces();
            }
        });
    }

    private void OnEnable()
    {
        GameStateManager.OnLevelFailed += ShowFailPanel;
        GameStateManager.OnLevelSucces += ShowSuccesPanel;
        CellItemTargetHolder.OnTargetEarned += AddProgress;
    }

    private void OnDisable()
    {
        GameStateManager.OnLevelFailed -= ShowFailPanel;
        GameStateManager.OnLevelSucces -= ShowSuccesPanel;
        CellItemTargetHolder.OnTargetEarned -= AddProgress;
    }
}
