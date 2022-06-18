using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Static
    {
        public const float FRAMES_PER_SECOND = 60;
        public const int FRAMES_PER_SECOND_INT = 60;
        private const float OFFSET_FRAMES_CONVERT = 1 / FRAMES_PER_SECOND / 2;
        public static int String2Int(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return 0;
        }
        public static float String2Float(string value)
        {
            if (float.TryParse(value, out float result))
                return result;
            return 0;
        }
        public static int Sec2Frame(float sec)
        {
            return Mathf.FloorToInt((sec + OFFSET_FRAMES_CONVERT) * FRAMES_PER_SECOND);
        }
        public static float Frame2Sec(int frame)
        {
            return frame / FRAMES_PER_SECOND - OFFSET_FRAMES_CONVERT;
        }
        public static string Sec2Text(string time)
        {
            return Sec2Text(String2Float(time));
        }
        public static string Sec2Text(float sec)
        {
            int min = 0; string timerText = string.Empty;
            int frames = (int)((sec - (int)sec) * 60f);
            while (sec >= 60f) { min++; sec -= 60; }

            sec = (int)sec; if (frames < 0)
            { frames = 60 + frames; sec -= 1; }

            if (min < 10) { timerText += "0" + min + ":"; }
            else { timerText += min + ":"; }

            if (sec < 10f) { timerText += "0" + sec.ToString("0"); }
            else { timerText += sec.ToString("0"); }

            if (frames < 10) { timerText += ".0" + frames; }
            else { timerText += "." + frames; }

            return timerText;
            /*timerTimeLine.text = timerText;
            timerMarkers.text = timerText;
            timerSecMarkers.text = sec + "." + frames;*/
        }
        public static float ConstantClamp(float value, float min, float max)
        {
            if (Mathf.Abs(value - min) > Mathf.Abs(max - value))
                return max;
            return min;
        }
        public const float SecondLength = 100f;
        public const float LayerLength = 50f;
        public static void RenderTimelineBorders(out int startFrame, out int endFrame, out int startHeigth, out int endHeigth,
            Vector2 contentStart, Vector2 contentEnd, Vector2 viewportStart, Vector2 viewportEnd)//Доделать
        {
            if (viewportStart.x < contentStart.x)
                startFrame = Sec2Frame(0);
            else
                startFrame = Sec2Frame(viewportStart.x / SecondLength);

            if (viewportEnd.x > contentEnd.x)
                endFrame = Sec2Frame(contentEnd.x / SecondLength);
            else
                endFrame = Sec2Frame(viewportEnd.x / SecondLength);

            if (viewportStart.y > contentStart.y)
                startHeigth = 0;
            else
                startHeigth = Mathf.FloorToInt(-viewportStart.y / LayerLength);

            if (viewportEnd.y < contentEnd.y)
                endHeigth = -(int)(contentEnd.y / LayerLength);
            else
                endHeigth = Mathf.FloorToInt(-viewportEnd.y / LayerLength) + 1;

            /*float lengthViewport = viewportStart.y - viewportEnd.y;
            endHeigth = startHeigth + Mathf.FloorToInt(lengthViewport) + 1;
            if (endHeigth > heigthCount)
                endHeigth = heigthCount;*/
        }
    }
}