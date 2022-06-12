using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Core
{
    [Serializable]
    public class GameRules
    {
        [Tooltip("Time multiplier")]
        [Range(0.25f, 4f)]
        [SerializeField]
        public float time;

        [Tooltip("Count of lifes (if equals 0 => player invinsible)")]
        [Range(1, 10)]
        [SerializeField]
        public int lifeCount;

        [Tooltip("Player can't die (he still take damage)")]
        [SerializeField]
        public bool immortality;

        [Tooltip("Player just watch how to play bot")]
        [SerializeField]
        public bool autopilot;

        [Tooltip("Player skin")]
        [SerializeField]
        public int skinID = 1;

        public float GetScoreMultiplier()
        {
            if (immortality || autopilot)
                return 0;
            float result = 1;
            result *= time;
            result /= lifeCount;
            return result;
        }

        public static readonly GameRules standard = new GameRules()
        {
            lifeCount = 3,
            time = 1,
            immortality = false,
            autopilot = false,
            skinID = 1
        };
    }
}