using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Data;

namespace UI
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Slider sliderMusic;
        [SerializeField] private HorizontalSliderInt sliderGraphics;
        [SerializeField] private Toggle toggleNotifications;

        [SerializeField] private SettingsData data;
        private SettingsData startData;

        private void OnEnable()
        {
            data = SettingsProvider.DATA;
            startData = data.Copy();
            Pull();
        }
        private void OnDisable()
        {
            Push();
            SettingsProvider.DATA = data;
        }

        private void Pull()
        {
            UpdateUI();

            sliderGraphics.SliderUpdate += UpdateGraphics;
            toggleNotifications.ToggleUpdate += UpdateNotifications;
        }
        private void Push()
        {
            sliderGraphics.SliderUpdate -= UpdateGraphics;
            toggleNotifications.ToggleUpdate -= UpdateNotifications;

            UpdateData();
        }

        private void UpdateUI()
        {
            sliderGraphics.UpdateSlider((int)data.graphics);
            toggleNotifications.PressToggle(data.notifications);
            sliderMusic.value = data.music;
        }
        private void UpdateData()
        {
            data.graphics = (Data.Graphics)sliderGraphics.Value;
            data.notifications = toggleNotifications.IsOn;
            data.music = sliderMusic.value;
        }

        public void UpdateMusic()
        {
            data.music = sliderMusic.value;
        }
        private void UpdateGraphics(int value, int maxValue)
        {
            data.graphics = (Data.Graphics)value;
        }
        private void UpdateNotifications(bool value)
        {
            data.notifications = value;
        }

        public void ResetSettings()
        {
            data = startData.Copy();
            UpdateUI();
        }
    }
}