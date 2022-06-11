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
    public struct CollisionDetectionParallel : IJob
    {
        [ReadOnly] public NativeArray<float3> positionPlayers;
        [ReadOnly] public NativeArray<SpriteType> spriteArray;
        [ReadOnly] public NativeArray<bool> colliderArray;
        [ReadOnly] public int countActivePrefabs;

        public NativeArray<float3> poses;
        public NativeArray<float3> rotes;
        public NativeArray<float3> scaes;
        public NativeArray<bool> collidedPlayers;

        public void Execute()
        {
            for (int index = 0; index < countActivePrefabs; index++)
            {
                if (colliderArray[index])
                {
                    if (CollisionDetection.GetDetection(poses[index], rotes[index], scaes[index], positionPlayers[0], spriteArray[index]))
                    {
                        collidedPlayers[0] = true;
                        break;
                    }
                }
            }
        }
    }
}
