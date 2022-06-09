using System.Collections.Generic;
using Game.Components;
using UnityEngine;
using System;
using Data;

namespace Game.Static
{
    public static class GetData
    {
        public static Vector3 GetPos(List<Pos> posMarkers, int frame, Vector3 local, AnchorPresets anchor)
        {
            if (posMarkers.Count == 1 || posMarkers[0].Frame >= frame)
                return CalculateData.CalculatePos(posMarkers[0], local, anchor);
            else if (posMarkers[posMarkers.Count - 1].Frame <= frame)
                return CalculateData.CalculatePos(posMarkers[posMarkers.Count - 1], local, anchor);

            Pos startPos = posMarkers[0], endPos = posMarkers[0];
            for (int i = 0; i < posMarkers.Count - 1; i++)
            {
                if (posMarkers[i].Frame <= frame && posMarkers[i + 1].Frame >= frame)
                {
                    startPos = posMarkers[i];
                    endPos = posMarkers[i + 1];
                    break;
                }
            }

            Vector3 start = CalculateData.CalculatePos(startPos, local, anchor);
            Vector3 end = CalculateData.CalculatePos(endPos, local, anchor);
            float progress = (float)(frame - startPos.Frame) / (endPos.Frame - startPos.Frame);
            return start + (end - start) * Easings.GetEasing(progress, endPos.Easing);
        }
        public static Vector3 GetRot(List<Rot> rotMarkers, int frame)
        {
            if (rotMarkers.Count == 1 || rotMarkers[0].Frame >= frame)
                return new Vector3(0, 0, CalculateData.CalculateRot(rotMarkers[0]));
            else if (rotMarkers[rotMarkers.Count - 1].Frame <= frame)
                return new Vector3(0, 0, CalculateData.CalculateRot(rotMarkers[rotMarkers.Count - 1]));

            Rot startRot = rotMarkers[0], endRot = rotMarkers[0];
            for (int i = 0; i < rotMarkers.Count - 1; i++)
            {
                if (rotMarkers[i].Frame <= frame && rotMarkers[i + 1].Frame >= frame)
                {
                    startRot = rotMarkers[i];
                    endRot = rotMarkers[i + 1];
                    break;
                }
            }

            float start = CalculateData.CalculateRot(startRot);
            float end = CalculateData.CalculateRot(endRot);
            if (Mathf.Abs(end - start) > 180f)
                end += 360f;
            float progress = (float)(frame - startRot.Frame) / (endRot.Frame - startRot.Frame);
            return new Vector3(0, 0, start + (end - start) * Easings.GetEasing(progress, endRot.Easing));
        }
        public static Vector3 GetSca(List<Sca> scaMarkers, int frame)
        {
            if (scaMarkers.Count == 1 || scaMarkers[0].Frame >= frame)
                return CalculateData.CalculateSca(scaMarkers[0]);
            else if (scaMarkers[scaMarkers.Count - 1].Frame <= frame)
                return CalculateData.CalculateSca(scaMarkers[scaMarkers.Count - 1]);

            Sca startSca = scaMarkers[0];
            Sca endSca = scaMarkers[0];

            for (int i = 0; i < scaMarkers.Count - 1; i++)
            {
                if (scaMarkers[i].Frame <= frame && scaMarkers[i + 1].Frame >= frame)
                {
                    startSca = scaMarkers[i];
                    endSca = scaMarkers[i + 1];
                    break;
                }
            }

            Vector3 start = CalculateData.CalculateSca(startSca);
            Vector3 end = CalculateData.CalculateSca(endSca);
            float progress = (float)(frame - startSca.Frame) / (endSca.Frame - startSca.Frame);
            return start + (end - start) * Easings.GetEasing(progress, endSca.Easing);
        }
        public static Vector4 GetClr(List<Clr> clrMarkers, int frame)
        {
            if (clrMarkers.Count == 1 || clrMarkers[0].Frame >= frame)
                return CalculateData.CalculateClr(clrMarkers[0]);
            else if (clrMarkers[clrMarkers.Count - 1].Frame <= frame)
                return CalculateData.CalculateClr(clrMarkers[clrMarkers.Count - 1]);

            Clr startClr = clrMarkers[0], endClr = clrMarkers[0];
            for (int i = 0; i < clrMarkers.Count - 1; i++)
            {
                if (clrMarkers[i].Frame <= frame && clrMarkers[i + 1].Frame >= frame)
                {
                    startClr = clrMarkers[i];
                    endClr = clrMarkers[i + 1];
                    break;
                }
            }

            Vector4 start = CalculateData.CalculateClr(startClr);
            Vector4 end = CalculateData.CalculateClr(endClr);
            float progress = (float)(frame - startClr.Frame) / (endClr.Frame - startClr.Frame);
            return start + (end - start) * Easings.GetEasing(progress, endClr.Easing);
        }
    }
}
