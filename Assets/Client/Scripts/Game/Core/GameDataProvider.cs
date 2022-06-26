using System.Collections;
using System.Collections.Generic;
using Game.Components;
using UnityEngine;
using Data;
using UI;

namespace Game.Core
{
    public static class GameDataProvider
    {
        private static bool isSet = false;
        private static GameRules rules;
        private static Level level;
        private static QuitType quitType;

        public static bool IsSet => isSet;
        public static GameRules Rules => rules;
        public static Level Level => level;
        public static QuitType QuitType => quitType;

        public static void Set(GameRules rules, Level level, QuitType type = QuitType.ToMenu)
        {
            isSet = true;
            GameDataProvider.rules = rules.Copy();
            GameDataProvider.level = level.Copy();
            quitType = type;
        }
        public static void Reset()
        {
            isSet = false;
        }
    }
}