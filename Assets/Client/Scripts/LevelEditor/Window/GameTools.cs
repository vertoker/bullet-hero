using System.Collections;
using System.Collections.Generic;
using Game.SerializationSaver;
using Game.Provider;
using UnityEngine;
using TMPro;
using UI;

namespace LevelEditor.Windows
{
    public class GameTools : Window
    {
        [SerializeField] private float scaleTimeStep = 0.1f;
        [SerializeField] private TMP_Text updateTimeScale;
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

        public void ScaleDefault()
        {
            LevelHolder.GameRules.time = 1f;
            UpdateScale();
        }
        public void ScaleUp()
        {
            LevelHolder.GameRules.time = ClampMax(LevelHolder.GameRules.time + scaleTimeStep, 3f);
            UpdateScale();
        }
        public void ScaleDown()
        {
            LevelHolder.GameRules.time = ClampMin(LevelHolder.GameRules.time - scaleTimeStep, -3f);
            UpdateScale();
        }
        private void UpdateScale()
        {
            runtime.SetTimeScale(LevelHolder.GameRules.time);
            updateTimeScale.text = LevelHolder.GameRules.time.ToString("0.0");
        }

        private static float ClampMax(float value, float max)
        {
            if (value > max)
                value = max;
            return value;
        }
        private static float ClampMin(float value, float min)
        {
            if (value < min)
                value = min;
            return value;
        }
    }
}