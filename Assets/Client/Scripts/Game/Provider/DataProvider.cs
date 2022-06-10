using System.Collections;
using System.Collections.Generic;
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

        private void Awake()
        {
            runtime = GetComponent<Runtime>();
            instance = this;
        }

        public static void Load(Level level, Player[] players)
        {
            if (instance != null)
                instance.runtime.LoadLevel(level, players);
        }
        public static void StartGame()
        {

        }
        public static void StopGame()
        {

        }
    }
}