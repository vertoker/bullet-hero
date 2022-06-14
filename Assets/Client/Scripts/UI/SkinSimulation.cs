using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class SkinSimulation : MonoBehaviour
    {
        [SerializeField] private float timeScale = 2;
        [SerializeField] private Vector2 multiplier = new Vector2(400, 150);
        [SerializeField] private Transform self;

        private Transform activePreset;
        private Coroutine updater;
        private float timer = 0f;
        private float PI2;

        private void Awake()
        {
            PI2 = Mathf.PI * 2f;
            //pointOffset = cam.ScreenToWorldPoint();
        }
        private void OnEnable()
        {
            updater = StartCoroutine(Timer());
        }
        private void OnDisable()
        {
            StopCoroutine(updater);
            timer = 0f;
        }

        public void Set(GameObject preset)
        {
            if (activePreset != null)
                Destroy(activePreset.gameObject);
            activePreset = Instantiate(preset, self).transform;
            activePreset.gameObject.SetActive(true);
        }
        private IEnumerator Timer()
        {
            while (true)
            {
                yield return null;
                timer += Time.deltaTime * timeScale;
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            float progress = Mathf.Repeat(timer, PI2);
            activePreset.localPosition = new Vector3(Mathf.Cos(progress) * multiplier.x, Mathf.Sin(progress) * multiplier.y, 0);
            activePreset.localEulerAngles = new Vector3(0, 0, progress * Mathf.Rad2Deg);
        }
    }
}