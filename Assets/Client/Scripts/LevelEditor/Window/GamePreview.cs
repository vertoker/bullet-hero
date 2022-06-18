using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Game.Provider;
using UnityEngine;
using Game.Core;
using Data;

namespace LevelEditor.Windows
{
    public class GamePreview : Window
    {
        [SerializeField] private SkinsData skinsData;
        [SerializeField] private TimelineRuntime timeline;
        [SerializeField] private Player player;
        private WindowController controller;

        public override void Init(WindowController controller)
        {
            this.controller = controller;
            InitProvider();
        }
        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {

        }

        private void InitProvider()
        {
            SceneManager.sceneLoaded += LoadedProvider;
            SceneManager.LoadScene(1, LoadSceneMode.Additive);

            RestartController();
        }
        private void LoadedProvider(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (loadSceneMode == LoadSceneMode.Additive)
            {
                DataProvider.Start(Level.DEFAULT_LEVEL, player, GameRules.standard);
                timeline.Init(DataProvider.Runtime);
                SceneManager.sceneLoaded -= LoadedProvider;
            }
        }

        public void Play()
        {
            DataProvider.Runtime.StartGame();
        }
        public void Pause()
        {
            DataProvider.Runtime.StopGame();
        }
        public void Restart()
        {
            RestartController();
            DataProvider.Runtime.Restart();
        }

        private void RestartController()
        {
            player.SetPlayerPreset(skinsData.GetSkin(controller.GameRules.skinID), true, 1);
            //healthBar.SetPlayerRules(gameRules.immortality ? 0 : gameRules.lifeCount);
            //inputController.Restart();
        }
    }
}