using System;
using System.Collections;
using System.Collections.Generic;
using GarawellGames.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace GarawellGames.UI
{
    public class FailWindow : MonoBehaviour
    {
        [SerializeField] private Button tryAgainButton;

        private void Awake()
        {
            tryAgainButton.onClick.AddListener(PlayAgain);
        }

        private void PlayAgain()
        {
            SceneManager.Instance.LoadScene(SceneKeys.IN_GAME_SCENE);
        }

        private void OnEnable()
        {
            AudioManager.Instance.PlayAnySound(AudioManager.SoundType.FAIL);
        }
    }
}
