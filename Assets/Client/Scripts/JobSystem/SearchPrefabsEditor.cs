using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public static class StaticSearchPrefabsEditor
{
    public static GetListRender searchPrefabsEditor = (int startFrame, int endFrame, int startHeigth, int endHeigth) =>
    {
        int length = LevelManager.level.Prefabs.Count;
        NativeArray<int> startFrames = new NativeArray<int>(length, Allocator.Temp);
        NativeArray<int> endFrames = new NativeArray<int>(length, Allocator.Temp);
        NativeArray<int> heigths = new NativeArray<int>(length, Allocator.Temp);

        for (int i = 0; i < length; i++)
        {
            startFrames[i] = LevelManager.level.Prefabs[i].StartFrame;
            endFrames[i] = LevelManager.level.Prefabs[i].EndFrame;
            heigths[i] = LevelManager.level.Prefabs[i].Height;
        }

        SearchPrefabsEditor searchPrefabsEditorParallelJob = new SearchPrefabsEditor
        {
            startFrames = startFrames,
            endFrames = endFrames,
            heigths = heigths,
            indexesPrefabs = new NativeArray<int>(length, Allocator.Temp),

            startFrame = startFrame,
            endFrame = endFrame,
            startHeigth = startHeigth,
            endHeigth = endHeigth,
            counter = 0
        };
        JobHandle jobHandle = searchPrefabsEditorParallelJob.Schedule(length, 50);
        jobHandle.Complete();

        List<IData> data = new List<IData>(searchPrefabsEditorParallelJob.counter);
        for (int i = 0; i < searchPrefabsEditorParallelJob.counter; i++)
            data[i] = LevelManager.level.Prefabs[searchPrefabsEditorParallelJob.indexesPrefabs[i]];

        startFrames.Dispose();
        endFrames.Dispose();
        heigths.Dispose();
        return data;
    };
}

[BurstCompile]
public struct SearchPrefabsEditor : IJobParallelFor
{
    [ReadOnly] public NativeArray<int> startFrames;
    [ReadOnly] public NativeArray<int> endFrames;
    [ReadOnly] public NativeArray<int> heigths;
    [ReadOnly] public NativeArray<int> indexesPrefabs;

    [ReadOnly] public int startFrame;
    [ReadOnly] public int endFrame;
    [ReadOnly] public int startHeigth;
    [ReadOnly] public int endHeigth;
    public int counter;

    public void Execute(int index)
    {
        if ((startFrame < endFrames[index] || endFrame > startFrames[index]) && (startHeigth <= heigths[index] || endHeigth >= heigths[index]))
        {
            indexesPrefabs[counter] = index;
            counter++;
        }
    }
}