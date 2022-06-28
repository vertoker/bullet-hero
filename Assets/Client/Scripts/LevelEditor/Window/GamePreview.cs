using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using Game.Provider;
using UnityEngine;
using Game.Core;
using Data;

namespace LevelEditor.Windows
{
    public class GamePreview : Window
    {
        [SerializeField] private Camera previewCam;
        [SerializeField] private RawImage preview;
        [SerializeField] private SkinsData skinsData;
        [SerializeField] private Player player;

        private static UnityEvent<Runtime> initEvent = new UnityEvent<Runtime>();
        public static event UnityAction<Runtime> InitEvent
        {
            add => initEvent.AddListener(value);
            remove => initEvent.RemoveListener(value);
        }

        public override void Init()
        {
            InitProvider();
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
                DataProvider.Start(LevelHolder.Level, player, LevelHolder.GameRules);
                initEvent.Invoke(DataProvider.Runtime);
                SceneManager.sceneLoaded -= LoadedProvider;
                Pause();
            }
        }

        public void Play()
        {
            DataProvider.Runtime.PlaySafe();
        }
        public void Pause()
        {
            DataProvider.Runtime.Pause();
        }
        public void Restart()
        {
            RestartController();
            DataProvider.Runtime.Restart();
        }

        private void RestartController()
        {
            player.SetPlayerPreset(skinsData.GetSkin(0), true, 1);
            var texture = new RenderTexture(Screen.width / 2, Screen.height / 2, 24);
            previewCam.targetTexture = texture;
            preview.texture = texture;
        }
    }
}