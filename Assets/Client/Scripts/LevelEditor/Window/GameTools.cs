using System.Collections;
using System.Collections.Generic;
using Game.SerializationSaver;
using Game.Provider;
using UnityEngine;
using UI;

namespace LevelEditor.Windows
{
    public class GameTools : Window
    {
        [SerializeField] private GameObject play, pause;

        private Runtime runtime;

        private void Awake()
        {
            GamePreview.InitEvent += Init;
        }
        private void Init(Runtime runtime)
        {
            GamePreview.InitEvent -= Init;
            this.runtime = runtime;
        }
        private void OnEnable()
        {
            Runtime.UpdatePause += PlayUpdate;
        }
        private void OnDisable()
        {
            Runtime.UpdatePause -= PlayUpdate;
        }

        public void PlayPauseToggle()
        {
            runtime.TogglePlayPause();
        }
        public void Play()
        {
            runtime.PlaySafe();
        }
        public void Pause()
        {
            runtime.Pause();
        }
        public void Save()
        {
            LevelSaver.Save(LevelHolder.Level);
        }

        private void PlayUpdate(bool isPlay)
        {
            play.SetActive(!isPlay);
            pause.SetActive(isPlay);
        }
    }
}