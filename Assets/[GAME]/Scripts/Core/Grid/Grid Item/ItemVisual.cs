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
        [SerializeField] private GameObject diamond;
        [SerializeField] private GameObject gold;
        [SerializeField] private GameObject ruby;

        public void InitializeVisual(Vector2 position, Transform parent, TargetItem.TargetType type = TargetItem.TargetType.Score)
        {
            transform.localPosition = position;
            transform.SetParent(parent);
            if (type != TargetItem.TargetType.Score)
            {
                AssignTargetItemVisual(type);
            }
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

        public void AssignTargetItemVisual(TargetItem.TargetType type)
        {
            switch (type)
            {
                case TargetItem.TargetType.Diamond:
                    diamond.SetActive(true);
                    break;
                case TargetItem.TargetType.Gold:
                    gold.SetActive(true);
                    break;
                case TargetItem.TargetType.Ruby:
                    ruby.SetActive(true);
                    break;
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
            public Color lineDefaultColor;
        }
    }
}
