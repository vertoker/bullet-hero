using System.Collections.Generic;
using Game.Components;
using Game.Provider;
using UnityEngine;
using System;
using Data;

namespace Game.Static
{
    public static class CalculateData
    {
        public static Vector3 CalculatePos(Pos pos, Vector3 local, AnchorPresets anchor)
        {
            Vector3 center = GameUtils.CenterAnchor(anchor);
            switch (pos.RandomType)
            {
                case VectorRandomType.N:
                    return center + local + new Vector3(pos.SX, pos.SY, 0);
                case VectorRandomType.IMM:
                    float xRandomIMM = GameData.Noise.GetPerlin(pos.SX, pos.EX, pos.Frame) / 2f + 0.5f;
                    float yRandomIMM = GameData.Noise.GetPerlin(pos.Frame, pos.SY, pos.EY) / 2f + 0.5f;
                    float xIMM = pos.SX + (pos.EX - pos.SX) * xRandomIMM;
                    float yIMM = pos.SY + (pos.EY - pos.SY) * yRandomIMM;
                    return center + local + new Vector3(xIMM, yIMM, 0);
                case VectorRandomType.MM:
                    float xRandomMM = GameData.Noise.GetPerlin(pos.SX, pos.EX, pos.Frame) / 2f + 0.5f;
                    float yRandomMM = GameData.Noise.GetPerlin(pos.Frame, pos.SY, pos.EY) / 2f + 0.5f;
                    float xMM = GameUtils.Interval(pos.SX + (pos.EX - pos.SX) * xRandomMM, pos.SX, pos.EX, pos.Interval);
                    float yMM = GameUtils.Interval(pos.SY + (pos.EY - pos.SY) * yRandomMM, pos.SY, pos.EY, pos.Interval);
                    return center + local + new Vector3(xMM, yMM, 0);
                case VectorRandomType.C:
                    float angleC = GameData.Noise.GetPerlin(pos.EX, pos.Frame, pos.EY);
                    return GameUtils.RandomPointOnCircle(pos.SX, pos.SY, angleC, pos.Interval, center + local);
                case VectorRandomType.M:
                    float multiplyRandomM = GameData.Noise.GetPerlin(pos.EX, pos.Frame, pos.EY) / 2f + 0.5f;
                    return center + local + new Vector3(pos.SX, pos.SY, 0) * (pos.EX + (pos.EY - pos.EX) * multiplyRandomM);
            }
            return Vector3.zero;
        }
        public static float CalculateRot(Rot rot)
        {
            switch (rot.RandomType)
            {
                case FloatRandomType.N:
                    return rot.SA;
                case FloatRandomType.IMM:
                    float randomIMM = GameData.Noise.GetPerlin(rot.SA, rot.Frame, rot.EA) / 2f + 0.5f;
                    return rot.SA + (rot.EA - rot.SA) * randomIMM;
                case FloatRandomType.MM:
                    float randomMM = GameData.Noise.GetPerlin(rot.SA, rot.Frame, rot.EA) / 2f + 0.5f;
                    return GameUtils.Interval(rot.SA + (rot.EA - rot.SA) * randomMM, rot.SA, rot.EA, rot.Interval);
                case FloatRandomType.M:
                    float multiplyRandomM = GameData.Noise.GetPerlin(rot.EA, rot.Frame, rot.Interval) / 2f + 0.5f;
                    return rot.SA * (rot.EA + (rot.Interval - rot.EA) * multiplyRandomM);
            }
            return 0;
        }
        public static Vector3 CalculateSca(Sca sca)
        {
            switch (sca.RandomType)
            {
                case VectorRandomType.N:
                    return new Vector3(sca.SX, sca.SY, 0);
                case VectorRandomType.IMM:
                    float xRandomIMM = GameData.Noise.GetPerlin(sca.SX, sca.EX, sca.Frame) / 2f + 0.5f;
                    float yRandomIMM = GameData.Noise.GetPerlin(sca.Frame, sca.SY, sca.EY) / 2f + 0.5f;
                    float xIMM = sca.SX + (sca.EX - sca.SX) * xRandomIMM;
                    float yIMM = sca.SY + (sca.EY - sca.SY) * yRandomIMM;
                    return new Vector3(xIMM, yIMM, 0);
                case VectorRandomType.MM:
                    float xRandomMM = GameData.Noise.GetPerlin(sca.SX, sca.EX, sca.Frame) / 2f + 0.5f;
                    float yRandomMM = GameData.Noise.GetPerlin(sca.Frame, sca.SY, sca.EY) / 2f + 0.5f;
                    float xMM = GameUtils.Interval(sca.SX + (sca.EX - sca.SX) * xRandomMM, sca.SX, sca.EX, sca.Interval);
                    float yMM = GameUtils.Interval(sca.SY + (sca.EY - sca.SY) * yRandomMM, sca.SY, sca.EY, sca.Interval);
                    return new Vector3(xMM, yMM, 0);
                case VectorRandomType.C:
                    float angleC = GameData.Noise.GetPerlin(sca.EX, sca.Frame, sca.EY);
                    return GameUtils.RandomPointOnCircle(sca.SX, sca.SY, angleC, sca.Interval);
                case VectorRandomType.M:
                    float multiplyRandomM = GameData.Noise.GetPerlin(sca.EX, sca.Frame, sca.EY) / 2f + 0.5f;
                    return new Vector3(sca.SX, sca.SY, 0) * (sca.EX + (sca.EY - sca.EX) * multiplyRandomM);
            }
            return Vector3.zero;
        }
        public static Vector4 CalculateClr(Clr clr)
        {
            switch (clr.RandomType)
            {
                case ColorRandomType.N:
                    return new Vector4(clr.SR, clr.SG, clr.SB, clr.SA);
                case ColorRandomType.IMM:
                    float rRandomIMM = GameData.Noise.GetPerlin(clr.Frame, clr.SR, clr.ER) / 2f + 0.5f;
                    float gRandomIMM = GameData.Noise.GetPerlin(clr.SG, clr.Frame, clr.EG) / 2f + 0.5f;
                    float bRandomIMM = GameData.Noise.GetPerlin(clr.SB, clr.EB, clr.Frame) / 2f + 0.5f;
                    float aRandomIMM = GameData.Noise.GetPerlin(clr.SA, -clr.Frame, clr.EA) / 2f + 0.5f;
                    float rIMM = clr.SR + (clr.ER - clr.SR) * rRandomIMM;
                    float gIMM = clr.SG + (clr.EG - clr.SG) * gRandomIMM;
                    float bIMM = clr.SB + (clr.EB - clr.SB) * bRandomIMM;
                    float aIMM = clr.SA + (clr.EA - clr.SA) * aRandomIMM;
                    return new Vector4(rIMM, gIMM, bIMM, aIMM);
                case ColorRandomType.MM:
                    float rRandomMM = GameData.Noise.GetPerlin(clr.Frame, clr.SR, clr.ER) / 2f + 0.5f;
                    float gRandomMM = GameData.Noise.GetPerlin(clr.SG, clr.Frame, clr.EG) / 2f + 0.5f;
                    float bRandomMM = GameData.Noise.GetPerlin(clr.SB, clr.EB, clr.Frame) / 2f + 0.5f;
                    float aRandomMM = GameData.Noise.GetPerlin(clr.SA, -clr.Frame, clr.EA) / 2f + 0.5f;
                    float rMM = GameUtils.Interval(clr.SR + (clr.ER - clr.SR) * rRandomMM, clr.SR, clr.ER, clr.Interval);
                    float gMM = GameUtils.Interval(clr.SG + (clr.EG - clr.SG) * gRandomMM, clr.SG, clr.EG, clr.Interval);
                    float bMM = GameUtils.Interval(clr.SB + (clr.EB - clr.SB) * bRandomMM, clr.SB, clr.EB, clr.Interval);
                    float aMM = GameUtils.Interval(clr.SA + (clr.EA - clr.SA) * aRandomMM, clr.SA, clr.EA, clr.Interval);
                    return new Vector4(rMM, gMM, bMM, aMM);
            }
            return Vector4.zero;
        }
    }
}
