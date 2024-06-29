using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;

namespace Custom
{
    public delegate void EndAnim();
    public delegate void DestroyDelegate(UnityEngine.Object obj);

    public static class Math
    {
        public static bool OutOfBorder(Vector2 vector, Vector2 border)
        {
            float x1 = Mathf.Abs(vector.x);
            float y1 = Mathf.Abs(vector.y);
            float x2 = Mathf.Abs(border.x);
            float y2 = Mathf.Abs(border.y);
            return x1 >= x2 || y1 >= y2;
        }
    }
    public class Anim
    {
        public static float fpsTime30 = 1f / 30f;
        public static float fpsTime60 = 1f / 60f;

        /// ////////////////////////////////////////////////////////////////////////////////////
        public IEnumerator BaseAnimPosition(Transform tr, Vector3 s, Vector3 e, float timeSec = 1)
        {
            Vector3 offset = e - s;
            for (float i = 0; i < 1; i += Time.deltaTime / timeSec)
            {
                tr.localPosition = s + offset * i;
                yield return null;
            }
        }
        public IEnumerator BaseAnimPositionRotation(Transform tr, Vector3 sPos, Vector3 ePos, Vector3 sRot, Vector3 eRot, float timeSec = 1)
        {
            Vector3 offsetPos = ePos - sPos;
            Vector3 offsetRot = eRot - sRot;
            for (float i = 0; i < 1; i += Time.deltaTime / timeSec)
            {
                tr.localPosition = sPos + offsetPos * i;
                tr.localEulerAngles = sRot + offsetRot * i;
                yield return null;
            }
        }
        public IEnumerator BaseAnimRotation(Transform tr, Vector3 s, Vector3 e, float timeSec = 1)
        {
            Vector3 offset = e - s;
            for (float i = 0; i < 1; i += Time.deltaTime / timeSec)
            {
                tr.localEulerAngles = s + offset * i;
                yield return null;
            }
        }
        public IEnumerator BaseAnimScale(Transform tr, Vector3 s, Vector3 e, float timeSec = 1)
        {
            Vector3 offset = e - s;
            for (float i = 0; i < 1; i += Time.deltaTime / timeSec)
            {
                tr.localScale = s + offset * i;
                yield return null;
            }
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        public IEnumerator SmoothAnimPosition(Transform tr, Vector3 s, Vector3 e, float timeSec = 1)
        {
            Vector3 offset = e - s;
            for (float i = 0; i < 1; i += Time.deltaTime / timeSec)
            {
                float smooth = i < 0.5 ? i * i * 2 : (1 - (1 - i) * (1 - i) * 2);
                tr.localPosition = s + offset * smooth;
                yield return null;
            }
        }
        public IEnumerator SmoothAnimPositionRotation(Transform tr, Vector3 sPos, Vector3 ePos, Vector3 sRot, Vector3 eRot, float timeSec = 1)
        {
            Vector3 offsetPos = ePos - sPos;
            Vector3 offsetRot = eRot - sRot;
            for (float i = 0; i < 1; i += Time.deltaTime / timeSec)
            {
                float smooth = i < 0.5 ? i * i * 2 : (1 - (1 - i) * (1 - i) * 2);
                tr.localPosition = sPos + offsetPos * smooth;
                tr.localEulerAngles = sRot + offsetRot * smooth;
                yield return null;
            }
        }
        public IEnumerator SmoothAnimRotation(Transform tr, Vector3 s, Vector3 e, float timeSec = 1)
        {
            Vector3 offset = e - s;
            for (float i = 0; i < 1; i += Time.deltaTime / timeSec)
            {
                float smooth = i < 0.5 ? i * i * 2 : (1 - (1 - i) * (1 - i) * 2);
                tr.localEulerAngles = s + offset * smooth;
                yield return null;
            }
        }
        public IEnumerator SmoothAnimScale(Transform tr, Vector3 s, Vector3 e, float timeSec = 1)
        {
            Vector3 offset = e - s;
            for (float i = 0; i < 1; i += Time.deltaTime / timeSec)
            {
                float smooth = i < 0.5 ? i * i * 2 : (1 - (1 - i) * (1 - i) * 2);
                tr.localScale = s + offset * smooth;
                yield return null;
            }
        }
    }
}