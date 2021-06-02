using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Localization Text", menuName = "New Text", order = 1)]
public class LocalizationData : ScriptableObject
{
    [SerializeField] [TextArea(4, 20)] private string eng;
    [SerializeField] [TextArea(4, 20)] private string rus;

    public string Get(string language)
    {
        switch (language)
        {
            case "eng": return eng;
            case "rus": return rus;
        }
        return string.Empty;
    }
}
