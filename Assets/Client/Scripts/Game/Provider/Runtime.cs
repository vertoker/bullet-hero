using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Game.Components;
using UnityEngine;
using System.Linq;
using Audio.Game;
using Game.Core;
using Data;

using UtilsStatic = Utils.Static;

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
        private AudioPlayer audioPlayer;

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

        private PositionDelegate playerPosition;
        private HealthDelegate playerDamage;
        private Player player;

        private const int CountBlocks = 1000;

        private int maxFrame = 0;
        private float levelLength = 0;
        [SerializeField] private float timer = 0f;
        [SerializeField] private int activeFrame = 0;
        private float timeScale = 1;
        private float timePause = 1;
        private bool activeUpdater = false;
        private Coroutine updater;

        public float Length => UtilsStatic.Frame2Sec(maxFrame);
        public bool IsPlay => timePause == 1;
        public bool IsActive => activeUpdater;
        public float Timer => timer;

        private static UnityEvent<int, int> frameEvent = new UnityEvent<int, int>();
        private static UnityEvent<bool> pauseEvent = new UnityEvent<bool>();

        public static event UnityAction<int, int> UpdateFrame
        {
            add => frameEvent.AddListener(value);
            remove => frameEvent.AddListener(value);
        }
        public static event UnityAction<bool> UpdatePause
        {
            add => pauseEvent.AddListener(value);
            remove => pauseEvent.AddListener(value);
        }

        private void Awake()
        {
            self = transform;
            trs = new Transform[CountBlocks];
            srs = new SpriteRenderer[CountBlocks];
            audioPlayer = GetComponent<AudioPlayer>();

            for (int i = 0; i < CountBlocks; i++)
            {
                trs[i] = self.GetChild(i).GetComponent<Transform>();
                srs[i] = self.GetChild(i).GetComponent<SpriteRenderer>();
            }

            float aspect = Screen.width / Screen.height;
            borderScreen = new float3(aspect * 10, 10, 0);
        }

        public void LoadLevel(Level level, Player player, GameRules rules)
        {
            levelLength = level.LevelData.Length;
            maxFrame = UtilsStatic.Sec2Frame(levelLength);
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

            timeScale = rules.time;

            if (this.player != null)
                player.DeathCaller -= StopGame;
            this.player = player;
            playerPosition = player.GetPositionDelegate;
            playerDamage = player.GetDamageDelegate;
            player.DeathCaller += StopGame;

            audioPlayer.SetAudio(level.LevelData, rules);

            StartGame();
        }
        private void OnDisable()
        {
            player.DeathCaller -= StopGame;
        }

        private IEnumerator Updater()
        {
            while (true)
            {
                yield return null;
                timer += Time.deltaTime * timeScale * timePause;
                int lastFrame = activeFrame;
                activeFrame = UtilsStatic.Sec2Frame(timer);
                if (activeFrame == lastFrame)
                    continue;
                frameEvent.Invoke(activeFrame, maxFrame);
                UpdateCycle();
                if (activeFrame == maxFrame)
                {
                    activeUpdater = false;
                    Pause();
                }
            }
        }
        public void TogglePlayPause()
        {
            if (timePause == 1)
                Pause();
            else if (activeFrame == maxFrame)
                Restart();
            else
                PlaySafe();
        }
        public void StartGame()
        {
            activeUpdater = true;
            updater = StartCoroutine(Updater());
            PlaySafe();
        }
        public void PlaySafe()
        {
            if (activeFrame != maxFrame)
            {
                Play();
            }
        }
        public void Play()
        {
            timePause = 1;
            audioPlayer.Drag(timer);
            audioPlayer.Play();
            pauseEvent.Invoke(true);
        }
        public void StopGame()
        {
            Pause();
            StopCoroutine(updater);
            activeUpdater = false;
        }
        public void Pause()
        {
            timePause = 0;
            audioPlayer.Pause();
            pauseEvent.Invoke(false);
        }
        public void Restart()
        {
            Pause();
            StopGame();
            timer = 0;
            activeFrame = 0;
            StartGame();
            PlaySafe();
        }
        public void SetSec(float sec)
        {
            if (sec < 0)
                timer = 0;
            else if (sec > levelLength)
                timer = levelLength;
            else
                timer = sec;
            audioPlayer.Drag(timer);
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
            NativeArray<float3> positionPlayer = new NativeArray<float3>(1, Allocator.TempJob);
            NativeArray<bool> collidedPlayer = new NativeArray<bool>(1, Allocator.TempJob);

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

                srs[i].sortingOrder = prefab.Layer;
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

            collidedPlayer[0] = false;
            var pos = playerPosition.Invoke();
            positionPlayer[0] = new float3(pos.x, pos.y, 0);

            MainCalculateParallel mainSimpleParallelJob = new MainCalculateParallel
            {
                seed = seed, activeFrame = activeFrame,
                countActivePrefabs = activeCount,
                borderScreen = borderScreen,
                anchorArray = prefabAnchorses,

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
                clres = clres
            };
            JobHandle mainSimpleJobHandle = mainSimpleParallelJob.Schedule(activeCount, 50);
            mainSimpleJobHandle.Complete();

            for (int i = 0; i < activeCount; i++)
            {
                trs[i].localPosition = poses[i];
                trs[i].localEulerAngles = rotes[i];
                trs[i].localScale = scaes[i];
                Vector4 color = clres[i];
                srs[i].color = color;
                srs[i].sprite = GameData.GetSprite(prefabSprites[i]);
            }

            for (int i = 0; i < activeCount; i++)
            {
                poses[i] = trs[i].position;
                rotes[i] = trs[i].eulerAngles;
                scaes[i] = trs[i].lossyScale;
            }

            CollisionDetectionParallel collisionDetectionParallelJob = new CollisionDetectionParallel
            {
                countActivePrefabs = activeCount,
                positionPlayers = positionPlayer,
                collidedPlayers = collidedPlayer,
                colliderArray = prefabColliders,
                spriteArray = prefabSprites,

                poses = poses,
                rotes = rotes,
                scaes = scaes
            };
            JobHandle collisionDetectionJobHandle = collisionDetectionParallelJob.Schedule();
            collisionDetectionJobHandle.Complete();

            if (collidedPlayer[0])
                playerDamage.Invoke(1);

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
            positionPlayer.Dispose();
            collidedPlayer.Dispose();
        }
    }
}