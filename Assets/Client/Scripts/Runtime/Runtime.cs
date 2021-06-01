using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class Runtime : MonoBehaviour
{
    public Transform player;
    public Sprite[] sprites;
    private Transform trParent;
    private List<Transform> trList = new List<Transform>();
    private List<SpriteRenderer> srList = new List<SpriteRenderer>();

    [Space]

    [SerializeField] private float timer = 0f;
    [SerializeField] private int activeFrame = 0;
    [SerializeField] private float timerFrame = 0f;

    private const int CountBlocks = 1000;
    private BlockJobTools tool;

    //Level params
    [SerializeField] private int countOfPrefabs = 0;
    [SerializeField] private int countOfEnableBlocks = 0;

    private List<int> startFramesList = new List<int>();
    private List<int> endFramesList = new List<int>();

    public float Timer { get => timer; set => timer = value; }

    private void Awake()
    {
        trParent = transform;
        for (int i = 0; i < CountBlocks; i++)
        {
            trList.Add(trParent.GetChild(i));
            srList.Add(trParent.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }

    public void LaunchLevel(float aspectRatio)
    {
        tool = new BlockJobTools(new float3(9 * aspectRatio, 9, 0), 1337);

        countOfPrefabs = LevelManager.level.Prefabs.Count;
        for (int i = 0; i < countOfPrefabs; i++)
        {
            startFramesList.Add(LevelManager.level.Prefabs[i].StartFrame);
            endFramesList.Add(LevelManager.level.Prefabs[i].EndFrame);
        }
    }

    private void Update()
    {
        #region Базовый расчёт кадра
        timer += Time.deltaTime;
        int lastFrame = activeFrame;
        activeFrame = Mathf.FloorToInt(timer * 60f);
        timerFrame = activeFrame / 60f;

        if (activeFrame == lastFrame)
            return;
        #endregion

        #region Сортировка по времени, определяет какие объекты сейчас должны быть на сцене
        NativeArray<int> startFrames = new NativeArray<int>(countOfPrefabs, Allocator.TempJob);
        NativeArray<int> endFrames = new NativeArray<int>(countOfPrefabs, Allocator.TempJob);

        NativeArray<int> activeBlocks = new NativeArray<int>(CountBlocks, Allocator.TempJob);
        NativeArray<int> counter = new NativeArray<int>(1, Allocator.TempJob);

        for (int i = 0; i < countOfPrefabs; i++)
        {
            startFrames[i] = startFramesList[i];
            endFrames[i] = endFramesList[i];
        }
        for (int i = 0; i < CountBlocks; i++)
            activeBlocks[i] = 0;
        counter[0] = 0;

        FindActiveBlocksParallel findActiveBlocksParallelJob = new FindActiveBlocksParallel
        {
            startFrames = startFrames,
            endFrames = endFrames,
            activeBlocks = activeBlocks,

            activeFrame = activeFrame,
            counter = counter
        };
        JobHandle sort = findActiveBlocksParallelJob.Schedule(countOfPrefabs, 50);
        sort.Complete();

        // Удаление и восстановление лишних спрайтов
        for (int i = 0; i < countOfEnableBlocks; i++)
            srList[i].sprite = null;
        countOfEnableBlocks = counter[0];
        #endregion

        #region Сортировка на parent и child объекты
        NativeArray<bool> hasChilds = new NativeArray<bool>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> IDPAllArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);// All IDParent array
        NativeArray<int> IDAllArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);// All ID array
        NativeList<int> ids = new NativeList<int>(Allocator.TempJob);

        for (int i = 0; i < countOfEnableBlocks; i++)
        {
            int id = activeBlocks[i];
            IDPAllArray[i] = LevelManager.level.Prefabs[id].ParentID;
            IDAllArray[i] = LevelManager.level.Prefabs[id].ID;
        }

        SortBlocksParallel sortBlocksParallelJob = new SortBlocksParallel()
        {
            countOfEnableObjects = countOfEnableBlocks,
            IDPAllArray = IDPAllArray,
            IDAllArray = IDAllArray,
            hasChilds = hasChilds,
            ids = ids
        };
        JobHandle sortJobHandle = sortBlocksParallelJob.Schedule();
        sortJobHandle.Complete();

        NativeList<int> sortBlocks = new NativeList<int>(Allocator.TempJob);
        NativeList<int> parentBlocks = new NativeList<int>(Allocator.TempJob);
        NativeList<int> simpleBlocks = new NativeList<int>(Allocator.TempJob);
        int countOfParentBlocks = 0;
        int countOfSimpleBlocks = 0;

        for (int i = 0; i < countOfEnableBlocks; i++)
        {
            int id = activeBlocks[i];
            if (hasChilds[id])
            {
                countOfParentBlocks++;
                parentBlocks.Add(id);
            }
            else
            {
                countOfSimpleBlocks++;
                simpleBlocks.Add(id);
            }
        }
        sortBlocks.AddRange(parentBlocks);
        sortBlocks.AddRange(simpleBlocks);
        Debug.Log(countOfEnableBlocks + " " + parentBlocks.Length + " " + simpleBlocks.Length);

        parentBlocks.Dispose();
        simpleBlocks.Dispose();
        activeBlocks.Dispose();
        startFrames.Dispose();
        endFrames.Dispose();
        counter.Dispose();

        hasChilds.Dispose();
        IDPAllArray.Dispose();
        IDAllArray.Dispose();
        ids.Dispose();
        #endregion

        /// Основное тело реализации объектов

        #region Основные массивы и листы для всех объектов
        NativeList<Pos> posList = new NativeList<Pos>(Allocator.TempJob);
        NativeList<Rot> rotList = new NativeList<Rot>(Allocator.TempJob);
        NativeList<Sca> scaList = new NativeList<Sca>(Allocator.TempJob);
        NativeList<Clr> clrList = new NativeList<Clr>(Allocator.TempJob);

        NativeArray<float3> poses = new NativeArray<float3>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<float3> rotes = new NativeArray<float3>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<float3> scaes = new NativeArray<float3>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<float4> clres = new NativeArray<float4>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> posCountArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> rotCountArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> scaCountArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> clrCountArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> posLengthArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> rotLengthArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> scaLengthArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> clrLengthArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);

        NativeArray<int> spriteArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<bool> colliderArray = new NativeArray<bool>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<AnchorPresets> anchorArray = new NativeArray<AnchorPresets>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> layerArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> IDPArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeArray<int> IDArray = new NativeArray<int>(countOfEnableBlocks, Allocator.TempJob);
        NativeList<int> idsParent = new NativeList<int>(Allocator.TempJob);
        #endregion

        #region Заполнение массивов и листов для всех объектов
        int counterPos = 0, counterRot = 0, counterSca = 0, counterClr = 0;
        for (int i = 0; i < countOfEnableBlocks; i++)
        {
            int id = sortBlocks[i];

            posCountArray[i] = counterPos;
            rotCountArray[i] = counterRot;
            scaCountArray[i] = counterSca;
            clrCountArray[i] = counterClr;

            posLengthArray[i] = LevelManager.level.Prefabs[id].Pos.Count;
            rotLengthArray[i] = LevelManager.level.Prefabs[id].Rot.Count;
            scaLengthArray[i] = LevelManager.level.Prefabs[id].Sca.Count;
            clrLengthArray[i] = LevelManager.level.Prefabs[id].Clr.Count;

            counterPos += posLengthArray[i];
            counterRot += rotLengthArray[i];
            counterSca += scaLengthArray[i];
            counterClr += clrLengthArray[i];

            for (int j = 0; j < LevelManager.level.Prefabs[id].Pos.Count; j++)
                posList.Add(LevelManager.level.Prefabs[id].Pos[j]);
            for (int j = 0; j < LevelManager.level.Prefabs[id].Rot.Count; j++)
                rotList.Add(LevelManager.level.Prefabs[id].Rot[j]);
            for (int j = 0; j < LevelManager.level.Prefabs[id].Sca.Count; j++)
                scaList.Add(LevelManager.level.Prefabs[id].Sca[j]);
            for (int j = 0; j < LevelManager.level.Prefabs[id].Clr.Count; j++)
                clrList.Add(LevelManager.level.Prefabs[id].Clr[j]);

            colliderArray[i] = LevelManager.level.Prefabs[id].Collider;
            anchorArray[i] = LevelManager.level.Prefabs[id].Anchor;
            spriteArray[i] = (int)LevelManager.level.Prefabs[id].SpriteType;
            layerArray[i] = LevelManager.level.Prefabs[id].Layer;
            IDPArray[i] = LevelManager.level.Prefabs[id].ParentID;
            IDArray[i] = LevelManager.level.Prefabs[id].ID;
        }
        #endregion
        
        #region Выполнение работы по высчитыванию кадра для parent объектов
        MainJobParentParallelBlock mainParentParallelJob = new MainJobParentParallelBlock
        {
            tool = tool,
            timer = timerFrame,
            playerPos = player.position,

            posArray = posList.AsArray(),
            rotArray = rotList.AsArray(),
            scaArray = scaList.AsArray(),
            clrArray = clrList.AsArray(),

            posCountArray = posCountArray,
            rotCountArray = rotCountArray,
            scaCountArray = scaCountArray,
            clrCountArray = clrCountArray,

            posLengthArray = posLengthArray,
            rotLengthArray = rotLengthArray,
            scaLengthArray = scaLengthArray,
            clrLengthArray = clrLengthArray,

            IDArray = IDArray,
            IDPArray = IDPArray,
            idsParent = idsParent,
            anchorArray = anchorArray,
            spriteArray = spriteArray,
            colliderArray = colliderArray,
            countOfParentBlocks = countOfParentBlocks,

            poses = poses,
            rotes = rotes,
            scaes = scaes,
            clres = clres
        };
        JobHandle mainParentJobHandle = mainParentParallelJob.Schedule();
        mainParentJobHandle.Complete();
        #endregion

        #region Выполнение работы по высчитыванию кадра для простых объектов
        MainJobSimpleParallelBlock mainSimpleParallelJob = new MainJobSimpleParallelBlock
        {
            tool = tool, timer = timerFrame,
            playerPos = player.position,
            countOfSimpleBlocks = countOfSimpleBlocks,
            countOfParentBlocks = countOfParentBlocks,
            countOfEnableBlocks = countOfEnableBlocks,

            posArray = posList.AsArray(),
            rotArray = rotList.AsArray(),
            scaArray = scaList.AsArray(),
            clrArray = clrList.AsArray(),

            posCountArray = posCountArray,
            rotCountArray = rotCountArray,
            scaCountArray = scaCountArray,
            clrCountArray = clrCountArray,

            posLengthArray = posLengthArray,
            rotLengthArray = rotLengthArray,
            scaLengthArray = scaLengthArray,
            clrLengthArray = clrLengthArray,

            anchorArray = anchorArray,
            colliderArray = colliderArray,
            spriteArray = spriteArray,
            IDPArray = IDPArray,
            IDArray = IDArray,

            poses = poses,
            rotes = rotes,
            scaes = scaes,
            clres = clres
        };
        JobHandle mainSimpleJobHandle = mainSimpleParallelJob.Schedule();
        mainSimpleJobHandle.Complete();
        #endregion

        #region Установка значений для всех объектов
        for (int i = 0; i < countOfEnableBlocks; i++)
        {
            trList[i].localPosition = poses[i];
            trList[i].localEulerAngles = rotes[i];
            trList[i].localScale = scaes[i];

            if (spriteArray[i] != 0)
            {
                srList[i].color = new Color(clres[i].x, clres[i].y, clres[i].z, clres[i].w);
                srList[i].sprite = sprites[spriteArray[i]];
                srList[i].sortingOrder = layerArray[i];
            }
        }
        #endregion

        #region Расформировывание всех объектов
        sortBlocks.Dispose();
        colliderArray.Dispose();
        anchorArray.Dispose();
        spriteArray.Dispose();
        layerArray.Dispose();
        idsParent.Dispose();

        IDPArray.Dispose();
        IDArray.Dispose();

        posCountArray.Dispose();
        rotCountArray.Dispose();
        scaCountArray.Dispose();
        clrCountArray.Dispose();
        posLengthArray.Dispose();
        rotLengthArray.Dispose();
        scaLengthArray.Dispose();
        clrLengthArray.Dispose();
        posList.Dispose();
        rotList.Dispose();
        scaList.Dispose();
        clrList.Dispose();
        poses.Dispose();
        rotes.Dispose();
        scaes.Dispose();
        clres.Dispose();
        #endregion
    }
}

[BurstCompile]
public struct FindActiveBlocksParallel : IJobParallelFor
{
    [ReadOnly] public int activeFrame;
    public NativeArray<int> counter;
    [ReadOnly] public NativeArray<int> startFrames;
    [ReadOnly] public NativeArray<int> endFrames;
    public NativeArray<int> activeBlocks;

    public void Execute(int index)
    {
        if (activeFrame >= startFrames[index] && activeFrame <= endFrames[index])
        {
            activeBlocks[counter[0]] = index;
            counter[0]++;
        }
    }
}

[BurstCompile]
public struct SortBlocksParallel : IJob
{
    [ReadOnly] public NativeArray<int> IDPAllArray;
    [ReadOnly] public NativeArray<int> IDAllArray;
    [ReadOnly] public int countOfEnableObjects;
    public NativeArray<bool> hasChilds;
    public NativeList<int> ids;

    public void Execute()
    {
        for (int index = 0; index < countOfEnableObjects; index++)
        {
            if (IDPAllArray[index] != 0)// Если у объекта есть родитель
                ids.Add(IDPAllArray[index]);// Добавляет в список проверяемых объектов
        }
        for (int index = 0; index < countOfEnableObjects; index++)
        {
            // Если объект (из общей массы) находиться в списке проверяемых объектов
            // у объекта (из общей массы) есть наследники (childs)
            hasChilds[index] = ids.Contains(IDAllArray[index]);
        }
    }
}

[BurstCompile]
public struct MainJobParentParallelBlock : IJob
{
    public NativeArray<bool> colliderArray;
    public NativeArray<AnchorPresets> anchorArray;
    public NativeArray<int> IDPArray, IDArray, spriteArray;

    [ReadOnly] public NativeArray<Pos> posArray;
    [ReadOnly] public NativeArray<Rot> rotArray;
    [ReadOnly] public NativeArray<Sca> scaArray;
    [ReadOnly] public NativeArray<Clr> clrArray;
    [ReadOnly] public NativeArray<int> posCountArray;
    [ReadOnly] public NativeArray<int> rotCountArray;
    [ReadOnly] public NativeArray<int> scaCountArray;
    [ReadOnly] public NativeArray<int> clrCountArray;
    [ReadOnly] public NativeArray<int> posLengthArray;
    [ReadOnly] public NativeArray<int> rotLengthArray;
    [ReadOnly] public NativeArray<int> scaLengthArray;
    [ReadOnly] public NativeArray<int> clrLengthArray;
    [ReadOnly] public int countOfParentBlocks;

    public NativeList<int> idsParent;
    public NativeArray<float3> poses;
    public NativeArray<float3> rotes;
    public NativeArray<float3> scaes;
    public NativeArray<float4> clres;

    public BlockJobTools tool;
    [ReadOnly] public float timer;
    [ReadOnly] public float3 playerPos;

    public void Execute()
    {
        int objectCounter = 0;
        DebugPrint(scaes.ToArray());
        for (int index = 0; index < countOfParentBlocks; index++)// Проходит по всем объектам
        {
            if (IDPArray[index] == 0)// Если у объекта нет родителя
            {
                clres[index] = tool.GetClr(clrArray.GetSubArray(clrCountArray[index], clrLengthArray[index]), timer);
                rotes[index] = tool.GetRot(rotArray.GetSubArray(rotCountArray[index], rotLengthArray[index]), timer);
                scaes[index] = tool.GetSca(scaArray.GetSubArray(scaCountArray[index], scaLengthArray[index]), timer);
                float3 localPivot = tool.CalculatePivot(rotes[index].z, scaes[index], anchorArray[index]);
                poses[index] = tool.GetPos(posArray.GetSubArray(posCountArray[index], posLengthArray[index]), timer, localPivot, anchorArray[index]);

                if (colliderArray[index])
                {
                    if (tool.CollisionDetection(poses[index], rotes[index], scaes[index], playerPos, spriteArray[index]))
                        Player.Instance.Damage();
                }

                idsParent.Add(IDArray[index]);// Добавляет id объекта в базу проверенных
                objectCounter++;
            }
        }
        while (objectCounter < countOfParentBlocks)// Пока все объекты не будут проверены
        {
            DebugPrint(scaes.ToArray());
            bool parentFinded = false;// Если родитель был найден в цикле ниже
            for (int index = 0; index < countOfParentBlocks; index++)// Проходит по всем объектам
            {
                // Если объект родитель есть в записанных объектах и самого объекта нету в записанных
                if (idsParent.Contains(IDPArray[index]) && !idsParent.Contains(IDArray[index]))
                {
                    int idParent = idsParent.IndexOf(IDPArray[index]);// Находиться индекс родительского объекта с помощью другого индекса
                    clres[index] = tool.GetClr(clrArray.GetSubArray(clrCountArray[index], clrLengthArray[index]), timer);
                    rotes[index] = tool.GetRotChild(rotArray.GetSubArray(rotCountArray[index], rotLengthArray[index]), timer, rotes[idParent]);
                    scaes[index] = tool.GetScaChild(scaArray.GetSubArray(scaCountArray[index], scaLengthArray[index]), timer, scaes[idParent]);
                    float3 localPivot = tool.CalculatePivot(rotes[index].z, scaes[index], anchorArray[index]);
                    float3 global = tool.CalculatePivot(rotes[idParent].z, scaes[idParent], anchorArray[idParent]);
                    poses[index] = tool.GetPosChild(posArray.GetSubArray(posCountArray[index], posLengthArray[index]), timer, localPivot, poses[idParent], global, anchorArray[index]);

                    if (colliderArray[index])
                    {
                        if (tool.CollisionDetection(poses[index], rotes[index], scaes[index], playerPos, spriteArray[index]))
                            Player.Instance.Damage();
                    }

                    idsParent.Add(IDArray[index]);// Добавляет id объекта в базу проверенных
                    objectCounter++;
                    parentFinded = true;
                }
            }
            if (!parentFinded)// Если у оставшихся объектов не было найдено родителей
            {
                for (int index = 0; index < countOfParentBlocks; index++)// Проходит по всем объектам
                {
                    // Если объекта нету в записанных объектах
                    if (!idsParent.Contains(IDArray[index]))
                    {
                        clres[index] = tool.GetClr(clrArray.GetSubArray(clrCountArray[index], clrLengthArray[index]), timer);
                        rotes[index] = tool.GetRot(rotArray.GetSubArray(rotCountArray[index], rotLengthArray[index]), timer);
                        scaes[index] = tool.GetSca(scaArray.GetSubArray(scaCountArray[index], scaLengthArray[index]), timer);
                        float3 localPivot = tool.CalculatePivot(rotes[index].z, scaes[index], anchorArray[index]);
                        poses[index] = tool.GetPos(posArray.GetSubArray(posCountArray[index], posLengthArray[index]), timer, localPivot, anchorArray[index]);

                        if (colliderArray[index])
                        {
                            if (tool.CollisionDetection(poses[index], rotes[index], scaes[index], playerPos, spriteArray[index]))
                                Player.Instance.Damage();
                        }

                        idsParent.Add(IDArray[index]);// Добавляет id объекта в базу проверенных
                        objectCounter++;
                    }
                }
            }
        }
        DebugPrint(scaes.ToArray());
    }
    static void DebugPrint<T>(T[] array)
    {
        string end = string.Empty;
        for (int i = 0; i < array.Length; i++)
            end += array[i].ToString() + ' ';
        Debug.Log(end);
    }
}

[BurstCompile]
public struct MainJobSimpleParallelBlock : IJob
{
    [ReadOnly] public NativeArray<bool> colliderArray;
    [ReadOnly] public NativeArray<AnchorPresets> anchorArray;
    [ReadOnly] public NativeArray<int> IDPArray, IDArray, spriteArray;

    [ReadOnly] public NativeArray<Pos> posArray;
    [ReadOnly] public NativeArray<Rot> rotArray;
    [ReadOnly] public NativeArray<Sca> scaArray;
    [ReadOnly] public NativeArray<Clr> clrArray;
    [ReadOnly] public NativeArray<int> posCountArray;
    [ReadOnly] public NativeArray<int> rotCountArray;
    [ReadOnly] public NativeArray<int> scaCountArray;
    [ReadOnly] public NativeArray<int> clrCountArray;
    [ReadOnly] public NativeArray<int> posLengthArray;
    [ReadOnly] public NativeArray<int> rotLengthArray;
    [ReadOnly] public NativeArray<int> scaLengthArray;
    [ReadOnly] public NativeArray<int> clrLengthArray;

    [ReadOnly] public int countOfParentBlocks;
    [ReadOnly] public int countOfSimpleBlocks;
    [ReadOnly] public int countOfEnableBlocks;

    public NativeArray<float3> poses;
    public NativeArray<float3> rotes;
    public NativeArray<float3> scaes;
    public NativeArray<float4> clres;

    public BlockJobTools tool;
    [ReadOnly] public float timer;
    [ReadOnly] public float3 playerPos;

    public void Execute()
    {
        for (int id = countOfParentBlocks; id < countOfEnableBlocks; id++)// Конструкция специально для перебора simple объектов
        {
            int pid = -1;// Сюда записывается id родительского объекта
            for (int j = 0; j < countOfParentBlocks; j++)// Перебор среди всех parent объектов
            {
                if (IDPArray[id] == IDArray[j])// Если pid простого объекта = перебираемого id родительского объекта
                {
                    pid = j;
                    break;
                }
            }
            if (pid == -1)// Если родитель не был найден
            {
                clres[id] = tool.GetClr(clrArray.GetSubArray(clrCountArray[id], clrLengthArray[id]), timer);
                rotes[id] = tool.GetRot(rotArray.GetSubArray(rotCountArray[id], rotLengthArray[id]), timer);
                scaes[id] = tool.GetSca(scaArray.GetSubArray(scaCountArray[id], scaLengthArray[id]), timer);
                float3 localPivot = tool.CalculatePivot(rotes[id].z, scaes[id], anchorArray[id]);
                poses[id] = tool.GetPos(posArray.GetSubArray(posCountArray[id], posLengthArray[id]), timer, localPivot, anchorArray[id]);
            }
            else// Если родитель у объекта был найден
            {
                clres[id] = tool.GetClr(clrArray.GetSubArray(clrCountArray[id], clrLengthArray[id]), timer);
                rotes[id] = tool.GetRotChild(rotArray.GetSubArray(rotCountArray[id], rotLengthArray[id]), timer, rotes[pid]);
                scaes[id] = tool.GetScaChild(scaArray.GetSubArray(scaCountArray[id], scaLengthArray[id]), timer, scaes[pid]);
                //Debug.Log(id + " " + pid + " " + scaes[id] + " " + scaes[pid]);
                float3 localPivot = tool.CalculatePivot(rotes[id].z, scaes[id], anchorArray[id]);
                float3 global = tool.CalculatePivot(rotes[pid].z, scaes[pid], anchorArray[pid]);
                poses[id] = tool.GetPosChild(posArray.GetSubArray(posCountArray[id], posLengthArray[id]), timer, localPivot, poses[pid], global, anchorArray[id]);
            }
            if (colliderArray[id])
            {
                if (tool.CollisionDetection(poses[id], rotes[id], scaes[id], playerPos, spriteArray[id]))
                    Player.Instance.Damage();
            }
        }
    }
    static void DebugPrint<T>(T[] array)
    {
        string end = string.Empty;
        for (int i = 0; i < array.Length; i++)
            end += array[i].ToString() + ' ';
        Debug.Log(end);
    }
}

public struct BlockJobTools
{
    public FastNoise noise;
    public float3 borderScreen;

    private const float Rad2Deg = 57.295779513f;
    private const float PlayerRadius = 0.25f;

    public BlockJobTools(float3 borderScreen, int seed = 1337)
    {
        noise = new FastNoise(seed);
        this.borderScreen = borderScreen;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Get
    public float3 GetPos(NativeArray<Pos> posMarkers, float timer, float3 local, AnchorPresets anchor)
    {
        if (posMarkers.Length == 1 || posMarkers[0].Time >= timer)
            return CalculatePos(posMarkers[0], local, anchor);
        else if (posMarkers[posMarkers.Length - 1].Time <= timer)
            return CalculatePos(posMarkers[posMarkers.Length - 1], local, anchor);

        Pos startPos = posMarkers[0], endPos = posMarkers[0];
        for (int i = 0; i < posMarkers.Length - 1; i++)
        {
            if (posMarkers[i].Time <= timer && posMarkers[i + 1].Time >= timer)
            {
                startPos = posMarkers[i];
                endPos = posMarkers[i + 1];
                break;
            }
        }

        float3 start = CalculatePos(startPos, local, anchor);
        float3 end = CalculatePos(endPos, local, anchor);
        float progress = (timer - startPos.Time) / (endPos.Time - startPos.Time);
        return start + (end - start) * GetEasing(progress, endPos.Easing);
    }
    public float3 GetRot(NativeArray<Rot> rotMarkers, float timer)
    {
        if (rotMarkers.Length == 1 || rotMarkers[0].Time >= timer)
            return new float3(0, 0, CalculateRot(rotMarkers[0]));
        else if (rotMarkers[rotMarkers.Length - 1].Time <= timer)
            return new float3(0, 0, CalculateRot(rotMarkers[rotMarkers.Length - 1]));

        Rot startRot = rotMarkers[0], endRot = rotMarkers[0];
        for (int i = 0; i < rotMarkers.Length - 1; i++)
        {
            if (rotMarkers[i].Time <= timer && rotMarkers[i + 1].Time >= timer)
            {
                startRot = rotMarkers[i];
                endRot = rotMarkers[i + 1];
                break;
            }
        }

        float start = CalculateRot(startRot);
        float end = CalculateRot(endRot);
        if (math.abs(end - start) > 180f)
            end += 360f;
        float progress = (timer - startRot.Time) / (endRot.Time - startRot.Time);
        return new float3(0, 0, start + (end - start) * GetEasing(progress, endRot.Easing));
    }
    public float3 GetSca(NativeArray<Sca> scaMarkers, float timer)
    {
        if (scaMarkers.Length == 1 || scaMarkers[0].Time >= timer)
            return CalculateSca(scaMarkers[0]);
        else if (scaMarkers[scaMarkers.Length - 1].Time <= timer)
            return CalculateSca(scaMarkers[scaMarkers.Length - 1]);

        Sca startSca = scaMarkers[0];
        Sca endSca = scaMarkers[0];

        for (int i = 0; i < scaMarkers.Length - 1; i++)
        {
            if (scaMarkers[i].Time <= timer && scaMarkers[i + 1].Time >= timer)
            {
                startSca = scaMarkers[i];
                endSca = scaMarkers[i + 1];
                break;
            }
        }

        float3 start = CalculateSca(startSca);
        float3 end = CalculateSca(endSca);
        float progress = (timer - startSca.Time) / (endSca.Time - startSca.Time);
        return start + (end - start) * GetEasing(progress, endSca.Easing);
    }
    public float4 GetClr(NativeArray<Clr> clrMarkers, float timer)
    {
        if (clrMarkers.Length == 1 || clrMarkers[0].Time >= timer)
            return CalculateClr(clrMarkers[0]);
        else if (clrMarkers[clrMarkers.Length - 1].Time <= timer)
            return CalculateClr(clrMarkers[clrMarkers.Length - 1]);

        Clr startClr = clrMarkers[0], endClr = clrMarkers[0];
        for (int i = 0; i < clrMarkers.Length - 1; i++)
        {
            if (clrMarkers[i].Time <= timer && clrMarkers[i + 1].Time >= timer)
            {
                startClr = clrMarkers[i];
                endClr = clrMarkers[i + 1];
                break;
            }
        }

        float4 start = CalculateClr(startClr);
        float4 end = CalculateClr(endClr);
        float progress = (timer - startClr.Time) / (endClr.Time - startClr.Time);
        return start + (end - start) * GetEasing(progress, endClr.Easing);
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Calculate
    public float3 CalculatePos(Pos pos, float3 local, AnchorPresets anchor)
    {
        float3 center = CenterAnchor(anchor);
        switch (pos.RandomType)
        {
            case VectorRandomType.N:
                return center + local + new float3(pos.SX, pos.SY, 0);
            case VectorRandomType.IMM:
                float xRandomIMM = noise.GetPerlin(pos.SX, pos.EX, pos.Time) / 2f + 0.5f;
                float yRandomIMM = noise.GetPerlin(pos.Time, pos.SY, pos.EY) / 2f + 0.5f;
                float xIMM = pos.SX + (pos.EX - pos.SX) * xRandomIMM;
                float yIMM = pos.SY + (pos.EY - pos.SY) * yRandomIMM;
                return center + local + new float3(xIMM, yIMM, 0);
            case VectorRandomType.MM:
                float xRandomMM = noise.GetPerlin(pos.SX, pos.EX, pos.Time) / 2f + 0.5f;
                float yRandomMM = noise.GetPerlin(pos.Time, pos.SY, pos.EY) / 2f + 0.5f;
                float xMM = Interval(pos.SX + (pos.EX - pos.SX) * xRandomMM, pos.SX, pos.EX, pos.Interval);
                float yMM = Interval(pos.SY + (pos.EY - pos.SY) * yRandomMM, pos.SY, pos.EY, pos.Interval);
                return center + local + new float3(xMM, yMM, 0);
            case VectorRandomType.C:
                float angleC = noise.GetPerlin(pos.EX, pos.Time, pos.EY);
                return center + local + RandomPointOnCircle(pos.SX, pos.SY, angleC, pos.Interval);
            case VectorRandomType.M:
                float multiplyRandomM = noise.GetPerlin(pos.EX, pos.Time, pos.EY) / 2f + 0.5f;
                return center + local + new float3(pos.SX, pos.SY, 0) * (pos.EX + (pos.EY - pos.EX) * multiplyRandomM);
        }
        return float3.zero;
    }
    public float CalculateRot(Rot rot)
    {
        switch (rot.RandomType)
        {
            case FloatRandomType.N:
                return rot.SA;
            case FloatRandomType.IMM:
                float randomIMM = noise.GetPerlin(rot.SA, rot.Time, rot.EA) / 2f + 0.5f;
                return rot.SA + (rot.EA - rot.SA) * randomIMM;
            case FloatRandomType.MM:
                float randomMM = noise.GetPerlin(rot.SA, rot.Time, rot.EA) / 2f + 0.5f;
                return Interval(rot.SA + (rot.EA - rot.SA) * randomMM, rot.SA, rot.EA, rot.Interval);
            case FloatRandomType.M:
                float multiplyRandomM = noise.GetPerlin(rot.EA, rot.Time, rot.Interval) / 2f + 0.5f;
                return rot.SA * (rot.EA + (rot.Interval - rot.EA) * multiplyRandomM);
        }
        return 0;
    }
    public float3 CalculateSca(Sca sca)
    {
        switch (sca.RandomType)
        {
            case VectorRandomType.N:
                return new float3(sca.SX, sca.SY, 0);
            case VectorRandomType.IMM:
                float xRandomIMM = noise.GetPerlin(sca.SX, sca.EX, sca.Time) / 2f + 0.5f;
                float yRandomIMM = noise.GetPerlin(sca.Time, sca.SY, sca.EY) / 2f + 0.5f;
                float xIMM = sca.SX + (sca.EX - sca.SX) * xRandomIMM;
                float yIMM = sca.SY + (sca.EY - sca.SY) * yRandomIMM;
                return new float3(xIMM, yIMM, 0);
            case VectorRandomType.MM:
                float xRandomMM = noise.GetPerlin(sca.SX, sca.EX, sca.Time) / 2f + 0.5f;
                float yRandomMM = noise.GetPerlin(sca.Time, sca.SY, sca.EY) / 2f + 0.5f;
                float xMM = Interval(sca.SX + (sca.EX - sca.SX) * xRandomMM, sca.SX, sca.EX, sca.Interval);
                float yMM = Interval(sca.SY + (sca.EY - sca.SY) * yRandomMM, sca.SY, sca.EY, sca.Interval);
                return new float3(xMM, yMM, 0);
            case VectorRandomType.C:
                float angleC = noise.GetPerlin(sca.EX, sca.Time, sca.EY);
                return RandomPointOnCircle(sca.SX, sca.SY, angleC, sca.Interval);
            case VectorRandomType.M:
                float multiplyRandomM = noise.GetPerlin(sca.EX, sca.Time, sca.EY) / 2f + 0.5f;
                return new float3(sca.SX, sca.SY, 0) * (sca.EX + (sca.EY - sca.EX) * multiplyRandomM);
        }
        return float3.zero;
    }
    public float4 CalculateClr(Clr clr)
    {
        switch (clr.RandomType)
        {
            case ColorRandomType.N:
                return new float4(clr.SR, clr.SG, clr.SB, clr.SA);
            case ColorRandomType.IMM:
                float rRandomIMM = noise.GetPerlin(clr.Time, clr.SR, clr.ER) / 2f + 0.5f;
                float gRandomIMM = noise.GetPerlin(clr.SG, clr.Time, clr.EG) / 2f + 0.5f;
                float bRandomIMM = noise.GetPerlin(clr.SB, clr.EB, clr.Time) / 2f + 0.5f;
                float aRandomIMM = noise.GetPerlin(clr.SA, -clr.Time, clr.EA) / 2f + 0.5f;
                float rIMM = clr.SR + (clr.ER - clr.SR) * rRandomIMM;
                float gIMM = clr.SG + (clr.EG - clr.SG) * gRandomIMM;
                float bIMM = clr.SB + (clr.EB - clr.SB) * bRandomIMM;
                float aIMM = clr.SA + (clr.EA - clr.SA) * aRandomIMM;
                return new float4(rIMM, gIMM, bIMM, aIMM);
            case ColorRandomType.MM:
                float rRandomMM = noise.GetPerlin(clr.Time, clr.SR, clr.ER) / 2f + 0.5f;
                float gRandomMM = noise.GetPerlin(clr.SG, clr.Time, clr.EG) / 2f + 0.5f;
                float bRandomMM = noise.GetPerlin(clr.SB, clr.EB, clr.Time) / 2f + 0.5f;
                float aRandomMM = noise.GetPerlin(clr.SA, -clr.Time, clr.EA) / 2f + 0.5f;
                float rMM = Interval(clr.SR + (clr.ER - clr.SR) * rRandomMM, clr.SR, clr.ER, clr.Interval);
                float gMM = Interval(clr.SG + (clr.EG - clr.SG) * gRandomMM, clr.SG, clr.EG, clr.Interval);
                float bMM = Interval(clr.SB + (clr.EB - clr.SB) * bRandomMM, clr.SB, clr.EB, clr.Interval);
                float aMM = Interval(clr.SA + (clr.EA - clr.SA) * aRandomMM, clr.SA, clr.EA, clr.Interval);
                return new float4(rMM, gMM, bMM, aMM);
        }
        return float4.zero;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Get (Child)
    public float3 GetPosChild(NativeArray<Pos> posMarkers, float timer, float3 local, float3 posParent, float3 global, AnchorPresets anchor)
    {
        if (posMarkers.Length == 1 || posMarkers[0].Time >= timer)
            return CalculatePosChild(posMarkers[0], local, posParent, global, anchor);
        else if (posMarkers[posMarkers.Length - 1].Time <= timer)
            return CalculatePosChild(posMarkers[posMarkers.Length - 1], local, posParent, global, anchor);

        Pos startPos = posMarkers[0], endPos = posMarkers[0];
        for (int i = 0; i < posMarkers.Length - 1; i++)
        {
            if (posMarkers[i].Time <= timer && posMarkers[i + 1].Time >= timer)
            {
                startPos = posMarkers[i];
                endPos = posMarkers[i + 1];
                break;
            }
        }

        float3 start = CalculatePosChild(startPos, local, posParent, global, anchor);
        float3 end = CalculatePosChild(endPos, local, posParent, global, anchor);
        float progress = (timer - startPos.Time) / (endPos.Time - startPos.Time);
        return start + (end - start) * GetEasing(progress, endPos.Easing);
    }
    public float3 GetRotChild(NativeArray<Rot> rotMarkers, float timer, float3 rotParent)
    {
        if (rotMarkers.Length == 1 || rotMarkers[0].Time >= timer)
            return new float3(0, 0, CalculateRotChild(rotMarkers[0], rotParent.z));
        else if (rotMarkers[rotMarkers.Length - 1].Time <= timer)
            return new float3(0, 0, CalculateRotChild(rotMarkers[rotMarkers.Length - 1], rotParent.z));

        Rot startRot = rotMarkers[0], endRot = rotMarkers[0];
        for (int i = 0; i < rotMarkers.Length - 1; i++)
        {
            if (rotMarkers[i].Time <= timer && rotMarkers[i + 1].Time >= timer)
            {
                startRot = rotMarkers[i];
                endRot = rotMarkers[i + 1];
                break;
            }
        }

        float start = CalculateRotChild(startRot, rotParent.z);
        float end = CalculateRotChild(endRot, rotParent.z);
        if (math.abs(end - start) > 180f)
            end += 360f;
        float progress = (timer - startRot.Time) / (endRot.Time - startRot.Time);
        return new float3(0, 0, start + (end - start) * GetEasing(progress, endRot.Easing));
    }
    public float3 GetScaChild(NativeArray<Sca> scaMarkers, float timer, float3 scaParent)
    {
        if (scaMarkers.Length == 1 || scaMarkers[0].Time >= timer)
            return CalculateScaChild(scaMarkers[0], scaParent);
        else if (scaMarkers[scaMarkers.Length - 1].Time <= timer)
            return CalculateScaChild(scaMarkers[scaMarkers.Length - 1], scaParent);

        Sca startSca = scaMarkers[0];
        Sca endSca = scaMarkers[0];

        for (int i = 0; i < scaMarkers.Length - 1; i++)
        {
            if (scaMarkers[i].Time <= timer && scaMarkers[i + 1].Time >= timer)
            {
                startSca = scaMarkers[i];
                endSca = scaMarkers[i + 1];
                break;
            }
        }

        float3 start = CalculateScaChild(startSca, scaParent);
        float3 end = CalculateScaChild(endSca, scaParent);
        float progress = (timer - startSca.Time) / (endSca.Time - startSca.Time);
        return start + (end - start) * GetEasing(progress, endSca.Easing);
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Calculate (Child)
    public float3 CalculatePosChild(Pos pos, float3 local, float3 posParent, float3 global, AnchorPresets anchor)
    {
        float3 center = CenterAnchor(anchor);
        float3 fullOffset = center + local + global + posParent;
        switch (pos.RandomType)
        {
            case VectorRandomType.N:
                return fullOffset + new float3(pos.SX, pos.SY, 0);
            case VectorRandomType.IMM:
                float xRandomIMM = noise.GetPerlin(pos.SX, pos.EX, pos.Time) / 2f + 0.5f;
                float yRandomIMM = noise.GetPerlin(pos.Time, pos.SY, pos.EY) / 2f + 0.5f;
                float xIMM = pos.SX + (pos.EX - pos.SX) * xRandomIMM;
                float yIMM = pos.SY + (pos.EY - pos.SY) * yRandomIMM;
                return fullOffset + new float3(xIMM, yIMM, 0);
            case VectorRandomType.MM:
                float xRandomMM = noise.GetPerlin(pos.SX, pos.EX, pos.Time) / 2f + 0.5f;
                float yRandomMM = noise.GetPerlin(pos.Time, pos.SY, pos.EY) / 2f + 0.5f;
                float xMM = Interval(pos.SX + (pos.EX - pos.SX) * xRandomMM, pos.SX, pos.EX, pos.Interval);
                float yMM = Interval(pos.SY + (pos.EY - pos.SY) * yRandomMM, pos.SY, pos.EY, pos.Interval);
                return fullOffset + new float3(xMM, yMM, 0);
            case VectorRandomType.C:
                float angleC = noise.GetPerlin(pos.EX, pos.Time, pos.EY);
                return fullOffset + RandomPointOnCircle(pos.SX, pos.SY, angleC, pos.Interval);
            case VectorRandomType.M:
                float multiplyRandomM = noise.GetPerlin(pos.EX, pos.Time, pos.EY) / 2f + 0.5f;
                return fullOffset + new float3(pos.SX, pos.SY, 0) * (pos.EX + (pos.EY - pos.EX) * multiplyRandomM);
        }
        return float3.zero;
    }
    public float CalculateRotChild(Rot rot, float rotParent)
    {
        switch (rot.RandomType)
        {
            case FloatRandomType.N:
                return rot.SA * rotParent;
            case FloatRandomType.IMM:
                float randomIMM = noise.GetPerlin(rot.SA, rot.Time, rot.EA) / 2f + 0.5f;
                return rot.SA + (rot.EA - rot.SA) * randomIMM * rotParent;
            case FloatRandomType.MM:
                float randomMM = noise.GetPerlin(rot.SA, rot.Time, rot.EA) / 2f + 0.5f;
                return Interval(rot.SA + (rot.EA - rot.SA) * randomMM, rot.SA, rot.EA, rot.Interval) * rotParent;
            case FloatRandomType.M:
                float multiplyRandomM = noise.GetPerlin(rot.EA, rot.Time, rot.Interval) / 2f + 0.5f;
                return rot.SA * (rot.EA + (rot.Interval - rot.EA) * multiplyRandomM) * rotParent;
        }
        return 0;
    }
    public float3 CalculateScaChild(Sca sca, float3 scaParent)
    {
        switch (sca.RandomType)
        {
            case VectorRandomType.N:
                return new float3(sca.SX, sca.SY, 0) * scaParent;
            case VectorRandomType.IMM:
                float xRandomIMM = noise.GetPerlin(sca.SX, sca.EX, sca.Time) / 2f + 0.5f;
                float yRandomIMM = noise.GetPerlin(sca.Time, sca.SY, sca.EY) / 2f + 0.5f;
                float xIMM = sca.SX + (sca.EX - sca.SX) * xRandomIMM;
                float yIMM = sca.SY + (sca.EY - sca.SY) * yRandomIMM;
                return new float3(xIMM, yIMM, 0) * scaParent;
            case VectorRandomType.MM:
                float xRandomMM = noise.GetPerlin(sca.SX, sca.EX, sca.Time) / 2f + 0.5f;
                float yRandomMM = noise.GetPerlin(sca.Time, sca.SY, sca.EY) / 2f + 0.5f;
                float xMM = Interval(sca.SX + (sca.EX - sca.SX) * xRandomMM, sca.SX, sca.EX, sca.Interval);
                float yMM = Interval(sca.SY + (sca.EY - sca.SY) * yRandomMM, sca.SY, sca.EY, sca.Interval);
                return new float3(xMM, yMM, 0) * scaParent;
            case VectorRandomType.C:
                float angleC = noise.GetPerlin(sca.EX, sca.Time, sca.EY);
                return RandomPointOnCircle(sca.SX, sca.SY, angleC, sca.Interval) * scaParent;
            case VectorRandomType.M:
                float multiplyRandomM = noise.GetPerlin(sca.EX, sca.Time, sca.EY) / 2f + 0.5f;
                return new float3(sca.SX, sca.SY, 0) * (sca.EX + (sca.EY - sca.EX) * multiplyRandomM) * scaParent;
        }
        return float3.zero;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Collision Detection
    public bool CollisionDetection(float3 pos, float3 rot, float3 sca, float3 player, int spriteType)
    {
        switch (spriteType)
        {
            case 1:// Square (rectangle)
                return CircleRectangle(pos, rot, sca, player);
            case 4:// Circle (ellipse)
                return CircleEllipse(pos, rot, sca, player);
            default:
                return false;
        }
    }

    public bool CircleRectangle(float3 pos, float3 rot, float3 sca, float3 player)
    {
        player = RotateVector(player - pos, -rot.z);

        float distanceX = math.abs(player.x);
        float distanceY = math.abs(player.y);

        if (distanceX > (sca.x / 2 + PlayerRadius)) { return false; }
        if (distanceY > (sca.y / 2 + PlayerRadius)) { return false; }
        if (distanceX <= (sca.x / 2)) { return true; }
        if (distanceY <= (sca.y / 2)) { return true; }

        float cDist_sqX = distanceX - sca.x / 2;
        float cDist_sqY = distanceY - sca.y / 2;
        float cDist_sq = cDist_sqX * cDist_sqX + cDist_sqY * cDist_sqY;

        return cDist_sq <= PlayerRadius * PlayerRadius;
    }

    public bool CircleEllipse(float3 pos, float3 rot, float3 sca, float3 player)
    {
        return false;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Tools
    private static Dictionary<AnchorPresets, float3> anchorCoefficient = new Dictionary<AnchorPresets, float3>()
    {
        { AnchorPresets.Left_Top, new float3(-1, 1, 0) }, { AnchorPresets.Center_Top, new float3(0, 1, 0) }, { AnchorPresets.Right_Top, new float3(1, 1, 0) },
        { AnchorPresets.Left_Middle, new float3(-1, 0, 0) }, { AnchorPresets.Center_Middle, new float3(0, 0, 0) }, { AnchorPresets.Right_Middle, new float3(1, 0, 0) },
        { AnchorPresets.Left_Bottom, new float3(-1, -1, 0) }, { AnchorPresets.Center_Bottom, new float3(0, -1, 0) }, { AnchorPresets.Right_Bottom, new float3(1, -1, 0) }
    };
    public float3 CenterAnchor(AnchorPresets anchor)
    {
        return borderScreen * anchorCoefficient[anchor];
    }

    public float3 RandomPointOnCircle(float x, float y, float angle, float radius)
    {
        angle -= 90f;
        float3 randomDirection = new float3(math.cos(angle / Rad2Deg), math.sin(angle / Rad2Deg), 0);
        return new float3(x, y, 0) + randomDirection * radius;
    }

    public float3 RotateVector(float3 a, float offsetAngle)//метод вращения объекта
    {
        float power = math.sqrt(a.x * a.x + a.y * a.y);//коэффициент силы
        float angle = math.atan2(a.y, a.x) * Rad2Deg + offsetAngle;//угол из координат с offset'ом
        return new float3(math.cos(angle / Rad2Deg), math.sin(angle / Rad2Deg), 0) * power;
        //построение вектора из изменённого угла с коэффициентом силы
    }

    public float Interval(float value, float min, float max, float interval)
    {
        value = math.floor(value / interval) * interval;
        return math.clamp(value, min, max);
    }

    public float3 CalculatePivot(float rot, float3 sca, AnchorPresets pivot)
    {
        sca *= anchorCoefficient[pivot] / -2f;
        float power = math.sqrt(sca.x * sca.x + sca.y * sca.y);
        float angle = math.atan2(sca.y, sca.x) * Rad2Deg + rot;
        return new float3(math.cos(angle / Rad2Deg), math.sin(angle / Rad2Deg), 0) * power;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Easings
    // Все реализации честно украдены с easings.net
    public float GetEasing(float x, EasingType easing)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        const float c3 = c1 + 1f;
        const float c4 = 2 * math.PI / 3;
        const float c5 = 2 * math.PI / 4.5f;

        switch (easing)
        {
            case EasingType.Linear:
                return x;
            case EasingType.Constant:
                return math.floor(x);

            case EasingType.InSine:
                return 1 - math.cos((x * math.PI) / 2);
            case EasingType.OutSine:
                return math.sin((x * math.PI) / 2);
            case EasingType.InOutSine:
                return -(math.cos(math.PI * x) - 1) / 2;

            case EasingType.InQuad:
                return x * x;
            case EasingType.OutQuad:
                return 1 - math.pow(1 - x, 2);
            case EasingType.InOutQuad:
                return x < 0.5f ? 2 * x * x : 1 - math.pow(-2 * x + 2, 2) / 2;

            case EasingType.InCubic:
                return x * x * x;
            case EasingType.OutCubic:
                return 1 - math.pow(1 - x, 3);
            case EasingType.InOutCubic:
                return x < 0.5f ? 4 * x * x * x : 1 - math.pow(-2 * x + 2, 3) / 2;

            case EasingType.InQuart:
                return x * x * x * x;
            case EasingType.OutQuart:
                return 1 - math.pow(1 - x, 4);
            case EasingType.InOutQuart:
                return x < 0.5f ? 8 * x * x * x * x : 1 - math.pow(-2 * x + 2, 4) / 2;

            case EasingType.InQuint:
                return x * x * x * x * x;
            case EasingType.OutQuint:
                return 1 - math.pow(1 - x, 5);
            case EasingType.InOutQuint:
                return x < 0.5f ? 16 * x * x * x * x * x : 1 - math.pow(-2 * x + 2, 5) / 2;

            case EasingType.InExpo:
                return x == 0 ? 0 : math.pow(2, 10 * x - 10);
            case EasingType.OutExpo:
                return x == 1 ? 1 : 1 - math.pow(2, -10 * x);
            case EasingType.InOutExpo:
                return x == 0 ? 0 : x == 1 ? 1 : x < 0.5f 
                    ? math.pow(2, 20 * x - 10) / 2 : (2 - math.pow(2, -20 * x + 10)) / 2;

            case EasingType.InCirc:
                return 1 - math.sqrt(1 - math.pow(x, 2));
            case EasingType.OutCirc:
                return math.sqrt(1 - math.pow(x - 1, 2));
            case EasingType.InOutCirc:
                return x < 0.5f ? (1 - math.sqrt(1 - math.pow(2 * x, 2))) / 2 
                    : (math.sqrt(1 - math.pow(-2 * x + 2, 2)) + 1) / 2;

            case EasingType.InBack:
                return c3 * x * x * x - c1 * x * x;
            case EasingType.OutBack:
                return 1 + c3 * math.pow(x - 1, 3) + c1 * math.pow(x - 1, 2);
            case EasingType.InOutBack:
                return x < 0.5f ? (math.pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2 
                    : (math.pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;

            case EasingType.InElastic:
                return x == 0 ? 0 : x == 1 ? 1 : -math.pow(2, 10 * x - 10) * math.sin((x * 10 - 10.75f) * c4);
            case EasingType.OutElastic:
                return x == 0 ? 0 : x == 1 ? 1 : math.pow(2, -10 * x) * math.sin((x * 10 - 0.75f) * c4) + 1;
            case EasingType.InOutElastic:
                return x == 0 ? 0 : x == 1 ? 1 : x < 0.5f 
                    ? -(math.pow(2, 20 * x - 10) * math.sin((20 * x - 11.125f) * c5)) / 2 
                    : (math.pow(2, -20 * x + 10) * math.sin((20 * x - 11.125f) * c5)) / 2 + 1;
        }
        return 0;
    }
    #endregion
}
