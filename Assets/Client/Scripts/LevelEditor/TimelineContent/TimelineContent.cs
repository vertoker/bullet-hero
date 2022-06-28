using System.Collections;
using System.Collections.Generic;
using LevelEditor.UI;
using UnityEngine;

namespace LevelEditor.TimelineContent
{
    public class TimelineContent : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;
        [SerializeField] private ContentHandler[] handlers;

        private void OnEnable()
        {
            foreach (var handler in handlers)
                timeline.UpdateTimeline += handler.UpdateUI;
        }
        private void OnDisable()
        {
            foreach (var handler in handlers)
                timeline.UpdateTimeline -= handler.UpdateUI;
        }
    }
}