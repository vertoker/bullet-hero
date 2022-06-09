using System.Collections.Generic;
using Game.Components;
using UnityEngine;
using System;
using Data;

namespace Game.Static
{
    public static class GetChildData
    {
        public static Vector3 GetPosChild(List<Pos> posMarkers, int frame, Vector3 local, Vector3 posParent, Vector3 global, AnchorPresets anchor)
        {
            if (posMarkers.Count == 1 || posMarkers[0].Frame >= frame)
                return CalculateChildData.CalculatePosChild(posMarkers[0], local, posParent, global, anchor);
            else if (posMarkers[posMarkers.Count - 1].Frame <= frame)
                return CalculateChildData.CalculatePosChild(posMarkers[posMarkers.Count - 1], local, posParent, global, anchor);

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

            Vector3 start = CalculateChildData.CalculatePosChild(startPos, local, posParent, global, anchor);
            Vector3 end = CalculateChildData.CalculatePosChild(endPos, local, posParent, global, anchor);
            float progress = (float)(frame - startPos.Frame) / (endPos.Frame - startPos.Frame);
            return start + (end - start) * Easings.GetEasing(progress, endPos.Easing);
        }
        public static Vector3 GetRotChild(List<Rot> rotMarkers, int frame, Vector3 rotParent)
        {
            if (rotMarkers.Count == 1 || rotMarkers[0].Frame >= frame)
                return new Vector3(0, 0, CalculateChildData.CalculateRotChild(rotMarkers[0], rotParent.z));
            else if (rotMarkers[rotMarkers.Count - 1].Frame <= frame)
                return new Vector3(0, 0, CalculateChildData.CalculateRotChild(rotMarkers[rotMarkers.Count - 1], rotParent.z));

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

            float start = CalculateChildData.CalculateRotChild(startRot, rotParent.z);
            float end = CalculateChildData.CalculateRotChild(endRot, rotParent.z);
            if (Mathf.Abs(end - start) > 180f)
                end += 360f;
            float progress = (float)(frame - startRot.Frame) / (endRot.Frame - startRot.Frame);
            return new Vector3(0, 0, start + (end - start) * Easings.GetEasing(progress, endRot.Easing));
        }
        public static Vector3 GetScaChild(List<Sca> scaMarkers, int frame, Vector3 scaParent)
        {
            if (scaMarkers.Count == 1 || scaMarkers[0].Frame >= frame)
                return CalculateChildData.CalculateScaChild(scaMarkers[0], scaParent);
            else if (scaMarkers[scaMarkers.Count - 1].Frame <= frame)
                return CalculateChildData.CalculateScaChild(scaMarkers[scaMarkers.Count - 1], scaParent);

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

            Vector3 start = CalculateChildData.CalculateScaChild(startSca, scaParent);
            Vector3 end = CalculateChildData.CalculateScaChild(endSca, scaParent);
            float progress = (float)(frame - startSca.Frame) / (endSca.Frame - startSca.Frame);
            return start + (end - start) * Easings.GetEasing(progress, endSca.Easing);
        }
    }
}
