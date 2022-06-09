using Game.Components;

using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;

namespace Game.Provider
{
    [BurstCompile]
    public struct FindActiveBlocksParallel : IJob
    {
        [ReadOnly] public int activeFrame, maxFrame;
        [ReadOnly] public NativeArray<int> startFrames;
        [ReadOnly] public NativeArray<int> endFrames;
        public NativeArray<int> activeBlocks;

        public void Execute()
        {
            int length = startFrames.Length;
            int progress = (int)(length * (float)(activeFrame / maxFrame));

            for (int i = 0; i < length; i++)
            {
                int id = GetNextID(i, ref progress, ref length);// Закончить
            }

            if (activeFrame >= startFrames[index] && activeFrame <= endFrames[index])
            {
                activeBlocks[counter] = index;
                counter++;
            }
        }

        public static int GetNextID(int id, ref int progress, ref int length)
        {
            int half = (id + 1) / 2;
            if (id % 2 == 0)
                return progress + half;
            return progress - half;
        }
    }
}
