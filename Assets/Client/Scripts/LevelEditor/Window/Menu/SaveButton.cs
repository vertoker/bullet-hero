using System.Collections;
using System.Collections.Generic;
using LevelEditor.Windows;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace LevelEditor.Windows.Menu
{
    public class SaveButton : MonoBehaviour
    {
        [SerializeField] private float animationTime = 0.5f;
        [SerializeField] private Color col = new Color(0f, 1f, 0.2f, 1f);
        [SerializeField] private GameObject iconSaving;
        [SerializeField] private Image progressBar;

        private Coroutine save, anim;

        private void OnEnable()
        {
            iconSaving.SetActive(false);
            AnimationReset();
        }
        private void OnDisable()
        {
            Deactivate();
        }
        public void Save()
        {
            Deactivate();
            AnimationReset();
            iconSaving.SetActive(true);
            save = StartCoroutine(SaveOperation());
        }

        private IEnumerator SaveOperation()
        {
            LevelHolder.Save();
            yield return new WaitForSeconds(0.3f);
            iconSaving.SetActive(false);
            anim = StartCoroutine(CompleteAnimation());
        }

        private void AnimationReset()
        {
            progressBar.fillAmount = 0;
            progressBar.color = col;
        }
        private IEnumerator CompleteAnimation()
        {
            for (float i = 0; i <= 1f; i += Time.deltaTime / animationTime)
            {
                yield return null;
                progressBar.fillAmount = i;
                progressBar.color = new Color(col.r, col.g, col.b, 1f - i);
            }
            AnimationReset();
        }

        private void Deactivate()
        {
            if (save != null)
                StopCoroutine(save);
            if (anim != null)
                StopCoroutine(anim);
        }
    }
}