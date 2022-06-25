using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Slider : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerExitHandler
    {
        [SerializeField] private bool resetOnEnable = false;
        private bool isAwaken = false;

        private UnityEvent<int, int> sliderUpdate = new UnityEvent<int, int>();
        private int currentPage;
        private int pagesCount;

        private RectTransform slider;
        private RectTransform marker;
        private float heightSlider;
        private float heightMarker;
        private Vector2 sliderStart;
        private Vector2 sliderEnd;
        private bool isExit = false;

        private Vector2 screenSize;
        private Vector2 canvasSize;
        private Vector2 screenMultiplier;

        public event UnityAction<int, int> SliderUpdate
        {
            add => sliderUpdate.AddListener(value);
            remove => sliderUpdate.RemoveListener(value);
        }

        private void Awake()
        {
            if (isAwaken)
                return;
            isAwaken = true;

            slider = GetComponent<RectTransform>();
            marker = transform.GetChild(0).GetComponent<RectTransform>();
            sliderStart = new Vector2(slider.rect.xMin, slider.rect.yMin);
            sliderEnd = new Vector2(slider.rect.xMax, slider.rect.yMax);
            heightSlider = slider.rect.height;

            float aspect = (float)Screen.width / Screen.height;
            screenSize = new Vector2(Screen.width, Screen.height);
            canvasSize = new Vector2(aspect * 1080f, 1080f);
            screenMultiplier = canvasSize / screenSize;
        }
        private void OnEnable()
        {
            if (resetOnEnable && isAwaken)
            {
                FullUp();
            }
        }
        private void Start()
        {
            UpdateSlider(0);
        }
        public void Initialize(int pagesCount)
        {
            Awake();
            this.pagesCount = pagesCount;
            heightMarker = heightSlider / pagesCount;
            marker.sizeDelta = new Vector2(marker.sizeDelta.x, heightMarker);
        }

        public void Up()
        {
            UpdateSlider(currentPage - 1);
        }
        public void Down()
        {
            UpdateSlider(currentPage + 1);
        }
        public void FullUp()
        {
            UpdateSlider(0);
        }
        public void FullDown()
        {
            UpdateSlider(pagesCount);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isExit = false;
            UpdateSlider(eventData.pointerCurrentRaycast.screenPosition);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (isExit)
                return;

            UpdateSlider(eventData.pointerCurrentRaycast.screenPosition);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            isExit = true;
        }

        private void UpdateSlider(Vector2 screenPosition)
        {
            Vector2 canvasPosition = ScreenToCanvas(screenPosition);
            float distanceCurrent = sliderEnd.y - canvasPosition.y;
            float distanceMax = sliderEnd.y - sliderStart.y;
            UpdateSlider(Floor(distanceCurrent / distanceMax * pagesCount - 0.5f) + pagesCount);
        }
        private void UpdateSlider(int currentPage)
        {
            this.currentPage = Mathf.Clamp(currentPage, 0, pagesCount - 1);
            marker.anchoredPosition = new Vector2(marker.anchoredPosition.x, -this.currentPage * heightMarker);
            sliderUpdate.Invoke(this.currentPage, pagesCount);
        }
        private Vector2 ScreenToCanvas(Vector2 screenPosition)
        {
            return screenPosition * screenMultiplier;
        }

        private static int Floor(float value)
        {
            return (int)(value - value % 1f);
        }
    }
}