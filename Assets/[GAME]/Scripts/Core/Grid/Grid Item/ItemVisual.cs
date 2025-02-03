using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace GarawellGames.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private VisualParams visualParams;

        public void InitializeVisual(Vector2 position, Transform parent)
        {
            //Custom effects & animations here
            //Or:
            //PlayEffect();
            //Or:
            transform.localPosition = position;
            transform.SetParent(parent);
        }
        public void VisualReset()
        {
            if (!spriteRenderer) return;

            spriteRenderer.color = visualParams.itemDefaultColor;
            CloseUnnecessaryVisual();
            //Do reset here
        }

        public void ShowCircleEffect(TweenCallback onComplete)
        {
            visualParams.circleObjectImage.gameObject.SetActive(true);
            visualParams.circleObjectImage.transform.DOScale(2, 0.2f).From(0).OnComplete(onComplete.Invoke);
        }
        
        public void ShowCircleEffectWithoutTime()
        {
            visualParams.circleObjectImage.gameObject.SetActive(true);
            visualParams.circleObjectImage.transform.DOScale(2, 0f);
        }

        public void ShowEraseEffect(TweenCallback onComplete)
        {
            spriteRenderer.DOColor(visualParams.itemEraseColor, 0.2f).OnComplete(() =>
            {
                spriteRenderer.DOColor(visualParams.itemDefaultColor, 0.2f);
                onComplete.Invoke();
            });
        }
        
        private void CloseUnnecessaryVisual()
        {
            if (spriteRenderer.transform.childCount > 0)
            {
                foreach (Transform child in spriteRenderer.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }


        [Serializable]
        public struct VisualParams
        {
            public SpriteRenderer circleObjectImage;
            public Color itemDefaultColor;
            public Color itemEraseColor;
            public Color itemWrongSolveColor;
            public Color solvedTextColor;
            public Color solvedTextColorCircle;
            public Color rowColumnCleaningColor;
            public Sprite titleSolvedSprite;
            public GameObject tick;
        }
    }
}
