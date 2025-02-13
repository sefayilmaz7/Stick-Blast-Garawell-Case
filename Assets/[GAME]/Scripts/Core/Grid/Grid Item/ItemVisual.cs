using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GarawellGames.Managers;
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
        [SerializeField] private CellItem cellItem;

        [SerializeField] private SpriteRenderer upLeftPin;
        [SerializeField] private SpriteRenderer upRightPin;
        [SerializeField] private SpriteRenderer downLeftPin;
        [SerializeField] private SpriteRenderer downRightPin;

        private void Update()
        {
            ColorizeCornersByDirections();
        }

        private void ColorizeCornersByDirections()
        {
            bool isUp = cellItem.CellDirections.Up;
            bool isDown = cellItem.CellDirections.Down;
            bool isLeft = cellItem.CellDirections.Left;
            bool isRight = cellItem.CellDirections.Right;
            
            SetCornerColorAndOrder(upLeftPin, isUp, isLeft);
            SetCornerColorAndOrder(upRightPin, isUp, isRight);
            SetCornerColorAndOrder(downLeftPin, isDown, isLeft);
            SetCornerColorAndOrder(downRightPin, isDown, isRight);
        }
        
        void SetCornerColorAndOrder(SpriteRenderer pin, params bool[] conditions)
        {
            bool isActive = false;
            foreach (bool condition in conditions)
            {
                if (condition)
                {
                    isActive = true;
                    break;
                }
            }
        
            pin.sortingOrder = isActive ? 4 : 3;
            pin.color = isActive ? DarkenColor(ColorManager.Instance.LevelColor) : visualParams.cornerDefaultColor;
        }

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
        
            
        public Color DarkenColor(Color color, float darkenAmount = 0.1f)
        {
            float r = Mathf.Clamp01(color.r - darkenAmount);
            float g = Mathf.Clamp01(color.g - darkenAmount);
            float b = Mathf.Clamp01(color.b - darkenAmount);

            return new Color(r, g, b, color.a);
        }


        [Serializable]
        public struct VisualParams
        {
            public float fillImageEnableTime;
            public Color lineDefaultColor;
            public Color cornerDefaultColor;
        }
    }
}
