using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelEditor;
using TMPro;

namespace LevelEditor.Windows.Menu
{
    public class LevelDebug : Window
    {
        [SerializeField] private TMP_Text debug;

        private void OnEnable()
        {
            debug.text = JsonUtility.ToJson(LevelHolder.Level, true);
        }
    }
}