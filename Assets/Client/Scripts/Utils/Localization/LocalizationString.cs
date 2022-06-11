using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Localization
{
    public class LocalizationString : ScriptableObject
    {
        [SerializeField] private string english;
        [SerializeField] private string russian;

        public string GetString(Language language)
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