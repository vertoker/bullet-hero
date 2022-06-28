using System.Collections;
using UnityEngine.UI;
using Game.Components;
using Game.Provider;
using UnityEngine;
using Utils.Pool;
using Data;

using UtilsStatic = Utils.Static;

using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

namespace LevelEditor.TimelineContent
{
    public class PrefabHandler : ContentHandler
    {
        [SerializeField] private PoolData data;
        [SerializeField] private PoolSpawner spawner;

        private PrefabElement[] prefabs;
        private int prefabsCount = 0;

        private int[] startFrames;
        private int[] endFrames;

        public const float SIZE_HEIGTH = 77f;

        private void OnEnable()
        {
            LevelHolder.UpdateLevelData += UpdateLevelData;
        }
        private void OnDisable()
        {
            LevelHolder.UpdateLevelData -= UpdateLevelData;
        }
        private void Start()
        {
            prefabs = new PrefabElement[data.GetCapacity];
            for (int i = 0; i < data.GetCapacity; i++)
            {
                prefabs[i] = spawner.Dequeue().GetComponent<PrefabElement>();
            }
        }

        private void UpdateLevelData(Level level)
        {
            prefabsCount = level.Prefabs.Count;

            startFrames = new int[prefabsCount];
            endFrames = new int[prefabsCount];

            for (int i = 0; i < prefabsCount; i++)
            {
                startFrames[i] = level.Prefabs[i].StartFrame;
                endFrames[i] = level.Prefabs[i].EndFrame;
            }
        }
        public override void UpdateUI(int startFrame, int endFrame, int startHeigth, int endHeigth)
        {
            FindFramePrefabs(out NativeArray<int> activePrefabs, out int activeCount,
                startFrame, endFrame, startHeigth, endHeigth);

            for (int i = 0; i < activeCount; i++)
            {
                Prefab prefab = LevelHolder.Level.Prefabs[activePrefabs[i]];
                float length = UtilsStatic.Frame2Sec(prefab.EndFrame - prefab.StartFrame) * UtilsStatic.SecondLength;
                float startPoint = UtilsStatic.Frame2Sec(prefab.StartFrame) * UtilsStatic.SecondLength;
                Vector2 position = new Vector2(startPoint, -prefab.Height * SIZE_HEIGTH);
                Vector2 size = new Vector2(length, SIZE_HEIGTH);
                prefabs[i].SetData(position, size);
                prefabs[i].Enable();
            }
            for (int i = activeCount; i < data.GetCapacity; i++)
            {
                prefabs[i].Disable();
            }

            activePrefabs.Dispose();
        }
        private void FindFramePrefabs(out NativeArray<int> activePrefabs, out int activeCount,
            int startFrame, int endFrame, int startHeigth, int endHeigth)
        {
            activePrefabs = new NativeArray<int>(Runtime.CountBlocks, Allocator.TempJob);
            NativeArray<int> counter = new NativeArray<int>(1, Allocator.TempJob);
            NativeArray<int> startFrames = new NativeArray<int>(this.startFrames, Allocator.TempJob);
            NativeArray<int> endFrames = new NativeArray<int>(this.endFrames, Allocator.TempJob);

            FindTimelineBlocksParallel findTimelineBlocksParallelJob = new FindTimelineBlocksParallel
            {
                startFrame = startFrame,
                endFrame = endFrame,
                startFrames = startFrames,
                endFrames = endFrames,
                activePrefabs = activePrefabs,
                counter = counter
            };
            JobHandle mainSimpleJobHandle = findTimelineBlocksParallelJob.Schedule(prefabsCount, 50);
            mainSimpleJobHandle.Complete();

            activeCount = counter[0];
            startFrames.Dispose();
            endFrames.Dispose();
            counter.Dispose();
        }
    }
}