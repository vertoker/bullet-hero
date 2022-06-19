using System.Collections;
using LevelEditor.Windows;
using Game.Provider;
using UnityEngine;
using Utils;
using TMPro;

namespace LevelEditor.UI
{
    public class TimerSubscribe : MonoBehaviour
    {
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }
        private void OnEnable()
        {
            Runtime.UpdateFrame += UpdateText;
        }
        private void OnDisable()
        {
            Runtime.UpdateFrame -= UpdateText;
        }

        private void UpdateText(int activeFrame, int maxFrame)
        {
            int frames = activeFrame % Static.FRAMES_PER_SECOND_INT;
            int seconds = Floor(Static.Frame2Sec(activeFrame));
            int minutes = Floor(seconds / 60f);
            seconds -= minutes * 60;

            text.text = string.Format("{0}:{1}.{2}", Format(minutes), Format(seconds), Format(frames));
        }

        private static string Format(float value)
        {
            return value < 10 ? string.Format("0{0}", value) : value.ToString();
        }
        private static int Floor(float value)
        {
            if (value < 0)
                return 0;
            return Mathf.FloorToInt(value);
        }
    }
}