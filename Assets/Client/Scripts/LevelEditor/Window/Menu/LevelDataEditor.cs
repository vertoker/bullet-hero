using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using LevelEditor;
using TMPro;
using Data;

namespace LevelEditor.Windows.Menu
{
    public class LevelDataEditor : Window
    {
        [SerializeField] private TMP_InputField levelName;
        [SerializeField] private TMP_InputField musicTitle;
        [SerializeField] private TMP_InputField musicAuthor;
        [SerializeField] private TMP_InputField levelAuthor;
        [SerializeField] private TMP_Text levelDataVersion;

        private LevelData levelData;

        public void OnEnable()
        {
            levelData = LevelHolder.Level.LevelData;

            levelName.text = levelData.LevelName;
            musicTitle.text = levelData.MusicTitle;
            musicAuthor.text = levelData.MusicAuthor;
            levelAuthor.text = levelData.LevelAuthor;
            levelDataVersion.text = levelData.EditorVersion.ToString();
        }
        public void OnDisable()
        {
            levelData.LevelName = levelName.text;
            levelData.MusicTitle = musicTitle.text;
            levelData.MusicAuthor = musicAuthor.text;
            levelData.LevelAuthor = levelAuthor.text;

            LevelHolder.Level.LevelData = levelData;
        }
    }
}