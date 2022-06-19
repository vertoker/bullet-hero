using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Data;

namespace LevelEditor.Windows
{
    public class PrefabEditor : Window
    {
        [SerializeField] private TMP_InputField nameField;

        private Level level;

        public override void Init()
        {
            level = LevelHolder.Level;
        }
        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {
            
        }

        public void Pull()
        {

        }
        public void Push()
        {

        }
    }
}