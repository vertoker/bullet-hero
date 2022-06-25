using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UI;

namespace LevelEditor.Windows
{
    public class LevelSelect : Window
    {
        [SerializeField] private ScrollViewStorage scrollView;

        private void OnEnable()
        {
            scrollView.SelectEvent += LevelHolder.Load;
        }
        private void OnDisable()
        {
            scrollView.SelectEvent -= LevelHolder.Load;
        }
    }
}