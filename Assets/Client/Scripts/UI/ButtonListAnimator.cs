using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Game.Static;
using Data;

namespace UI
{
    public class ButtonListAnimator : MonoBehaviour
    {
        [SerializeField] private bool openOnStart = false;
        [SerializeField] private bool isOpen = false;
        [SerializeField] private float openTime = 0.5f;
        [SerializeField] private float closeTime = 0.5f;

        [SerializeField] private Vector2 openPosition = new Vector2(-250f, 0f);
        [SerializeField] private Vector2 closePosition = new Vector2(-5000f, 0f);

        [SerializeField] private RectTransform layout;

        private Coroutine currentAnimation;
        private Coroutine offTimer;

        private void Awake()
        {
            layout.gameObject.SetActive(isOpen);
            if (isOpen)
                layout.anchoredPosition = openPosition;
            else
                layout.anchoredPosition = closePosition;
        }
        private void Start()
        {
            if (openOnStart)
            {
                Open();
            }
        }

        public void Open()
        {
            if (currentAnimation != null)
                StopCoroutine(currentAnimation);
            if (offTimer != null)
                StopCoroutine(offTimer);

            layout.gameObject.SetActive(true);
            currentAnimation = StartCoroutine(Animation(layout, layout.anchoredPosition, openPosition, EasingType.InOutQuad, openTime));
        }
        public void Close()
        {
            if (currentAnimation != null)
                StopCoroutine(currentAnimation);
            if (offTimer != null)
                StopCoroutine(offTimer);

            currentAnimation = StartCoroutine(Animation(layout, layout.anchoredPosition, closePosition, EasingType.InOutQuad, closeTime));
            offTimer = StartCoroutine(OffTimer());
        }
        private static IEnumerator Animation(RectTransform layout, Vector2 origin, Vector2 target, EasingType easingType, float time)
        {
            for (float i = 0; i <= time; i += Time.deltaTime)
            {
                yield return null;
                Vector2 current = origin + (target - origin) * Easings.GetEasing(i / time, easingType);
                layout.anchoredPosition = current;
            }
            layout.anchoredPosition = target;
        }
        private IEnumerator OffTimer()
        {
            yield return new WaitForSeconds(closeTime);
            layout.gameObject.SetActive(false);
        }
    }
}