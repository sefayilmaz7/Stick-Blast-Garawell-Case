using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    public Color LevelColor;

    private void Start()
    {
        
        LevelColor = LevelManager.Instance.GetLevelData().BlockColorForLevel;
    }
}
