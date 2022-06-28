using System.Collections;
using System.Collections.Generic;
using Game.SerializationSaver;
using LevelEditor.Windows;
using UnityEngine.Events;
using UnityEngine;
using Game.Core;
using Data;

namespace LevelEditor
{
    public class LevelHolder : MonoBehaviour
    {
        [SerializeField] private float timeoutLoad = 1.5f;
        [SerializeField] private GameTools tools;
        [SerializeField] private GameObject levelSelect;
        [SerializeField] private WindowController controller;
        [SerializeField] private GameRules gameRules;
        [SerializeField] private Level level;

        private static LevelHolder instance;

        private static UnityEvent<Level> updateLevelData = new UnityEvent<Level>();
        public static event UnityAction<Level> UpdateLevelData
        {
            add => updateLevelData.AddListener(value);
            remove => updateLevelData.RemoveListener(value);
        }

        public static GameRules GameRules
        {
            get { return instance.gameRules; }
            set { instance.gameRules = value; }
        }
        public static Level Level
        {
            get { return instance.level; }
            set { instance.level = value; updateLevelData.Invoke(value); }
        }

        private void Awake()
        {
            instance = this;
            levelSelect.SetActive(!GameDataProvider.IsSet);
            if (GameDataProvider.IsSet)
                Load(GameDataProvider.Level);
        }
        public static void Load(Level level)
        {
            Level = level;
            GameRules = GameRules.standard;
            GameDataProvider.Reset();
            instance.controller.Init();
            instance.StartCoroutine(instance.LoadFinish());
        }
        public static void Save()
        {
            LevelSaver.Save(Level);
        }

        private IEnumerator LoadFinish()
        {
            yield return new WaitForSecondsRealtime(timeoutLoad);
            levelSelect.SetActive(false);
            tools.Play();
        }
    }
}