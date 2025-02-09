using System;
using System.Collections;
using System.Collections.Generic;
using GarawellGames.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GarawellGames.UI
{
    public class MainMenuPlayButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonText;
        [SerializeField] private Button playButton;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            buttonText.text = "LEVEL " + LevelManager.Instance.CurrentLevel;
            playButton.onClick.AddListener(()=>
            {
                AudioManager.Instance.PlayAnySound(AudioManager.SoundType.BUTTON_CLICK);
                SceneManager.Instance.LoadScene(SceneKeys.IN_GAME_SCENE);
            });
        }
    }
}
