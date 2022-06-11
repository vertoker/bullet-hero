using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Pool;

namespace UI
{
    public class InfinityLoadingIcon : MonoBehaviour
    {
        [SerializeField] private int countCircles = 10;
        [SerializeField] private float scale = 100;
        [SerializeField] private float time = 5f;

        private float space;

        private PoolSpawner pool;
        private RectTransform[] circles;
        private Coroutine coroutine;

        private void Awake()
        {
            pool = GetComponent<PoolSpawner>();
            circles = new RectTransform[countCircles];
            for (int i = 0; i < countCircles; i++)
                circles[i] = pool.Dequeue().GetComponent<RectTransform>();
            space = 1f / countCircles;
        }
        private void OnEnable()
        {
            Play();
        }
        private void OnDisable()
        {
            Stop();
        }

        public void Play()
        {
            Stop();
            coroutine = StartCoroutine(AnimationLoop());
        }
        public void Stop()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        private IEnumerator AnimationLoop()
        {
            float progress = 0;
            while (true)
            {
                UpdateDots(progress);
                yield return null;
                progress += Time.deltaTime / time;
                if (progress > 1)
                    progress--;
            }
        }
        private void UpdateDots(float progress)
        {
            for (int i = 0; i < countCircles; i++)
            {
                float currentProgress = Mathf.Repeat(progress + i * space, 1);
                circles[i].anchoredPosition = InfinityFunction(currentProgress, scale);
            }
        }

        private static Vector2 InfinityFunction(float progress, float scale)
        {
            if (progress < 0.5f)
            {
                float radians = Mathf.Repeat(progress * 4 + 1, 2) * Mathf.PI;
                return new Vector2(Mathf.Cos(radians) * scale + scale, Mathf.Sin(radians) * scale);
            }
            else
            {
                float radians = (4 - progress * 4) * Mathf.PI;
                return new Vector2(Mathf.Cos(radians) * scale - scale, Mathf.Sin(radians) * scale);
            }
        }
    }
}