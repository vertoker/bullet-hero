using Game.Components;

using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;

namespace LevelEditor.TimelineContent
{
    [BurstCompile]
    public struct FindTimelineBlocksParallel : IJobParallelFor
    {
        [ReadOnly] public int startFrame, endFrame;
        [ReadOnly] public NativeArray<int> startFrames;
        [ReadOnly] public NativeArray<int> endFrames;
        public NativeArray<int> activePrefabs;
        public NativeArray<int> counter;

        public void Execute(int index)
        {
            if (startFrame <= endFrames[index] && startFrames[index] <= endFrame)
            {
                activePrefabs[counter[0]] = index;
                counter[0]++;
            }
        }
    }
}
