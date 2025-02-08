using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    [SerializeField] private GameBuildData data; //  Temp

    public Color LevelColor;

    private void Start()
    {
        LevelColor = data.BlockColorForLevel;
    }
}
