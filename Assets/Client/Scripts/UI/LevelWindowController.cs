using StandardSlider = UnityEngine.UI.Slider;
using UnityEngine.SceneManagement;
using Utils.Localization;
using UnityEngine;
using Game.Core;
using TMPro;
using Data;

namespace UI
{
    public class LevelWindowController : MonoBehaviour
    {
        [Header("Game Rules")]
        [SerializeField] private StandardSlider sliderLifes;
        [SerializeField] private StandardSlider sliderTime;
        [SerializeField] private Toggle immortalityToggle;
        [SerializeField] private Toggle botToggle;

        [Header("Level Info")]
        [SerializeField] private LocalizationString musicString;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _music;
        [SerializeField] private TMP_Text _info;

        [Space]
        [SerializeField] private GameRules gameRules;
        [SerializeField] private Level level;

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

            _level.text = "AUTHOR - LEVEL NAME";
            _music.text = "AUTHOR - TRACK NAME :MUSIC";
            _info.text = "(VERSION) TRACK LENGTH - PREFAB COUNT";
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

        public void LoadLevelInfo(Level level)
        {
            this.level = level;
            var data = level.LevelData;
            _level.text = string.Format("{0} - {1}", data.LevelAuthor, data.LevelName);
            _music.text = string.Format("{0} - {1} :{2}", data.MusicAuthor, data.MusicTitle, musicString.GetString());
            _info.text = string.Format("({2}) {0} - {1}", data.Length, level.Prefabs.Count, data.LevelDataVersion);
        }

        public void Play()
        {
            gameRules.skinID = ShopData.Loader.Selected;
            GameDataProvider.Set(gameRules, level);
            SceneManager.LoadSceneAsync(2);
        }
    }
}