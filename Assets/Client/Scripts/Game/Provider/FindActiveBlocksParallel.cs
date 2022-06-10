using Game.Components;

using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;

namespace Game.Provider
{
    [BurstCompile]
    public struct FindActiveBlocksParallel : IJobParallelFor
    {
        [ReadOnly] public int activeFrame, maxFrame;
        [ReadOnly] public NativeArray<int> startFrames;
        [ReadOnly] public NativeArray<int> endFrames;
        [ReadOnly] public NativeArray<bool> prefabActives;
        public NativeArray<int> activePrefabs;
        public NativeArray<int> counter;

        public void Execute(int index)
        {
            if (activeFrame >= startFrames[index] && activeFrame <= endFrames[index] && prefabActives[index])
            {
                activePrefabs[counter[0]] = index;
                counter[0]++;
            }

            /*int length = allPrefabs.Length;
            int progress = (int)(length * (float)(activeFrame / maxFrame));

            for (int index = progress; index < length; index++)
            {
                if (activeFrame >= allPrefabs[index].StartFrame && activeFrame <= allPrefabs[index].EndFrame && allPrefabs[index].Active)
                {
                    activePrefabs[counter[0]] = allPrefabs[index];
                    counter[0]++;
                }
                else
                    break;
            }
            for (int index = progress - 1; index >= 0; index--)
            {
                if (activeFrame >= allPrefabs[index].StartFrame && activeFrame <= allPrefabs[index].EndFrame && allPrefabs[index].Active)
                {
                    activePrefabs[counter[0]] = allPrefabs[index];
                    counter[0]++;
                }
                else
                    break;
            }*/
        }
    }
}
