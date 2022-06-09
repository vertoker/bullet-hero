using System.Collections.Generic;
using Game.Components;
using Game.Provider;
using UnityEngine;
using System;
using Data;

namespace Game.Static
{
    public static class CalculateChildData
    {
        public static Vector3 CalculatePosChild(Pos pos, Vector3 local, Vector3 posParent, Vector3 global, AnchorPresets anchor)
        {
            Vector3 center = GameUtils.CenterAnchor(anchor);
            Vector3 fullOffset = center + local + global + posParent;
            switch (pos.RandomType)
            {
                case VectorRandomType.N:
                    return fullOffset + new Vector3(pos.SX, pos.SY, 0);
                case VectorRandomType.IMM:
                    float xRandomIMM = GameData.Noise.GetPerlin(pos.SX, pos.EX, pos.Frame) / 2f + 0.5f;
                    float yRandomIMM = GameData.Noise.GetPerlin(pos.Frame, pos.SY, pos.EY) / 2f + 0.5f;
                    float xIMM = pos.SX + (pos.EX - pos.SX) * xRandomIMM;
                    float yIMM = pos.SY + (pos.EY - pos.SY) * yRandomIMM;
                    return fullOffset + new Vector3(xIMM, yIMM, 0);
                case VectorRandomType.MM:
                    float xRandomMM = GameData.Noise.GetPerlin(pos.SX, pos.EX, pos.Frame) / 2f + 0.5f;
                    float yRandomMM = GameData.Noise.GetPerlin(pos.Frame, pos.SY, pos.EY) / 2f + 0.5f;
                    float xMM = GameUtils.Interval(pos.SX + (pos.EX - pos.SX) * xRandomMM, pos.SX, pos.EX, pos.Interval);
                    float yMM = GameUtils.Interval(pos.SY + (pos.EY - pos.SY) * yRandomMM, pos.SY, pos.EY, pos.Interval);
                    return fullOffset + new Vector3(xMM, yMM, 0);
                case VectorRandomType.C:
                    float angleC = GameData.Noise.GetPerlin(pos.EX, pos.Frame, pos.EY);
                    return fullOffset + GameUtils.RandomPointOnCircle(pos.SX, pos.SY, angleC, pos.Interval);
                case VectorRandomType.M:
                    float multiplyRandomM = GameData.Noise.GetPerlin(pos.EX, pos.Frame, pos.EY) / 2f + 0.5f;
                    return fullOffset + new Vector3(pos.SX, pos.SY, 0) * (pos.EX + (pos.EY - pos.EX) * multiplyRandomM);
            }
            return Vector3.zero;
        }
        public static float CalculateRotChild(Rot rot, float rotParent)
        {
            switch (rot.RandomType)
            {
                case FloatRandomType.N:
                    return rot.SA * rotParent;
                case FloatRandomType.IMM:
                    float randomIMM = GameData.Noise.GetPerlin(rot.SA, rot.Frame, rot.EA) / 2f + 0.5f;
                    return rot.SA + (rot.EA - rot.SA) * randomIMM * rotParent;
                case FloatRandomType.MM:
                    float randomMM = GameData.Noise.GetPerlin(rot.SA, rot.Frame, rot.EA) / 2f + 0.5f;
                    return GameUtils.Interval(rot.SA + (rot.EA - rot.SA) * randomMM, rot.SA, rot.EA, rot.Interval) * rotParent;
                case FloatRandomType.M:
                    float multiplyRandomM = GameData.Noise.GetPerlin(rot.EA, rot.Frame, rot.Interval) / 2f + 0.5f;
                    return rot.SA * (rot.EA + (rot.Interval - rot.EA) * multiplyRandomM) * rotParent;
            }
            return 0;
        }
        public static Vector3 CalculateScaChild(Sca sca, Vector3 scaParent)
        {
            switch (sca.RandomType)
            {
                case VectorRandomType.N:
                    return new Vector3(sca.SX * scaParent.x, sca.SY * scaParent.y, 0);
                case VectorRandomType.IMM:
                    float xRandomIMM = GameData.Noise.GetPerlin(sca.SX, sca.EX, sca.Frame) / 2f + 0.5f;
                    float yRandomIMM = GameData.Noise.GetPerlin(sca.Frame, sca.SY, sca.EY) / 2f + 0.5f;
                    float xIMM = sca.SX + (sca.EX - sca.SX) * xRandomIMM;
                    float yIMM = sca.SY + (sca.EY - sca.SY) * yRandomIMM;
                    return new Vector3(xIMM * scaParent.x, yIMM * scaParent.y, 0);
                case VectorRandomType.MM:
                    float xRandomMM = GameData.Noise.GetPerlin(sca.SX, sca.EX, sca.Frame) / 2f + 0.5f;
                    float yRandomMM = GameData.Noise.GetPerlin(sca.Frame, sca.SY, sca.EY) / 2f + 0.5f;
                    float xMM = GameUtils.Interval(sca.SX + (sca.EX - sca.SX) * xRandomMM, sca.SX, sca.EX, sca.Interval);
                    float yMM = GameUtils.Interval(sca.SY + (sca.EY - sca.SY) * yRandomMM, sca.SY, sca.EY, sca.Interval);
                    return new Vector3(xMM * scaParent.x, yMM * scaParent.y, 0);
                case VectorRandomType.C:
                    float angleC = GameData.Noise.GetPerlin(sca.EX, sca.Frame, sca.EY);
                    return GameUtils.RandomPointOnCircle(sca.SX, sca.SY, angleC, sca.Interval, scaParent);
                case VectorRandomType.M:
                    float multiplyRandomM = GameData.Noise.GetPerlin(sca.EX, sca.Frame, sca.EY) / 2f + 0.5f;
                    return new Vector3(sca.SX * scaParent.x, sca.SY * scaParent.y, 0) * (sca.EX + (sca.EY - sca.EX) * multiplyRandomM);
            }
            return Vector3.zero;
        }
    }
}
