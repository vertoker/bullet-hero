using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using Game.Components;
using UnityEngine;
using Utils;
using Data;

namespace Game.Static
{
    public static class GetData
    {
        public static float3 GetPos(NativeArray<Pos> posMarkers, int frame, AnchorPresets anchor, ref FastNoise noise, ref float3 borderScreen)
        {
            if (posMarkers.Length == 1 || posMarkers[0].Frame >= frame)
                return CalculateData.CalculatePos(posMarkers[0], anchor, ref noise, ref borderScreen);
            else if (posMarkers[^1].Frame <= frame)
                return CalculateData.CalculatePos(posMarkers[^1], anchor, ref noise, ref borderScreen);

            Pos startPos = posMarkers[0], endPos = posMarkers[0];
            for (int i = 0; i < posMarkers.Length - 1; i++)
            {
                if (posMarkers[i].Frame <= frame && posMarkers[i + 1].Frame >= frame)
                {
                    startPos = posMarkers[i];
                    endPos = posMarkers[i + 1];
                    break;
                }
            }

            float3 start = CalculateData.CalculatePos(startPos, anchor, ref noise, ref borderScreen);
            float3 end = CalculateData.CalculatePos(endPos, anchor, ref noise, ref borderScreen);
            float progress = (float)(frame - startPos.Frame) / (endPos.Frame - startPos.Frame);
            return start + (end - start) * Easings.GetEasing(progress, endPos.Easing);
        }
        public static float3 GetRot(NativeArray<Rot> rotMarkers, int frame, ref FastNoise noise)
        {
            if (rotMarkers.Length == 1 || rotMarkers[0].Frame >= frame)
                return new float3(0, 0, CalculateData.CalculateRot(rotMarkers[0], ref noise));
            else if (rotMarkers[^1].Frame <= frame)
                return new float3(0, 0, CalculateData.CalculateRot(rotMarkers[^1], ref noise));

            Rot startRot = rotMarkers[0], endRot = rotMarkers[0];
            for (int i = 0; i < rotMarkers.Length - 1; i++)
            {
                if (rotMarkers[i].Frame <= frame && rotMarkers[i + 1].Frame >= frame)
                {
                    startRot = rotMarkers[i];
                    endRot = rotMarkers[i + 1];
                    break;
                }
            }

            float start = CalculateData.CalculateRot(startRot, ref noise);
            float end = CalculateData.CalculateRot(endRot, ref noise);
            if (Mathf.Abs(end - start) > 180f)
                end += 360f;
            float progress = (float)(frame - startRot.Frame) / (endRot.Frame - startRot.Frame);
            return new float3(0, 0, start + (end - start) * Easings.GetEasing(progress, endRot.Easing));
        }
        public static float3 GetSca(NativeArray<Sca> scaMarkers, int frame, ref FastNoise noise)
        {
            if (scaMarkers.Length == 1 || scaMarkers[0].Frame >= frame)
                return CalculateData.CalculateSca(scaMarkers[0], ref noise);
            else if (scaMarkers[^1].Frame <= frame)
                return CalculateData.CalculateSca(scaMarkers[^1], ref noise);

            Sca startSca = scaMarkers[0];
            Sca endSca = scaMarkers[0];

            for (int i = 0; i < scaMarkers.Length - 1; i++)
            {
                if (scaMarkers[i].Frame <= frame && scaMarkers[i + 1].Frame >= frame)
                {
                    startSca = scaMarkers[i];
                    endSca = scaMarkers[i + 1];
                    break;
                }
            }

            float3 start = CalculateData.CalculateSca(startSca, ref noise);
            float3 end = CalculateData.CalculateSca(endSca, ref noise);
            float progress = (float)(frame - startSca.Frame) / (endSca.Frame - startSca.Frame);
            return start + (end - start) * Easings.GetEasing(progress, endSca.Easing);
        }
        public static float4 GetClr(NativeArray<Clr> clrMarkers, int frame, ref FastNoise noise)
        {
            if (clrMarkers.Length == 1 || clrMarkers[0].Frame >= frame)
                return CalculateData.CalculateClr(clrMarkers[0], ref noise);
            else if (clrMarkers[^1].Frame <= frame)
                return CalculateData.CalculateClr(clrMarkers[^1], ref noise);

            Clr startClr = clrMarkers[0], endClr = clrMarkers[0];
            for (int i = 0; i < clrMarkers.Length - 1; i++)
            {
                if (clrMarkers[i].Frame <= frame && clrMarkers[i + 1].Frame >= frame)
                {
                    startClr = clrMarkers[i];
                    endClr = clrMarkers[i + 1];
                    break;
                }
            }

            float4 start = CalculateData.CalculateClr(startClr, ref noise);
            float4 end = CalculateData.CalculateClr(endClr, ref noise);
            float progress = (float)(frame - startClr.Frame) / (endClr.Frame - startClr.Frame);
            return start + (end - start) * Easings.GetEasing(progress, endClr.Easing);
        }
    }
}
