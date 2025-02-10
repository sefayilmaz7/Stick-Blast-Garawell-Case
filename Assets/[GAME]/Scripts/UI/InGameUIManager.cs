using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GarawellGames.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GarawellGames.Managers
{
    public class InGameUIManager : Singleton<InGameUIManager>
    {
        private GameBuildData _data;

        [SerializeField] private CanvasGroup succesPanel;
        [SerializeField] private CanvasGroup failPanel;
        [SerializeField] private Image targetImage;
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private Slider _targetSlider;


        protected override void AwakeSingleton()
        {
            base.AwakeSingleton();
            _data = LevelManager.Instance.GetLevelData();
            PrepTargetItems();
        }

        private void PrepTargetItems()
        {
            if (!_data)
                return;

            targetImage.sprite = _data.TargetSprite;
            targetText.text = _data.TargetAmount.ToString();
            _targetSlider.maxValue = _data.TargetAmount;
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
            _targetSlider.DOValue(_targetSlider.value + 1, 0.08f).OnComplete(() =>
            {
                if (_targetSlider.value >= _targetSlider.maxValue)
                {
                    GameStateManager.Instance.InvokeGameSucces();
                }
            });
        }

        public void MoveSpriteToTarget(SpriteRenderer spriteRenderer)
        {
            DecreaseTarget();
            spriteRenderer.transform.SetParent(null);
            Vector3 targetWorldPos = _targetSlider.transform.position;


            targetWorldPos = Camera.main.ScreenToWorldPoint(targetWorldPos);

            spriteRenderer.transform.DOScale(0.2f, 0.5f);
            spriteRenderer.transform.DOLocalMove(targetWorldPos, 0.3f).SetEase(Ease.InOutQuad);
        }

        public void DecreaseTarget()
        {
            int currentValue = int.Parse(targetText.text);
            currentValue = Mathf.Max(0, currentValue - 1);
            targetText.text = currentValue.ToString();
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
}