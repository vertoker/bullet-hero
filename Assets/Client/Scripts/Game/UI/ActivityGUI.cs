using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.UI
{
    public class ActivityGUI : MonoBehaviour
    {
        [SerializeField] private GameObject gui;

        private void Start()
        {
            UpdateActivity();
        }
        public void UpdateActivity()
        {
            var data = SettingsProvider.DATA;
            gui.SetActive(data.game_interface);
        }
    }
}