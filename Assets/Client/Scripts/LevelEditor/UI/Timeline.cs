using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using Utils;
using TMPro;

namespace LevelEditor.UI
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] private RectTransform timeline;
        [SerializeField] private Scrollbar scrollbarHorizontal;
        [SerializeField] private RectTransform scrollView;
        [SerializeField] private RectTransform secLine;
        [SerializeField] private RectTransform secMarker;
        [SerializeField] private RectTransform secLineMarker;
        [SerializeField] private RectTransform secMarkerScrollbar;
        [SerializeField] private RectTransform content;

        [Space]
        [SerializeField] private bool verticalControl = false;

        private ITimelineController controller;

        [Space]
        [SerializeField] private float timelineLength, aspect;
        [SerializeField] private float width, secLength;
        [SerializeField] private float startCoeff, endCoeff;
        [SerializeField] private float secCurrent = 0;
        [SerializeField] private bool isPressed = false;
        [SerializeField] private bool wasPlayed = false;

        [SerializeField] private Vector2 fullScreen;
        [SerializeField] private Vector2 viewportSizeDelta;
        [SerializeField] private Vector2 contentSizeDelta;
        [SerializeField] private Vector2 viewportStart;
        [SerializeField] private Vector2 viewportEnd;

        private UnityEvent<int, int, int, int> updateTimeline = new UnityEvent<int, int, int, int>();
        public event UnityAction<int, int, int, int> UpdateTimeline
        {
            add => updateTimeline.AddListener(value);
            remove => updateTimeline.RemoveListener(value);
        }

        private void Awake()
        {
            aspect = (float)Screen.width / Screen.height;
            fullScreen = new Vector2(1080f * aspect, 1080f);
            viewportSizeDelta = new Vector2(timeline.sizeDelta.x, timeline.sizeDelta.y - scrollView.sizeDelta.y);
            viewportStart = timeline.anchoredPosition - viewportSizeDelta / 2f + fullScreen / 2f;
            viewportEnd = timeline.anchoredPosition + viewportSizeDelta / 2f + fullScreen / 2f;
            startCoeff = viewportStart.x / fullScreen.x;
            endCoeff = viewportEnd.x / fullScreen.x;
            width = viewportSizeDelta.x;
        }

        public void Init(float length, ITimelineController controller)
        {
            secLength = length;
            this.controller = controller;
            timelineLength = 100f * secLength;
            secLine.sizeDelta = new Vector2(timelineLength, Static.LayerLength);
            if (verticalControl)
                content.sizeDelta = new Vector2(timelineLength, 1000f);
            else
                content.sizeDelta = new Vector2(timelineLength, content.sizeDelta.y);
            contentSizeDelta = new Vector2(timelineLength, -1000f);

            controller.Add(UpdateSecLineMarker);
            UpdateSecLineMarker(secCurrent);
            TimelineScrollRectUpdate();
        }

        public void UpdateSecLineMarker(float sec)
        {
            secCurrent = sec;
            secMarker.localPosition = new Vector2(sec * 100f - timelineLength * scrollbarHorizontal.value, -25f);
            secMarkerScrollbar.anchoredPosition = new Vector2(sec / secLength * width - scrollbarHorizontal.value * 10f, 0f);
            float progress = secMarker.anchoredPosition.x - (timelineLength - viewportSizeDelta.x) * scrollbarHorizontal.value;
            secLineMarker.anchoredPosition = new Vector2(progress, 0f);
        }
        public void TimelineScrollRectUpdate()
        {
            secLine.pivot = new Vector2(scrollbarHorizontal.value, 1f);
            secLine.anchoredPosition = new Vector2(width * scrollbarHorizontal.value, 0f);
            secLineMarker.anchoredPosition = new Vector2(secMarker.position.x, 0f);
            UpdateSecLineMarker(secCurrent);
            if (isPressed)
                SecLineDrag();

            RenderTimeline();
        }
        public void RenderTimeline()
        {
            Vector2 contentStart = Vector2.zero;
            Vector2 contentEnd = contentSizeDelta;
            //Debug.Log(string.Join(" - ", contentStart, contentEnd, viewportStart, viewportEnd));

            Static.RenderTimelineBorders(out int startFrame, out int endFrame, out int startHeigth, out int endHeigth,
                contentStart, contentEnd, viewportStart, viewportEnd);

            updateTimeline.Invoke(startFrame, endFrame, startHeigth, endHeigth);
            //Debug.Log(string.Join(" - ", startFrame, endFrame, startHeigth, endHeigth));
        }

        public void SecLineDown()
        {
            isPressed = true;
            wasPlayed = controller.IsPlay;
            controller.Pause();
            SecLineDrag();
        }
        public void SecLineDrag()
        {
            float locWidth = timelineLength - width;
            float start = locWidth * scrollbarHorizontal.value;
            float posX_clamped = Mathf.Clamp(Input.mousePosition.x / Screen.width, startCoeff, endCoeff);
            float posX_01 = (posX_clamped - startCoeff) / (endCoeff - startCoeff);
            float posTimeline = start + posX_01 * width;

            secCurrent = posTimeline / timelineLength * secLength;
            if (secCurrent < 0) { secCurrent = 0; }
            else if (secCurrent > secLength)
            { secCurrent = secLength; }

            controller.SecCurrent = secCurrent;
        }
        public void SecLineUp()
        {
            if (wasPlayed)
                controller.Play();
            isPressed = false;
        }
        public void WatchSecondMarker()
        {
            //float timeTarget = secCurrent - secWidth / 2 + secWidth * secCurrent;
            scrollbarHorizontal.value = Mathf.Clamp01(secCurrent / secLength);
        }
    }
}