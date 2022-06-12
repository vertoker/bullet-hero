using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Game.Provider;
using UnityEngine;

namespace Game.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        private RectTransform progressBar;
        private float width;

        private void Awake()
        {
            width = 1080f * cam.aspect;
            progressBar = GetComponent<RectTransform>();
        }
        private void OnEnable()
        {
            Runtime.UpdateFrame += UpdateProgress;
        }
        private void OnDisable()
        {
            Runtime.UpdateFrame -= UpdateProgress;
        }

        private void UpdateProgress(int frame, int maxFrame)
        {
            float progress = (float)frame / maxFrame;
            progressBar.sizeDelta = new Vector2(progress * width, progressBar.sizeDelta.y);
        }
    }
}