using System.Collections.Generic;
using Game.Provider;
using UnityEngine;
using Data;

namespace Game.Static
{
    public static class GameUtils
    {
        public const float Rad2Deg = 57.295779513f;
        public const float PLAYER_RADIUS = 0.25f;


        private static Dictionary<AnchorPresets, Vector2> anchorCoefficient = new Dictionary<AnchorPresets, Vector2>()
        {
            { AnchorPresets.Left_Top, new Vector2(-1, 1) }, { AnchorPresets.Center_Top, new Vector2(0, 1) }, { AnchorPresets.Right_Top, new Vector2(1, 1) },
            { AnchorPresets.Left_Middle, new Vector2(-1, 0) }, { AnchorPresets.Center_Middle, new Vector2(0, 0) }, { AnchorPresets.Right_Middle, new Vector2(1, 0) },
            { AnchorPresets.Left_Bottom, new Vector2(-1, -1) }, { AnchorPresets.Center_Bottom, new Vector2(0, -1) }, { AnchorPresets.Right_Bottom, new Vector2(1, -1) }
        };
        public static Vector3 CenterAnchor(AnchorPresets anchor)
        {
            return GameData.BorderScreen * anchorCoefficient[anchor];
        }
        public static Vector3 RandomPointOnCircle(float x, float y, float angle, float radius)
        {
            angle -= 90f;
            return new Vector2(x + Mathf.Cos(angle / Rad2Deg), y + Mathf.Sin(angle / Rad2Deg)) * radius;
        }
        public static Vector3 RandomPointOnCircle(float x, float y, float angle, float radius, Vector3 offset)
        {
            angle -= 90f;
            return new Vector3(x + Mathf.Cos(angle / Rad2Deg), y + Mathf.Sin(angle / Rad2Deg)) * radius + offset;
        }
        public static Vector3 RotateVector(Vector3 a, float offsetAngle)//метод вращения объекта
        {
            float power = Mathf.Sqrt(a.x * a.x + a.y * a.y);//коэффициент силы
            float angle = Mathf.Atan2(a.y, a.x) * Rad2Deg + offsetAngle;//угол из координат с offset'ом
            return new Vector2(Mathf.Cos(angle / Rad2Deg), Mathf.Sin(angle / Rad2Deg)) * power;
            //построение вектора из изменённого угла с коэффициентом силы
        }
        public static float Interval(float value, float min, float max, float interval)
        {
            value = Mathf.Floor(value / interval) * interval;
            return Mathf.Clamp(value, min, max);
        }
        public static Vector2 CalculatePivot(float rot, Vector3 sca, AnchorPresets pivot)
        {
            sca *= anchorCoefficient[pivot] / -2f;
            float power = Mathf.Sqrt(sca.x * sca.x + sca.y * sca.y);
            float angle = Mathf.Atan2(sca.y, sca.x) * Rad2Deg + rot;
            return new Vector2(Mathf.Cos(angle / Rad2Deg), Mathf.Sin(angle / Rad2Deg)) * power;
        }
    }
}