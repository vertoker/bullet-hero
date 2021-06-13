using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LevelDataWindow : MonoBehaviour, IWindow
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private TMP_InputField levelNameField;
    [SerializeField] private TMP_InputField musicTitleField;
    [SerializeField] private TMP_InputField musicAuthorField;
    [SerializeField] private TMP_InputField levelAuthorField;
    [SerializeField] private TMP_Text version;

    public void Init()
    {
        levelNameField.onValueChanged.AddListener(LevelManager.LevelName);
        musicTitleField.onValueChanged.AddListener(LevelManager.MusicTitle);
        musicAuthorField.onValueChanged.AddListener(LevelManager.MusicAuthor);
        levelAuthorField.onValueChanged.AddListener(LevelManager.LevelAuthor);
        version.text += ": " + LevelManager.level.LevelData.EditorVersion.ToString();
    }
    public RectTransform Open()
    {
        parent.gameObject.SetActive(true);
        return parent;
    }
    public void Close()
    {
        parent.gameObject.SetActive(false);
    }
}
