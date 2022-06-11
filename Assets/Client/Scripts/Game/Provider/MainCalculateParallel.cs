using Game.Components;

using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Game.Static;
using Unity.Jobs;
using Utils;
using Data;

namespace Game.Provider
{
    [BurstCompile]
    public struct MainCalculateParallel : IJobParallelFor
    {
        [ReadOnly] public float3 borderScreen;
        [ReadOnly] public int activeFrame, seed;
        [ReadOnly] public NativeArray<AnchorPresets> anchorArray;

        [ReadOnly] public NativeArray<Pos> posArray;
        [ReadOnly] public NativeArray<Rot> rotArray;
        [ReadOnly] public NativeArray<Sca> scaArray;
        [ReadOnly] public NativeArray<Clr> clrArray;
        [ReadOnly] public NativeArray<int> posCountArray;
        [ReadOnly] public NativeArray<int> rotCountArray;
        [ReadOnly] public NativeArray<int> scaCountArray;
        [ReadOnly] public NativeArray<int> clrCountArray;
        [ReadOnly] public NativeArray<int> posLengthArray;
        [ReadOnly] public NativeArray<int> rotLengthArray;
        [ReadOnly] public NativeArray<int> scaLengthArray;
        [ReadOnly] public NativeArray<int> clrLengthArray;
        [ReadOnly] public int countActivePrefabs;

        public NativeArray<float3> poses;
        public NativeArray<float3> rotes;
        public NativeArray<float3> scaes;
        public NativeArray<float4> clres;

        public void Execute(int index)
        {
            var noise = new FastNoise(seed);

            clres[index] = GetData.GetClr(clrArray.GetSubArray(clrCountArray[index], clrLengthArray[index]), activeFrame, ref noise);
            rotes[index] = GetData.GetRot(rotArray.GetSubArray(rotCountArray[index], rotLengthArray[index]), activeFrame, ref noise);
            scaes[index] = GetData.GetSca(scaArray.GetSubArray(scaCountArray[index], scaLengthArray[index]), activeFrame, ref noise);
            poses[index] = GetData.GetPos(posArray.GetSubArray(posCountArray[index], posLengthArray[index]), activeFrame, anchorArray[index], ref noise, ref borderScreen);
        }
    }
}
