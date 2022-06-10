using System.Collections;
using System.Collections.Generic;
using Game.Components;
using UnityEngine;
using System.Linq;
using Game.Core;
using Data;

using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

namespace Game.Provider
{
    public class Runtime : MonoBehaviour
    {
        [SerializeField] private int seed = 666;
        [Space]

        private Transform self;
        private Transform[] trs;
        private SpriteRenderer[] srs;

        private int prefabsCount;
        private int lastActivePrefabs = 0;
        private float3 borderScreen;

        // Prefabs
        private List<Prefab> prefabs;
        private int[] startFrames;
        private int[] endFrames;
        private bool[] prefabActives;
        private bool[] prefabColliders;
        private Pos[][] prefabPoses;
        private Rot[][] prefabRotes;
        private Sca[][] prefabScaes;
        private Clr[][] prefabClres;
        private AnchorPresets[] prefabAnchorses;
        private SpriteType[] prefabSprites;

        private int playersCount;
        private PositionDelegate[] playerPositions;
        private HealthDelegate[] playerDamages;

        private const float FrameRate = 60;
        private const int CountBlocks = 1000;

        private int maxFrame = 0;
        [SerializeField] private float timer = 0f;
        [SerializeField] private int activeFrame = 0;
        private Coroutine updater;

        private void Awake()
        {
            self = transform;
            trs = new Transform[CountBlocks];
            srs = new SpriteRenderer[CountBlocks];
            for (int i = 0; i < CountBlocks; i++)
            {
                trs[i] = self.GetChild(i).GetComponent<Transform>();
                srs[i] = self.GetChild(i).GetComponent<SpriteRenderer>();
            }

            float aspect = Screen.width / Screen.height;
            borderScreen = new float3(aspect * 10, 10, 0);
        }

        public void LoadLevel(Level level, Player[] players)
        {
            maxFrame = (int)(level.LevelData.EndFadeOut * FrameRate);
            prefabsCount = level.Prefabs.Count;
            prefabs = new List<Prefab>();

            foreach (var prefab in level.Prefabs)
                prefabs.Add(prefab);
            prefabs = prefabs.OrderBy(x => x.StartFrame).ThenByDescending(x => x.EndFrame).ToList();

            startFrames = new int[prefabsCount];
            endFrames = new int[prefabsCount];
            prefabActives = new bool[prefabsCount];
            prefabColliders = new bool[prefabsCount];
            prefabPoses = new Pos[prefabsCount][];
            prefabRotes = new Rot[prefabsCount][];
            prefabScaes = new Sca[prefabsCount][];
            prefabClres = new Clr[prefabsCount][];
            prefabAnchorses = new AnchorPresets[prefabsCount];
            prefabSprites = new SpriteType[prefabsCount];

            for (int i = 0; i < prefabsCount; i++)
            {
                startFrames[i] = prefabs[i].StartFrame;
                endFrames[i] = prefabs[i].EndFrame;
                prefabActives[i] = prefabs[i].Active;
                prefabColliders[i] = prefabs[i].Collider;
                prefabPoses[i] = prefabs[i].Pos;
                prefabRotes[i] = prefabs[i].Rot;
                prefabScaes[i] = prefabs[i].Sca;
                prefabClres[i] = prefabs[i].Clr;
                prefabAnchorses[i] = prefabs[i].Anchor;
                prefabSprites[i] = prefabs[i].SpriteType;
            }

            playersCount = players.Length;
            playerPositions = new PositionDelegate[playersCount];
            playerDamages = new HealthDelegate[playersCount];
            for (int i = 0; i < playersCount; i++)
            {
                playerPositions[i] = players[i].GetPositionDelegate;
                playerDamages[i] = players[i].GetDamageDelegate;
            }

            updater = StartCoroutine(Updater());
            //UpdateCycle();
        }

        private IEnumerator Updater()
        {
            while (true)
            {
                yield return null;
                timer += Time.deltaTime;
                int lastFrame = activeFrame;
                activeFrame = Mathf.FloorToInt(timer * FrameRate);
                //timerFrame = activeFrame / FrameRate;
                if (activeFrame == lastFrame)
                    continue;
                UpdateCycle();
            }
        }

        private void UpdateCycle()
        {
            FindFramePrefabs(out NativeArray<int> activePrefabs, out int activeCount);
            CreateParentTree(ref activePrefabs, activeCount);
            CalculateFrame(ref activePrefabs, activeCount);
            lastActivePrefabs = activeCount;
            activePrefabs.Dispose();
        }
        private void FindFramePrefabs(out NativeArray<int> activePrefabs, out int activeCount)
        {
            activePrefabs = new NativeArray<int>(CountBlocks, Allocator.TempJob);
            NativeArray<int> counter = new NativeArray<int>(1, Allocator.TempJob);
            NativeArray<int> startFrames = new NativeArray<int>(this.startFrames, Allocator.TempJob);
            NativeArray<int> endFrames = new NativeArray<int>(this.endFrames, Allocator.TempJob);
            NativeArray<bool> prefabActives = new NativeArray<bool>(this.prefabActives, Allocator.TempJob);

            FindActiveBlocksParallel findActiveBlocksParallelJob = new FindActiveBlocksParallel
            {
                activeFrame = activeFrame,
                maxFrame = maxFrame,
                startFrames = startFrames,
                endFrames = endFrames,
                prefabActives = prefabActives,
                activePrefabs = activePrefabs,
                counter = counter
            };
            JobHandle mainSimpleJobHandle = findActiveBlocksParallelJob.Schedule(prefabsCount, 50);
            mainSimpleJobHandle.Complete();

            activeCount = counter[0];
            startFrames.Dispose();
            endFrames.Dispose();
            prefabActives.Dispose();
            counter.Dispose();
        }
        private void CreateParentTree(ref NativeArray<int> activePrefabs, int activeCount)
        {
            int counter = 0;
            List<int> parentsID = new List<int>();
            List<int> activesID = new List<int>();

            for (int i = activeCount; i < lastActivePrefabs; i++)
            {
                trs[i].SetParent(self);
                srs[i].sprite = null;
            }

            for (int i = 0; i < activeCount; i++)
            {
                var prefab = prefabs[activePrefabs[i]];
                if (prefab.ID == prefab.ParentID)
                {
                    parentsID.Add(prefab.ID);
                    trs[i].SetParent(self);
                    counter++;
                }
            }

            while (counter < activeCount)
            {
                for (int i = 0; i < activeCount; i++)
                {
                    var prefab = prefabs[activePrefabs[i]];
                    int parent = parentsID.FindIndex((int id) => { return prefab.ParentID == id; });
                    if (parent != -1)
                    {
                        activesID.Add(prefab.ParentID);
                        trs[i].SetParent(trs[parent]);
                        counter++;
                    }
                }

                var temp = parentsID;
                parentsID = activesID;
                activesID = temp;
                activesID.Clear();
            }
        }
        private void CalculateFrame(ref NativeArray<int> activePrefabs, int activeCount)
        {
            NativeArray<int> startFrames = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<int> endFrames = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<bool> prefabActives = new NativeArray<bool>(activeCount, Allocator.TempJob);
            NativeArray<bool> prefabColliders = new NativeArray<bool>(activeCount, Allocator.TempJob);

            NativeArray<int> posCountArray = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<int> rotCountArray = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<int> scaCountArray = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<int> clrCountArray = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<int> posLengthArray = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<int> rotLengthArray = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<int> scaLengthArray = new NativeArray<int>(activeCount, Allocator.TempJob);
            NativeArray<int> clrLengthArray = new NativeArray<int>(activeCount, Allocator.TempJob);

            NativeArray<float3> poses = new NativeArray<float3>(activeCount, Allocator.TempJob);
            NativeArray<float3> rotes = new NativeArray<float3>(activeCount, Allocator.TempJob);
            NativeArray<float3> scaes = new NativeArray<float3>(activeCount, Allocator.TempJob);
            NativeArray<float4> clres = new NativeArray<float4>(activeCount, Allocator.TempJob);

            NativeArray<AnchorPresets> prefabAnchorses = new NativeArray<AnchorPresets>(activeCount, Allocator.TempJob);
            NativeArray<SpriteType> prefabSprites = new NativeArray<SpriteType>(activeCount, Allocator.TempJob);
            NativeArray<float3> positionPlayers = new NativeArray<float3>(playersCount, Allocator.TempJob);
            NativeArray<bool> collidedPlayers = new NativeArray<bool>(playersCount, Allocator.TempJob);

            int counterPos = 0, counterRot = 0, counterSca = 0, counterClr = 0;
            for (int i = 0; i < activeCount; i++)
            {
                var prefab = prefabs[activePrefabs[i]];

                startFrames[i] = prefab.StartFrame;
                endFrames[i] = prefab.EndFrame;
                prefabActives[i] = prefab.Active;
                prefabColliders[i] = prefab.Collider;

                posCountArray[i] = counterPos;
                rotCountArray[i] = counterRot;
                scaCountArray[i] = counterSca;
                clrCountArray[i] = counterClr;

                posLengthArray[i] = prefab.Pos.Length;
                rotLengthArray[i] = prefab.Rot.Length;
                scaLengthArray[i] = prefab.Sca.Length;
                clrLengthArray[i] = prefab.Clr.Length;

                counterPos += posLengthArray[i];
                counterRot += rotLengthArray[i];
                counterSca += scaLengthArray[i];
                counterClr += clrLengthArray[i];

                prefabAnchorses[i] = prefab.Anchor;
                prefabSprites[i] = prefab.SpriteType;
            }

            NativeArray<Pos> prefabPoses = new NativeArray<Pos>(counterPos, Allocator.TempJob);
            NativeArray<Rot> prefabRotes = new NativeArray<Rot>(counterRot, Allocator.TempJob);
            NativeArray<Sca> prefabScaes = new NativeArray<Sca>(counterSca, Allocator.TempJob);
            NativeArray<Clr> prefabClres = new NativeArray<Clr>(counterClr, Allocator.TempJob);

            counterPos = 0; counterRot = 0; counterSca = 0; counterClr = 0;
            for (int x = 0; x < activeCount; x++)
            {
                var prefab = prefabs[activePrefabs[x]];

                for (int y = 0; y < posLengthArray[x]; y++)
                {
                    prefabPoses[counterPos] = prefab.Pos[y];
                    counterPos++;
                }
                for (int y = 0; y < rotLengthArray[x]; y++)
                {
                    prefabRotes[counterRot] = prefab.Rot[y];
                    counterRot++;
                }
                for (int y = 0; y < scaLengthArray[x]; y++)
                {
                    prefabScaes[counterSca] = prefab.Sca[y];
                    counterSca++;
                }
                for (int y = 0; y < clrLengthArray[x]; y++)
                {
                    prefabClres[counterClr] = prefab.Clr[y];
                    counterClr++;
                }
            }

            for (int i = 0; i < playersCount; i++)
            {
                var pos = playerPositions[i].Invoke();
                positionPlayers[i] = new float3(pos.x, pos.y, 0);
                collidedPlayers[i] = false;
            }

            MainCalculateParallel mainSimpleParallelJob = new MainCalculateParallel
            {
                seed = seed,
                activeFrame = activeFrame,
                playersCount = playersCount,
                countActivePrefabs = activeCount,
                borderScreen = borderScreen,

                anchorArray = prefabAnchorses,
                colliderArray = prefabColliders,
                spriteArray = prefabSprites,

                posArray = prefabPoses,
                rotArray = prefabRotes,
                scaArray = prefabScaes,
                clrArray = prefabClres,
                posCountArray = posCountArray,
                rotCountArray = rotCountArray,
                scaCountArray = scaCountArray,
                clrCountArray = clrCountArray,
                posLengthArray = posLengthArray,
                rotLengthArray = rotLengthArray,
                scaLengthArray = scaLengthArray,
                clrLengthArray = clrLengthArray,

                poses = poses,
                rotes = rotes,
                scaes = scaes,
                clres = clres,

                positionPlayers = positionPlayers,
                collidedPlayers = collidedPlayers
            };
            JobHandle mainSimpleJobHandle = mainSimpleParallelJob.Schedule(activeCount, 50);
            mainSimpleJobHandle.Complete();

            for (int i = 0; i < playersCount; i++)
                if (collidedPlayers[i])
                    playerDamages[i].Invoke(1);

            for (int i = 0; i < activeCount; i++)
            {
                trs[i].localPosition = poses[i];
                trs[i].localEulerAngles = rotes[i];
                trs[i].localScale = scaes[i];
                Vector4 color = clres[i];
                srs[i].color = color;
                srs[i].sprite = GameData.GetSprite(prefabSprites[i]);
            }

            startFrames.Dispose();
            endFrames.Dispose();
            prefabActives.Dispose();
            prefabColliders.Dispose();

            prefabPoses.Dispose();
            prefabRotes.Dispose();
            prefabScaes.Dispose();
            prefabClres.Dispose();
            posCountArray.Dispose();
            rotCountArray.Dispose();
            scaCountArray.Dispose();
            clrCountArray.Dispose();
            posLengthArray.Dispose();
            rotLengthArray.Dispose();
            scaLengthArray.Dispose();
            clrLengthArray.Dispose();

            poses.Dispose();
            rotes.Dispose();
            scaes.Dispose();
            clres.Dispose();
            prefabAnchorses.Dispose();
            prefabSprites.Dispose();
            positionPlayers.Dispose();
            collidedPlayers.Dispose();
        }
    }
}