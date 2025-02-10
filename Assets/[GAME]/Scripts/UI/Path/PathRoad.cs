using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GarawellGames.UI
{
    public class PathRoad : MonoBehaviour
    {
        [SerializeField] private Image[] pathRoadImages;

        public void ColorizePath(Color color)
        {
            foreach (var roadImage in pathRoadImages)
            {
                roadImage.DOColor(color, 1f);
            }
        }
    }
}
