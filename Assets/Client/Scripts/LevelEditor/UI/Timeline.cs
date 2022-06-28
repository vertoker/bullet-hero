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
        [SerializeField] private RectTransform parent;
        [SerializeField] private Vector2 offset = new Vector2();
        [SerializeField] private RectTransform timeline;
        [SerializeField] private Scrollbar scrollbarHorizontal;
        [SerializeField] private RectTransform scrollView;
        [SerializeField] private RectTransform secLine;
        [SerializeField] private RectTransform secMarker;
        [SerializeField] private RectTransform secLineMarker;
        [SerializeField] private RectTransform secMarkerScrollbar;
        [SerializeField] private RectTransform content;

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
        [SerializeField] private Vector2 secLineOffset;

        private UnityEvent<int, int, int, int> updateTimeline = new UnityEvent<int, int, int, int>();
        public event UnityAction<int, int, int, int> UpdateTimeline
        {
            add => updateTimeline.AddListener(value);
            remove => updateTimeline.RemoveListener(value);
        }

        private void Awake()
        {
            secLineOffset = new Vector2(0f, 50f);
            aspect = (float)Screen.width / Screen.height;
            fullScreen = new Vector2(1080f * aspect, 1080f);
            viewportSizeDelta = new Vector2(timeline.rect.width, timeline.rect.height - scrollView.sizeDelta.y);
            viewportStart = parent.anchoredPosition + offset - viewportSizeDelta / 2f + fullScreen / 2f + secLineOffset;
            viewportEnd = parent.anchoredPosition + offset + viewportSizeDelta / 2f + fullScreen / 2f - secLineOffset;
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
            content.sizeDelta = contentSizeDelta = new Vector2(timelineLength, content.sizeDelta.y);

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
            float posStart = (timelineLength - width) * scrollbarHorizontal.value;
            float posEnd = width + timelineLength * scrollbarHorizontal.value;
            float posFull = timelineLength * scrollbarHorizontal.value;

            Vector2 contentStart = new Vector2(-posFull, 0f);
            Vector2 contentEnd = contentStart + contentSizeDelta;
            //Debug.Log(string.Join(' ', posStart, posEnd, posFull));

            int startFrame, endFrame, startHeigth, endHeigth;

            if (viewportStart.x < contentStart.x)
                startFrame = Static.Sec2Frame(0);
            else
                startFrame = Static.Sec2Frame(posStart / Static.SecondLength);

            if (viewportEnd.x > contentEnd.x)
                endFrame = Static.Sec2Frame(secLength);
            else
                endFrame = Static.Sec2Frame(posEnd / Static.SecondLength);

            if (viewportStart.y > contentStart.y)
                startHeigth = 0;
            else
                startHeigth = Mathf.FloorToInt(-viewportStart.y / Static.LayerLength);

            if (viewportEnd.y < contentEnd.y)
                endHeigth = -(int)(contentEnd.y / Static.LayerLength);
            else
                endHeigth = Mathf.FloorToInt(-viewportEnd.y / Static.LayerLength) + 1;

            updateTimeline.Invoke(startFrame, endFrame, startHeigth, endHeigth);
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