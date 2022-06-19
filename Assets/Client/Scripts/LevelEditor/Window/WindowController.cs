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
        [SerializeField] private bool initOnAwake = false;
        private WindowData lastActive;
        private bool hasLast = false;

        private void Awake()
        {
            if (initOnAwake)
                Init();
        }

        public void Init()
        {
            foreach (var window in data)
            {
                if (window.data != null)
                    window.data.Init();
                if (window.startActive)
                    window.Enable();
                else
                    window.Disable();
            }
        }

        public void Enable(string name)
        {
            lastActive = data.FirstOrDefault((WindowData windowData) => { return windowData.obj.name == name; });
            lastActive.Enable();
            hasLast = true;
        }
        public void Disable(string name)
        {
            lastActive = data.FirstOrDefault((WindowData windowData) => { return windowData.obj.name == name; });
            lastActive.Disable();
            hasLast = true;
        }
        public void EnableLast()
        {
            if (hasLast)
            {
                lastActive.Enable();
                hasLast = false;
            }
        }
        public void DisableLast()
        {
            if (hasLast)
            {
                lastActive.Disable();
                hasLast = false;
            }
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

        public void Enable()
        {
            obj.SetActive(true);
        }
        public void Disable()
        {
            obj.SetActive(false);
        }
    }
}