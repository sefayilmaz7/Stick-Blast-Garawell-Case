using System;
using System.Collections;
using System.Collections.Generic;
using GarawellGames.Core;
using GarawellGames.Managers;
using UnityEngine;
using Grid = GarawellGames.Core.Grid;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float offset = 3;
    [SerializeField] private float yOffset;
    [SerializeField] private Transform blockHolderTransform;

    [Header("Camera Shake Values")] 
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;
    
    public IEnumerator Shake()
    {
        AudioManager.Instance.PlayAnySound(AudioManager.SoundType.ROW_COLUMN_FILLED);
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
    
    private void StartShake()
    {
        StartCoroutine(Shake());
    }
    
    private void SetCamera()
    {
        Grid grid = GameBuilder.Instance.GetGrid();
        float hgt = grid.Height;
        float wdt = grid.Width;

        float x = (wdt - 1) / 2;
        float y = (hgt - 1) / 2 - yOffset;

        camera.orthographicSize = hgt + offset;
        transform.position = new Vector3(x, y, -10);
        
        
        if (camera != null)
        {
            float pixelsToUnits = 300f / Screen.height * (2f * camera.orthographicSize);
            
            float bottomY = camera.transform.position.y - camera.orthographicSize;
            
            blockHolderTransform.position = new Vector3(camera.transform.position.x, bottomY + pixelsToUnits, blockHolderTransform.position.z);
        }
    
    }

    private void Start()
    {
        SetCamera();
    }

    private void OnEnable()
    {
        BoardController.OnRowOrColumnCleared += StartShake;
    }
    
    private void OnDisable()
    {
        BoardController.OnRowOrColumnCleared -= StartShake;
    }
}