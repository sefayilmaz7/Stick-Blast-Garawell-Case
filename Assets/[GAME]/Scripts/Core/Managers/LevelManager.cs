using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarawellGames.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        public int CurrentLevel = 1;

        [SerializeField] private List<GameBuildData> allLevels = new List<GameBuildData>();

        protected override void AwakeSingleton()
        {
            base.AwakeSingleton();
            GetCurrentLevel();
        }

        private void GetCurrentLevel()
        {
            if (PlayerPrefs.HasKey(PrefKeys.PLAYER_LEVEL))
            {
                CurrentLevel = PlayerPrefs.GetInt(PrefKeys.PLAYER_LEVEL);
            }
            else
            {
                PlayerPrefs.SetInt(PrefKeys.PLAYER_LEVEL, 1);
            }
        }

        public void LevelUp()
        {
            CurrentLevel++;
            PlayerPrefs.SetInt(PrefKeys.PLAYER_LEVEL, CurrentLevel);
        }

        public GameBuildData GetLevelData()
        {
            GameBuildData currentData = allLevels[CurrentLevel - 1];

            return currentData == null ? allLevels[Random.Range(0, allLevels.Count - 1)] : currentData;
        }
    }

}