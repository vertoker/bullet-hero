using UnityEngine;
using TMPro;

namespace UI
{
    public class SliderText : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }
        private void OnEnable()
        {
            slider.SliderUpdate += SliderUpdate;
        }
        private void OnDisable()
        {
            slider.SliderUpdate -= SliderUpdate;
        }
        private void SliderUpdate(int currentPage, int pagesCount)
        {
            text.text = string.Format("{0} / {1}", currentPage + 1, pagesCount);
        }
    }
}