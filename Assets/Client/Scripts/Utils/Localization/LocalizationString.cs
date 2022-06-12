using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Localization
{
    [Serializable]
    [CreateAssetMenu(menuName = "Language/New String", fileName = "LocalizationString", order = 0)]
    public class LocalizationString : ScriptableObject
    {
        [SerializeField] private string english;
        [SerializeField] private string russian;

        public string GetString(Language language = Language.English)
        {
            switch (language)
            {
                case Language.English: return english;
                case Language.Russian: return russian;
                default: return string.Empty;
            }
        }
    }
}