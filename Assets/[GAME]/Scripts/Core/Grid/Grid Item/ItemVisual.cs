using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GarawellGames.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer fillSprite;
        [SerializeField] private VisualParams visualParams;
        [SerializeField] private SpriteRenderer[] _dropEffectSprites;

        public void InitializeVisual(Vector2 position, Transform parent)
        {
            transform.localPosition = position;
            transform.SetParent(parent);
            ColorizeDropEffect();
        }

        private void ColorizeDropEffect()
        {
            foreach (var sprite in _dropEffectSprites)
            {
                sprite.color = ColorManager.Instance.LevelColor;
            }
        }
        public void VisualReset()
        {
            if (!fillSprite) return;
            
            fillSprite.transform.DOScale(0,0.2f);
            CloseUnnecessaryVisual();
            //Do reset here
        }
        
        
        private void CloseUnnecessaryVisual()
        {
            if (fillSprite.transform.childCount > 0)
            {
                foreach (Transform child in fillSprite.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        public void FillCellItem(Color color)
        {
            fillSprite.sortingOrder = 2;
            fillSprite.color = color;
            fillSprite.transform.DOScale(0.9f, visualParams.fillImageEnableTime);
        }

        public void PlayDropEffect()
        {
            foreach (var sprite in _dropEffectSprites)
            {
                var initialPos = sprite.transform.position;
                var initialRotation = sprite.transform.rotation;
                
                sprite.gameObject.SetActive(true);
                sprite.transform.DOMoveY(sprite.transform.position.y - 20, 2.5f);
                sprite.transform
                    .DORotate(new Vector3(0, 0, Random.Range(0, 360)), 2.5f, RotateMode.FastBeyond360)
                    .OnComplete(() =>
                    {
                        sprite.gameObject.SetActive(false);
                        sprite.transform.DOMove(initialPos, 0);
                        sprite.transform.DORotate(initialRotation.eulerAngles, 0);
                    });
            }
        }


        [Serializable]
        public struct VisualParams
        {
            public float fillImageEnableTime;
            public Color lineDefaultColor;
        }
    }
}
