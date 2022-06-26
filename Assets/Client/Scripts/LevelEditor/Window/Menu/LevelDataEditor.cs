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

        [SerializeField] private LevelData levelData;

        public void OnEnable()
        {
            levelData = LevelHolder.Level.LevelData;

            levelName.text = levelData.LevelName;
            musicTitle.text = levelData.MusicTitle;
            musicAuthor.text = levelData.MusicAuthor;
            levelAuthor.text = levelData.LevelAuthor;
            levelDataVersion.text = levelData.LevelDataVersion.ToString();
        }

        public void LevelNameUpdate()
        {
            levelData.LevelName = levelName.text;
            LevelHolder.Level.LevelData = levelData;
        }
        public void MusicTitleUpdate()
        {
            levelData.MusicTitle = musicTitle.text;
            LevelHolder.Level.LevelData = levelData;
        }
        public void MusicAuthorUpdate()
        {
            levelData.MusicAuthor = musicAuthor.text;
            LevelHolder.Level.LevelData = levelData;
        }
        public void LevelAuthorUpdate()
        {
            levelData.LevelAuthor = levelAuthor.text;
            LevelHolder.Level.LevelData = levelData;
        }
    }
}