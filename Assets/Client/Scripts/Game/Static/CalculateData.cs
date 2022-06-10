using System.Collections.Generic;
using Unity.Mathematics;
using Game.Components;
using Game.Provider;
using UnityEngine;
using Utils;
using Data;

namespace Game.Static
{
    public static class CalculateData
    {
        public static float3 CalculatePos(Pos pos, AnchorPresets anchor, ref FastNoise noise, ref float3 borderScreen)
        {
            float3 center = GameUtils.GetCoefficient(anchor, ref borderScreen);
            switch (pos.RandomType)
            {
                case VectorRandomType.N:
                    return center + new float3(pos.SX, pos.SY, 0);
                case VectorRandomType.IMM:
                    float xRandomIMM = noise.GetPerlin(pos.SX, pos.EX, pos.Frame) / 2f + 0.5f;
                    float yRandomIMM = noise.GetPerlin(pos.Frame, pos.SY, pos.EY) / 2f + 0.5f;
                    float xIMM = pos.SX + (pos.EX - pos.SX) * xRandomIMM;
                    float yIMM = pos.SY + (pos.EY - pos.SY) * yRandomIMM;
                    return center + new float3(xIMM, yIMM, 0);
                case VectorRandomType.MM:
                    float xRandomMM = noise.GetPerlin(pos.SX, pos.EX, pos.Frame) / 2f + 0.5f;
                    float yRandomMM = noise.GetPerlin(pos.Frame, pos.SY, pos.EY) / 2f + 0.5f;
                    float xMM = GameUtils.Interval(pos.SX + (pos.EX - pos.SX) * xRandomMM, pos.SX, pos.EX, pos.Interval);
                    float yMM = GameUtils.Interval(pos.SY + (pos.EY - pos.SY) * yRandomMM, pos.SY, pos.EY, pos.Interval);
                    return center + new float3(xMM, yMM, 0);
                case VectorRandomType.C:
                    float angleC = noise.GetPerlin(pos.EX, pos.Frame, pos.EY);
                    return GameUtils.RandomPointOnCircle(pos.SX, pos.SY, angleC, pos.Interval, center);
                case VectorRandomType.M:
                    float multiplyRandomM = noise.GetPerlin(pos.EX, pos.Frame, pos.EY) / 2f + 0.5f;
                    return center + new float3(pos.SX, pos.SY, 0) * (pos.EX + (pos.EY - pos.EX) * multiplyRandomM);
            }
            return float3.zero;
        }
        public static float CalculateRot(Rot rot, ref FastNoise noise)
        {
            switch (rot.RandomType)
            {
                case FloatRandomType.N:
                    return rot.SA;
                case FloatRandomType.IMM:
                    float randomIMM = noise.GetPerlin(rot.SA, rot.Frame, rot.EA) / 2f + 0.5f;
                    return rot.SA + (rot.EA - rot.SA) * randomIMM;
                case FloatRandomType.MM:
                    float randomMM = noise.GetPerlin(rot.SA, rot.Frame, rot.EA) / 2f + 0.5f;
                    return GameUtils.Interval(rot.SA + (rot.EA - rot.SA) * randomMM, rot.SA, rot.EA, rot.Interval);
                case FloatRandomType.M:
                    float multiplyRandomM = noise.GetPerlin(rot.EA, rot.Frame, rot.Interval) / 2f + 0.5f;
                    return rot.SA * (rot.EA + (rot.Interval - rot.EA) * multiplyRandomM);
            }
            return 0;
        }
        public static float3 CalculateSca(Sca sca, ref FastNoise noise)
        {
            switch (sca.RandomType)
            {
                case VectorRandomType.N:
                    return new float3(sca.SX, sca.SY, 0);
                case VectorRandomType.IMM:
                    float xRandomIMM = noise.GetPerlin(sca.SX, sca.EX, sca.Frame) / 2f + 0.5f;
                    float yRandomIMM = noise.GetPerlin(sca.Frame, sca.SY, sca.EY) / 2f + 0.5f;
                    float xIMM = sca.SX + (sca.EX - sca.SX) * xRandomIMM;
                    float yIMM = sca.SY + (sca.EY - sca.SY) * yRandomIMM;
                    return new float3(xIMM, yIMM, 0);
                case VectorRandomType.MM:
                    float xRandomMM = noise.GetPerlin(sca.SX, sca.EX, sca.Frame) / 2f + 0.5f;
                    float yRandomMM = noise.GetPerlin(sca.Frame, sca.SY, sca.EY) / 2f + 0.5f;
                    float xMM = GameUtils.Interval(sca.SX + (sca.EX - sca.SX) * xRandomMM, sca.SX, sca.EX, sca.Interval);
                    float yMM = GameUtils.Interval(sca.SY + (sca.EY - sca.SY) * yRandomMM, sca.SY, sca.EY, sca.Interval);
                    return new float3(xMM, yMM, 0);
                case VectorRandomType.C:
                    float angleC = noise.GetPerlin(sca.EX, sca.Frame, sca.EY);
                    return GameUtils.RandomPointOnCircle(sca.SX, sca.SY, angleC, sca.Interval);
                case VectorRandomType.M:
                    float multiplyRandomM = noise.GetPerlin(sca.EX, sca.Frame, sca.EY) / 2f + 0.5f;
                    return new float3(sca.SX, sca.SY, 0) * (sca.EX + (sca.EY - sca.EX) * multiplyRandomM);
            }
            return float3.zero;
        }
        public static float4 CalculateClr(Clr clr, ref FastNoise noise)
        {
            switch (clr.RandomType)
            {
                case ColorRandomType.N:
                    return new float4(clr.SR, clr.SG, clr.SB, clr.SA);
                case ColorRandomType.IMM:
                    float rRandomIMM = noise.GetPerlin(clr.Frame, clr.SR, clr.ER) / 2f + 0.5f;
                    float gRandomIMM = noise.GetPerlin(clr.SG, clr.Frame, clr.EG) / 2f + 0.5f;
                    float bRandomIMM = noise.GetPerlin(clr.SB, clr.EB, clr.Frame) / 2f + 0.5f;
                    float aRandomIMM = noise.GetPerlin(clr.SA, -clr.Frame, clr.EA) / 2f + 0.5f;
                    float rIMM = clr.SR + (clr.ER - clr.SR) * rRandomIMM;
                    float gIMM = clr.SG + (clr.EG - clr.SG) * gRandomIMM;
                    float bIMM = clr.SB + (clr.EB - clr.SB) * bRandomIMM;
                    float aIMM = clr.SA + (clr.EA - clr.SA) * aRandomIMM;
                    return new float4(rIMM, gIMM, bIMM, aIMM);
                case ColorRandomType.MM:
                    float rRandomMM = noise.GetPerlin(clr.Frame, clr.SR, clr.ER) / 2f + 0.5f;
                    float gRandomMM = noise.GetPerlin(clr.SG, clr.Frame, clr.EG) / 2f + 0.5f;
                    float bRandomMM = noise.GetPerlin(clr.SB, clr.EB, clr.Frame) / 2f + 0.5f;
                    float aRandomMM = noise.GetPerlin(clr.SA, -clr.Frame, clr.EA) / 2f + 0.5f;
                    float rMM = GameUtils.Interval(clr.SR + (clr.ER - clr.SR) * rRandomMM, clr.SR, clr.ER, clr.Interval);
                    float gMM = GameUtils.Interval(clr.SG + (clr.EG - clr.SG) * gRandomMM, clr.SG, clr.EG, clr.Interval);
                    float bMM = GameUtils.Interval(clr.SB + (clr.EB - clr.SB) * bRandomMM, clr.SB, clr.EB, clr.Interval);
                    float aMM = GameUtils.Interval(clr.SA + (clr.EA - clr.SA) * aRandomMM, clr.SA, clr.EA, clr.Interval);
                    return new float4(rMM, gMM, bMM, aMM);
            }
            return float4.zero;
        }
    }
}
