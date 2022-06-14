using System.Collections;
using System.Collections.Generic;
using Game.Components;
using UnityEngine;
using Game.Core;
using Data;

namespace Game.Core
{
    public static class GameDataProvider
    {
        private static bool isSet = false;
        private static GameRules rules;
        private static Level level;

        public static bool IsSet => isSet;
        public static GameRules Rules => rules;
        public static Level Level => level;

        public static void Set(GameRules rules, Level level)
        {
            isSet = true;
            GameDataProvider.rules = rules.Copy();
            GameDataProvider.level = level.Copy();
        }
    }
}