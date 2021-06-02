using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Localize : MonoBehaviour
{
    [SerializeField] private TextDataTranslation[] data;
    [SerializeField] private DataRandomTextTranslation[] randomTextData;
    public static Localize Instance;

    private void Awake()
    { Instance = this; }

    public void TranslateRandomText(int index)
    {
        string language = PlayerPrefs.GetString("language");
        randomTextData[index].Translate(language);
    }

    public void TranslateInit()
    {
        string language = PlayerPrefs.GetString("language");
        for (int i = 0; i < data.Length; i++)
            data[i].Translate(language);
    }
}

[SerializeField]
public struct TextDataTranslation
{
    [SerializeField] private LocalizationData data;
    [SerializeField] private TMP_Text[] texts;

    public void Translate(string language)
    {
        string result = data.Get(language);
        for (int i = 0; i < texts.Length; i++)
            texts[i].text = result;
    }
}
[SerializeField]
public struct DataRandomTextTranslation
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private LocalizationData[] data;

    public void Translate(string language)
    {
        int randomID = Random.Range(0, data.Length);
        text.text = data[randomID].Get(language);
    }
}