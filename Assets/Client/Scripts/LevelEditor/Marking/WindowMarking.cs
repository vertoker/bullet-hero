using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace LevelEditor.Marking
{
    public class WindowMarking : MonoBehaviour
    {
        [SerializeField] private MarkingData[] data;
        [SerializeField] private Camera cam;

        private float aspect;
        private Dictionary<WindowLocation, Vector2> sizes;
        private Dictionary<WindowLocation, Vector2> positions;

        private void Awake()
        {
            aspect = cam.aspect;
            sizes = new Dictionary<WindowLocation, Vector2>();
            positions = new Dictionary<WindowLocation, Vector2>();

            sizes.Add(WindowLocation.Full, new Vector2(1080f * aspect, 1080f));
            positions.Add(WindowLocation.Full, Vector2.zero);
            sizes.Add(WindowLocation.Left, new Vector2(540f * aspect, 1080f));
            positions.Add(WindowLocation.Left, new Vector2(-270f * aspect, 0f));
            sizes.Add(WindowLocation.Right, new Vector2(540f * aspect, 1080f));
            positions.Add(WindowLocation.Right, new Vector2(270f * aspect, 0f));

            sizes.Add(WindowLocation.GamePreview, new Vector2(540f * aspect, 540f));
            positions.Add(WindowLocation.GamePreview, new Vector2(-270f * aspect, 270f));
            sizes.Add(WindowLocation.Tools, new Vector2(540f * aspect, 100f));
            positions.Add(WindowLocation.Tools, new Vector2(-270f * aspect, -50f));
            sizes.Add(WindowLocation.Timeline, new Vector2(540f * aspect, 440f));
            positions.Add(WindowLocation.Timeline, new Vector2(-270f * aspect, -320));

            sizes.Add(WindowLocation.CustomizationDown, new Vector2(540f * aspect, 540f));
            positions.Add(WindowLocation.CustomizationDown, new Vector2(270f * aspect, -270f));
            sizes.Add(WindowLocation.CustomizationUp, new Vector2(540f * aspect, 540f));
            positions.Add(WindowLocation.CustomizationUp, new Vector2(270f * aspect, 270f));
            sizes.Add(WindowLocation.CustomizationTimeline, new Vector2(540f * aspect, 340f));
            positions.Add(WindowLocation.CustomizationTimeline, new Vector2(270f * aspect, -370f));
            sizes.Add(WindowLocation.CustomizationPrefab, new Vector2(540f * aspect, 740f));
            positions.Add(WindowLocation.CustomizationPrefab, new Vector2(270f * aspect, 170f));

            foreach (var markingData in data)
            {
                markingData.transform.sizeDelta = sizes[markingData.location];
                markingData.transform.anchoredPosition = positions[markingData.location];
            }
        }
    }

    [System.Serializable]
    public struct MarkingData
    {
        public RectTransform transform;
        public WindowLocation location;
    }

    public enum WindowLocation
    {
        Full = 0,
        Left = 1,
        Right = 2,
        GamePreview = 3,
        Tools = 4,
        Timeline = 5,
        CustomizationUp = 6,
        CustomizationDown = 7,
        CustomizationPrefab = 8,
        CustomizationTimeline = 9
    }
}