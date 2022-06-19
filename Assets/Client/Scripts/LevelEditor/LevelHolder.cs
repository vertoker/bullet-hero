using System.Collections;
using System.Collections.Generic;
using Game.SerializationSaver;
using LevelEditor.Windows;
using UnityEngine;
using Game.Core;
using Data;

namespace LevelEditor
{
    public class LevelHolder : MonoBehaviour
    {
        [SerializeField] private WindowController controller;
        [SerializeField] private GameRules gameRules;
        [SerializeField] private Level level;

        private static LevelHolder instance;

        public static GameRules GameRules
        {
            get { return GameRules.standard; }
            set { instance.gameRules = value; }
        }
        public static Level Level
        {
            get { return Level.DEFAULT_LEVEL; }
            set { instance.level = value; }
        }

        /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
        }*/

        private void Awake()
        {
            instance = this;
            controller.Init();
        }
        public static void Save()
        {
            LevelSaver.Save(Level);
        }
    }
}