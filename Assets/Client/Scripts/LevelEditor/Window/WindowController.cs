using System.Collections;
using System.Linq;
using UnityEngine;
using Game.Core;
using Data;

namespace LevelEditor.Windows
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private WindowData[] data;
        [SerializeField] private GameRules gameRules;
        [SerializeField] private Level level;

        public GameRules GameRules
        {
            get { return gameRules; }
            set { gameRules = value; }
        }
        public Level Level
        {
            get { return level; }
            set { level = value; }
        }

        private void Awake()
        {
            foreach (var window in data)
            {
                window.data.Init(this);
                if (window.startActive)
                    window.Enable(this);
                else
                    window.Disable();
            }
        }

        public void Enable(string name)
        {
            data.FirstOrDefault((WindowData windowData) => { return windowData.obj.name == name; }).Enable(this);
        }
        public void Disable(string name)
        {
            data.FirstOrDefault((WindowData windowData) => { return windowData.obj.name == name; }).Disable();
        }

        private void OnDisable()
        {
            foreach (var window in data)
                window.Disable();
        }
    }

    [System.Serializable]
    public struct WindowData
    {
        public GameObject obj;
        public Window data;
        public bool startActive;

        public void Enable(WindowController controller)
        {
            obj.SetActive(true);
        }
        public void Disable()
        {
            obj.SetActive(false);
        }
    }
}