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
    public static GetListRender searchPrefabsEditor = (int startFrame, int endFrame, int startHeigth, int endHeigth, out int[] indexes) =>
    {
        int length = LevelManager.level.Prefabs.Count;
        NativeArray<int> startFrames = new NativeArray<int>(length, Allocator.TempJob);
        NativeArray<int> endFrames = new NativeArray<int>(length, Allocator.TempJob);
        NativeArray<int> heigths = new NativeArray<int>(length, Allocator.TempJob);
        NativeArray<int> indexesArray = new NativeArray<int>(length + 1, Allocator.TempJob);

        for (int i = 0; i < length; i++)
        {
            startFrames[i] = LevelManager.level.Prefabs[i].StartFrame;
            endFrames[i] = LevelManager.level.Prefabs[i].EndFrame;
            heigths[i] = LevelManager.level.Prefabs[i].Height;
        }
        indexesArray[0] = 0;

        SearchPrefabsEditor searchPrefabsEditorParallelJob = new SearchPrefabsEditor
        {
            startFrames = startFrames,
            endFrames = endFrames,
            heigths = heigths,
            indexesPrefabs = indexesArray,

            startFrame = startFrame,
            endFrame = endFrame,
            startHeigth = startHeigth,
            endHeigth = endHeigth
        };
        JobHandle jobHandle = searchPrefabsEditorParallelJob.Schedule(length, 50);
        jobHandle.Complete();

        length = searchPrefabsEditorParallelJob.indexesPrefabs[0];
        IData[] data = new IData[length];
        indexes = new int[length];
        for (int i = 0; i < length; i++)
        {
            int id = searchPrefabsEditorParallelJob.indexesPrefabs[i + 1];
            data[i] = LevelManager.level.Prefabs[id];
            indexes[i] = id;
        }

        startFrames.Dispose();
        endFrames.Dispose();
        heigths.Dispose();
        indexesArray.Dispose();
        return data;
    };
}

[BurstCompile]
public struct SearchPrefabsEditor : IJobParallelFor
{
    [ReadOnly] public NativeArray<int> startFrames;
    [ReadOnly] public NativeArray<int> endFrames;
    [ReadOnly] public NativeArray<int> heigths;
    public NativeArray<int> indexesPrefabs;

    [ReadOnly] public int startFrame;
    [ReadOnly] public int endFrame;
    [ReadOnly] public int startHeigth;
    [ReadOnly] public int endHeigth;

    public void Execute(int index)
    {
        if ((startFrame < endFrames[index] || endFrame > startFrames[index]) && (startHeigth <= heigths[index] || endHeigth >= heigths[index]))
        {
            indexesPrefabs[indexesPrefabs[0]] = index;
            indexesPrefabs[0]++;
        }
    }
}