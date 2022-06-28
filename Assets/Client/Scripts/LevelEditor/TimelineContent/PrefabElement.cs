using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace LevelEditor.TimelineContent
{
    public class PrefabElement : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private RectTransform rect;

        public void SetData(Vector2 position, Vector2 size)
        {
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }
        public void Enable()
        {
            image.enabled = true;
        }
        public void Disable()
        {
            image.enabled = false;
        }
    }
}