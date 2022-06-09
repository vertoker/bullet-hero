using System.Collections;
using System.Collections.Generic;
using Game.Components;
using UnityEngine;
using System.Linq;
using Data;

using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;

namespace Game.Provider
{
    public class Runtime : MonoBehaviour
    {
        [SerializeField] private Transform[] trs;
        [SerializeField] private SpriteRenderer[] srs;
        [Space]
        [SerializeField] private List<Prefab> prefabs;

        private const float FrameRate = 60;
        private const int CountBlocks = 1000;

        [SerializeField] private float timer = 0f;
        [SerializeField] private int activeFrame = 0;
        //[SerializeField] private float timerFrame = 0f;

        private void Awake()
        {
            for (int i = 0; i < CountBlocks; i++)
            {
                trs[i] = transform.GetChild(i).GetComponent<Transform>();
                srs[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            }
        }

        public void LoadLevel(Level level)
        {
            prefabs = new List<Prefab>();
            foreach (var prefab in level.Prefabs)
                prefabs.Add(prefab);

            prefabs = prefabs.OrderBy(x => x.StartFrame).ThenByDescending(x => x.EndFrame).ToList();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            int lastFrame = activeFrame;
            activeFrame = Mathf.FloorToInt(timer * FrameRate);
            //timerFrame = activeFrame / FrameRate;
            if (activeFrame == lastFrame)
                return;

            NativeArray<Prefab> prefabs = FindFramePrefabs();
        }

        private NativeArray<Prefab> FindFramePrefabs()
        {
            NativeArray<Prefab> all = new NativeArray<Prefab>(prefabs.ToArray(), Allocator.TempJob);

            FindActiveBlocksParallel findActiveBlocksParallelJob = new FindActiveBlocksParallel
            {

            };
            JobHandle mainSimpleJobHandle = findActiveBlocksParallelJob.Schedule();
            mainSimpleJobHandle.Complete();
        }
    }
}