using System;
using System.Collections;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement;

namespace GarawellGames.Managers
{
    public class SceneManager : Singleton<SceneManager>
    {
        [SerializeField] private float splashDelayTime = 1f;

        protected override void AwakeSingleton()
        {
            UnityScene.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
           StartLoadScene();
        }

        void StartLoadScene()
        {
            StartCoroutine(AutoLoadSplashToMenu());
        }

        public void LoadScene(string sceneName)
        {
            //If sceneName is not valid throw error log and abort process
            if(!SceneKeys.IsValidSceneKey(sceneName))
            {
                return;
            }

            //If sceneName is "Splash" start splash scene routine
            if(sceneName == SceneKeys.SPLASH_SCENE)
            {
                Load(sceneName);
                StartCoroutine(AutoLoadSplashToMenu());
                return;
            }

            //Load scene named sceneName
            Load(sceneName);
        }

        private IEnumerator AutoLoadSplashToMenu()
        {
            yield return new WaitForSeconds(splashDelayTime);
            Load(SceneKeys.MAIN_MENU_SCENE);
        }

        private void Load(string sceneName)
        {
            UnityScene.SceneManager.LoadScene(sceneName);
        }

        private void OnSceneLoaded(UnityScene.Scene loadedScene, UnityScene.LoadSceneMode arg1)
        {
            ChangeGameState(loadedScene.name);
        }

        private void ChangeGameState(string loadedScene)
        {
            switch(loadedScene)
            {
                case SceneKeys.SPLASH_SCENE:
                    GameManager.Instance.UpdateGameState(GameState.Splash);
                    break;
                case SceneKeys.MAIN_MENU_SCENE:
                    GameManager.Instance.UpdateGameState(GameState.Menu);
                    break;
                case SceneKeys.IN_GAME_SCENE:
                    GameManager.Instance.UpdateGameState(GameState.Game);
                    break;
            }
        }

        private void OnDisable()
        {
            UnityScene.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
