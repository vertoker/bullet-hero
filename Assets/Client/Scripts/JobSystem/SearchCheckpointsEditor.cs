using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public static class StaticSearchCheckpointsEditor
{
    public static GetListRender searchCheckpointsEditor = (int startFrame, int endFrame, int startHeigth, int endHeigth, out int[] indexes) =>
    {
        int length = LevelManager.level.Checkpoints.Count;
        NativeArray<int> frames = new NativeArray<int>(length, Allocator.TempJob);
        NativeArray<int> indexesArray = new NativeArray<int>(length + 1, Allocator.TempJob);

        for (int i = 0; i < length; i++)
        {
            frames[i] = LevelManager.level.Checkpoints[i].Frame;
        }
        indexesArray[0] = 0;

        SearchCheckpointsEditor searchCheckpointsEditorParallelJob = new SearchCheckpointsEditor
        {
            frames = frames,
            indexes = indexesArray,
            startFrame = startFrame,
            endFrame = endFrame
        };
        JobHandle jobHandle = searchCheckpointsEditorParallelJob.Schedule(length, 10);
        jobHandle.Complete();

        length = searchCheckpointsEditorParallelJob.indexes[0];
        IData[] data = new IData[length];
        indexes = new int[length];

        for (int i = 0; i < length; i++)
        {
            int id = searchCheckpointsEditorParallelJob.indexes[i + 1];
            data[i] = LevelManager.level.Checkpoints[id];
            indexes[i] = id;
        }

        frames.Dispose();
        indexesArray.Dispose();
        return data;
    };
}

[BurstCompile]
public struct SearchCheckpointsEditor : IJobParallelFor
{
    [ReadOnly] public NativeArray<int> frames;
    public NativeArray<int> indexes;
    [ReadOnly] public int startFrame;
    [ReadOnly] public int endFrame;

    public void Execute(int index)
    {
        if (startFrame < frames[index] && endFrame < frames[index])
        {
            indexes[indexes[0]] = index;
            indexes[0]++;
        }
    }
}