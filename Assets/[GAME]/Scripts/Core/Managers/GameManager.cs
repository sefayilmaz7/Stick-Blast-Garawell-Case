using UnityEngine;
using System;

namespace GarawellGames.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public event Action<GameState> GameStateUpdated;
        public GameState CurrentGameState;

        private void OnEnable()
        {
            Application.targetFrameRate = 180;
            UpdateGameState(GameState.Splash);
        }

        /// <param name="NewState"></param>
        public void UpdateGameState(GameState NewState)
        {
            CurrentGameState = NewState;
            GameStateUpdated?.Invoke(CurrentGameState);
        }

    }
}

public enum GameState
{
    Splash,
    Menu,
    Game
}
