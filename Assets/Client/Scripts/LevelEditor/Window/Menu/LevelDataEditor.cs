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
            Debug.Log("OnEnable");
            levelData = LevelHolder.Level.LevelData;

            levelName.text = levelData.LevelName;
            musicTitle.text = levelData.MusicTitle;
            musicAuthor.text = levelData.MusicAuthor;
            levelAuthor.text = levelData.LevelAuthor;
            levelDataVersion.text = levelData.LevelDataVersion.ToString();
        }
        public void OnDisable()
        {
            Debug.Log("OnDisable");
            //levelData.LevelName = levelName.text;
            //levelData.MusicTitle = musicTitle.text;
            //levelData.MusicAuthor = musicAuthor.text;
            //levelData.LevelAuthor = levelAuthor.text;

            LevelHolder.Level.LevelData = levelData;
        }

        private void LevelNameUpdate()
        {

        }
        private void MusicTitleUpdate()
        {

        }
        private void MusicAuthorUpdate()
        {

        }
        private void LevelAuthorUpdate()
        {

        }
    }
}