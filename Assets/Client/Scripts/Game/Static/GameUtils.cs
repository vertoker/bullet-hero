using System.Collections.Generic;
using Unity.Mathematics;
using Game.Provider;
using UnityEngine;
using Data;

namespace Game.Static
{
    public static class GameUtils
    {
        public const float Rad2Deg = 57.295779513f;
        
        public static float3 GetCoefficient(AnchorPresets anchor, ref float3 borderScreen)
        {
            switch (anchor)
            {
                case AnchorPresets.Center_Middle: return float3.zero;
                case AnchorPresets.Left_Top: return new float3(-borderScreen.x, borderScreen.y, 0);
                case AnchorPresets.Center_Top: return new float3(0, borderScreen.y, 0);
                case AnchorPresets.Right_Top: return new float3(borderScreen.x, borderScreen.y, 0);
                case AnchorPresets.Left_Middle: return new float3(-borderScreen.x, 0, 0);
                case AnchorPresets.Right_Middle: return new float3(borderScreen.x, 0, 0);
                case AnchorPresets.Left_Bottom: return new float3(-borderScreen.x, -borderScreen.y, 0);
                case AnchorPresets.Center_Bottom: return new float3(0, -borderScreen.y, 0);
                case AnchorPresets.Right_Bottom: return new float3(borderScreen.x, -borderScreen.y, 0);
                default: return float3.zero;
            }
        }
        public static float3 RandomPointOnCircle(float x, float y, float angle, float radius)
        {
            angle -= 90f;
            return new float3(x + Mathf.Cos(angle / Rad2Deg), y + Mathf.Sin(angle / Rad2Deg), 0) * radius;
        }
        public static float3 RandomPointOnCircle(float x, float y, float angle, float radius, float3 offset)
        {
            angle -= 90f;
            return new float3(x + Mathf.Cos(angle / Rad2Deg), y + Mathf.Sin(angle / Rad2Deg), 0) * radius + offset;
        }
        public static float3 RotateVector(float3 a, float offsetAngle)//метод вращения объекта
        {
            float power = Mathf.Sqrt(a.x * a.x + a.y * a.y);//коэффициент силы
            float angle = Mathf.Atan2(a.y, a.x) * Rad2Deg + offsetAngle;//угол из координат с offset'ом
            return new float3(Mathf.Cos(angle / Rad2Deg), Mathf.Sin(angle / Rad2Deg), 0) * power;
            //построение вектора из изменённого угла с коэффициентом силы
        }
        public static float Interval(float value, float min, float max, float interval)
        {
            value = Mathf.Floor(value / interval) * interval;
            return Mathf.Clamp(value, min, max);
        }
        public static float2 CalculatePivot(float rot, float3 sca, AnchorPresets pivot)
        {
            sca = GetCoefficient(pivot, ref sca) / -2f;
            float power = Mathf.Sqrt(sca.x * sca.x + sca.y * sca.y);
            float angle = Mathf.Atan2(sca.y, sca.x) * Rad2Deg + rot;
            return new float2(Mathf.Cos(angle / Rad2Deg), Mathf.Sin(angle / Rad2Deg)) * power;
        }
    }
}