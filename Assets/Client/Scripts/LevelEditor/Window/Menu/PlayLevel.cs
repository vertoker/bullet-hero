using StandardSlider = UnityEngine.UI.Slider;
using UnityEngine.SceneManagement;
using UnityEngine;
using Game.Core;
using Data;
using UI;

namespace LevelEditor.Windows.Menu
{
    public class PlayLevel : Window
    {
        [Header("Game Rules")]
        [SerializeField] private StandardSlider sliderLifes;
        [SerializeField] private StandardSlider sliderTime;
        [SerializeField] private Toggle immortalityToggle;
        [SerializeField] private Toggle botToggle;

        [SerializeField] private GameRules gameRules;

        private void OnEnable()
        {
            immortalityToggle.ToggleUpdate += UpdateImmortality;
            botToggle.ToggleUpdate += UpdateBot;
        }
        private void OnDisable()
        {
            immortalityToggle.ToggleUpdate -= UpdateImmortality;
            botToggle.ToggleUpdate -= UpdateBot;

            immortalityToggle.PressToggle(false);
            botToggle.PressToggle(false);
            sliderLifes.value = 3;
            sliderTime.value = 1;
        }

        public void SliderLifesUpdate()
        {
            int value = (int)sliderLifes.value;
            gameRules.lifeCount = value;
        }
        public void SliderTimeUpdate()
        {
            float value = sliderTime.value;
            gameRules.time = value;
        }
        public void UpdateImmortality(bool value)
        {
            gameRules.immortality = value;
        }
        public void UpdateBot(bool value)
        {
            gameRules.autopilot = value;
        }

        public void Play()
        {
            gameRules.skinID = ShopData.Loader.Selected;
            GameDataProvider.Set(gameRules, LevelHolder.Level, QuitType.ToEditor);
            SceneManager.LoadSceneAsync(2);
        }
    }
}