using StandardSlider = UnityEngine.UI.Slider;
using UnityEngine;
using TMPro;

namespace UI
{
    public class StandardSliderText : MonoBehaviour
    {
        [SerializeField] private StandardSlider slider;
        [SerializeField] private FormatSingle format = FormatSingle.Float;
        private bool isInited = false;
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            isInited = true;
        }
        public void SliderUpdate()
        {
            if (!isInited)
                return;

            float value = slider.value;
            if (format == FormatSingle.Float)
            {
                text.text = value.ToString("0.00");
            }
            if (format == FormatSingle.Int)
            {
                text.text = ((int)value).ToString();
            }
        }
    }

    public enum FormatSingle : byte
    {
        Float = 0,
        Int = 1
    }
}