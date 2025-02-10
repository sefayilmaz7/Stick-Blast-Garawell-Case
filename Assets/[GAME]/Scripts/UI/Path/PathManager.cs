using System;
using System.Collections;
using System.Collections.Generic;
using GarawellGames.UI;
using UnityEngine;

namespace GarawellGames.Managers
{
    public class PathManager : MonoBehaviour
    {
        [SerializeField] private Color pathColor;
        [SerializeField] private PathRoad[] pathRoads;

        private void Start()
        {
            InitPath();
        }

        private void InitPath()
        {
            if (LevelManager.Instance.CurrentLevel > pathRoads.Length -1)
            {
                foreach (var road in pathRoads)
                {
                    road.ColorizePath(pathColor);
                }
                return;
            }
            
            for (int i = 0; i < LevelManager.Instance.CurrentLevel; i++)
            {
                pathRoads[i].ColorizePath(pathColor);
            }
        }
    }
}
