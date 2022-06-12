using System.Collections;
using System.Collections.Generic;
using Game.Components;
using UnityEngine;
using Game.Core;
using Data;

namespace Game.Core
{
    public class GameDataProvider : MonoBehaviour
    {
        private GameController controller;
        private static GameRules rules;
        private static Level level;
        private static bool isSet = false;

        private void Awake()
        {
            controller = GetComponent<GameController>();
        }
        private void Start()
        {
            if (isSet)
                controller.LoadLevel(rules, level);
        }

        public static void Set(GameRules rules, Level level)
        {
            isSet = true;
            GameDataProvider.rules = rules;
            GameDataProvider.level = level;
        }
    }
}