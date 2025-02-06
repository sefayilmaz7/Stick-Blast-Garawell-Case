using System;
using System.Collections;
using System.Collections.Generic;
using GarawellGames.Core;
using UnityEngine;
using Grid = GarawellGames.Core.Grid;

public class CameraPositionSetter : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float offset = 3;
    [SerializeField] private float yOffset;

    private void SetCamera()
    {
        Grid grid = GameBuilder.Instance.GetGrid();
        float hgt = grid.Height;
        float wdt = grid.Width;

        float x = (wdt - 1) / 2;
        float y = (hgt - 1) / 2 - yOffset;

        camera.orthographicSize = hgt + offset;
        transform.position = new Vector3(x, y, -10);
    }

    private void Start()
    {
        SetCamera();
    }
}