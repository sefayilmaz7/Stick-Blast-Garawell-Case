using System;
using System.Collections;
using System.Collections.Generic;
using GarawellGames.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace GarawellGames.UI
{
    public class SuccesWindow : MonoBehaviour
    {
        [SerializeField] private Button nextLevelButton;

        private void Awake()
        {
            nextLevelButton.onClick.AddListener(PlayNextLevel);
        }

        private void PlayNextLevel()
        {
            LevelManager.Instance.LevelUp();
            SceneManager.Instance.LoadScene(SceneKeys.IN_GAME_SCENE);
        }

        private void OnEnable()
        {
            AudioManager.Instance.PlayAnySound(AudioManager.SoundType.SUCCES);
        }
    }
}
