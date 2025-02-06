using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GarawellGames.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer fillSprite;
        [SerializeField] private VisualParams visualParams;

        public void InitializeVisual(Vector2 position, Transform parent)
        {
            transform.localPosition = position;
            transform.SetParent(parent);
        }
        public void VisualReset()
        {
            if (!fillSprite) return;

            fillSprite.transform.DOScale(0,0);
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
            fillSprite.color = color;
            fillSprite.transform.DOScale(1, visualParams.fillImageEnableTime);
        }


        [Serializable]
        public struct VisualParams
        {
            public float fillImageEnableTime;
        }
    }
}
