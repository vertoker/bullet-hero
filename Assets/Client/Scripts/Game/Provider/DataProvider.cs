using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Game.Components;
using UnityEngine;
using Game.Core;
using Data;

using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;

namespace Game.Provider
{
    public class DataProvider : MonoBehaviour
    {
        private Runtime runtime;
        private static DataProvider instance;

        public static Runtime Runtime => instance.runtime;

        private void Awake()
        {
            runtime = GetComponent<Runtime>();
            instance = this;
        }

        public static void Start(Level level, Player player, GameRules rules)
        {
            if (instance != null)
                instance.runtime.LoadLevel(level, player, rules);
        }
    }
}